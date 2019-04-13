// <copyright file="MBusParser.cs" company="Poul Erik Venø Hansen">
// Copyright (c) Poul Erik Venø Hansen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using MBus.DataRecord;
using MBus.DataRecord.DataRecordHeader.DataInformationBlock;
using MBus.DataRecord.DataRecordHeader.ValueInformationBlock;
using MBus.DataRecord.DataRecordHeader.ValueInformationBlock.Extension;
using MBus.Extensions;
using MBus.Extensions.Decryption;
using MBus.Header;

namespace MBus
{
    /// <summary>
    /// Parser for both wired and wireless mbus telegrams.
    /// </summary>
    public sealed class MBusParser
    {
        /// <summary>
        /// parse the header.
        /// </summary>
        /// <param name="payload">the payload.</param>
        /// <returns>the mbus header.</returns>
        public MBusHeader ParseHeader(IList<byte> payload, bool controlFieldIsFirstByte = false)
        {
            return new MBusHeader(payload, controlFieldIsFirstByte);
        }

        /// <summary>
        /// parse the payload.
        /// </summary>
        /// <param name="header">the mbus header. Used for manufacturer specific parsing.</param>
        /// <param name="payload">the payload.</param>
        /// <param name="decryptionKey">the decryption key.</param>
        /// <param name="initializationVector">the initialization vector.</param>
        /// <returns>the mbus telegram.</returns>
        public MBusTelegram ParsePayload(MBusHeader header, IList<byte> payload, byte[] decryptionKey = null)
        {
            double? rssi = null;
            // todo refactor to be able to return rssi

            if (header.IsWireless)
            {
                rssi = -payload.Last() / 2;
                payload = payload.Take(payload.Count - 1).ToArray();
            }

            IList<VariableDataRecord>? records = this.ParseRecords(
                header,
                payload,
                decryptionKey);

            return new MBusTelegram(header, records, rssi);
        }

        /// <summary>
        /// Parse an entire mbus telegram.
        /// </summary>
        /// <param name="payload">the bytes of the telegram.</param>
        /// <param name="decryptionKey">optional decryption key</param>
        /// <param name="initializationVector">optional IV</param>
        /// <returns>A <see cref="MBusTelegram"/>.</returns>
        public MBusTelegram Parse(IList<byte> payload, byte[] decryptionKey = null)
        {
            var header = this.ParseHeader(payload);

            return this.ParsePayload(
                header,
                payload,
                decryptionKey);
        }

        /// <summary>
        /// parse the records.
        /// </summary>
        /// <param name="header">the mbus header. Useful for manufacturer specific parsing.</param>
        /// <param name="payload">the payload.</param>
        /// <param name="decryptionKey">the decryption key.</param>
        /// <param name="initializationVector">the initialization vector.</param>
        /// <returns>a IList<MbusRecord> containing the parse records.</returns>
        private IList<VariableDataRecord> ParseRecords(MBusHeader header, IList<byte> payload, byte[] decryptionKey)
        {
            List<byte> payloadBody;

            // Todo find out whats wrong with this.
            if(header.ExtendedLinkLayer != null)
            {
                payloadBody = payload.Skip(header.PayloadStartsAtIndex + 7).ToList();
            }
            else
            {
                payloadBody = payload.Skip(header.PayloadStartsAtIndex).ToList();
            }
            

            if (decryptionKey != null)
            {
                payloadBody = DecryptPayload(header, payloadBody, decryptionKey);
            }

            if (header.ExtendedLinkLayer != null)
            {
                var frameType = ControlInformationLookup.Find(payloadBody.First()).frameType;


                if (frameType != FrameType.Full)
                {
                    return null;
                }

                payloadBody = payloadBody.Skip(1).ToList();
            }

            var list = new List<VariableDataRecord>();

            // MBus has a maximum length of 255.
            byte indexer = 0;

            while (indexer <= payloadBody.Count)
            {
                var difFieldByte = payloadBody[indexer++];

                // TODO Handle special case stuff
                if (difFieldByte == 0x1F || difFieldByte == 0x0F)
                {
                    break;
                }

                var dataInformationField = new DataInformationField(difFieldByte);
                var dataInformationFieldExtensions = new List<DataInformationExtensionField>();

                var difesAvailable = dataInformationField.HasExtensionBit;
                while (difesAvailable)
                {
                    var extension = new DataInformationExtensionField(payloadBody[indexer++]);
                    dataInformationFieldExtensions.Add(extension);

                    difesAvailable = extension.HasExtensionBit;
                }

                var valueInformationField = new PrimaryValueInformationField(payloadBody[indexer++], dataInformationField.DataField);
                var valueInformationFieldExtensions = new List<ValueInformationExtensionField>();

                var vifesAvailable = valueInformationField.HasExtensionBit;
                while (vifesAvailable)
                {
                    ValueInformationExtensionField? extension = null;
                    if (!valueInformationFieldExtensions.Any())
                    {
                        switch (valueInformationField.Type)
                        {
                            case PrimaryValueInformation.FBValueInformationExtension:
                                extension = new FBValueInformationExtensionField(payloadBody[indexer++]);
                                break;
                            case PrimaryValueInformation.FDValueInformationExtension:
                                extension = new FDValueInformationExtensionField(payloadBody[indexer++]);
                                break;
                            case PrimaryValueInformation.ManufacturerSpecific:
                                switch (header.ManufacturerName.ToLowerInvariant())
                                {
                                    case "kam":
                                        extension = new KamstrupValueInformationExtensionField(payloadBody[indexer++]);
                                        break;
                                }

                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        extension = new OrthogonalValueInformationExtensionField(payloadBody[indexer++]);
                    }

                    valueInformationFieldExtensions.Add(extension);

                    vifesAvailable = extension.HasExtensionBit;
                }

                // is LVAR, the length of the data is in the first byte of the "real" data.
                if (dataInformationField.DataLength == null && dataInformationField.DataField == DataField.VariableLength)
                {
                    dataInformationField.DataLength = payloadBody[indexer++];
                }

                var dataBlock = new DataBlock(payloadBody.Skip(indexer).Take(dataInformationField.DataLength.Value).ToArray(), dataInformationField, dataInformationFieldExtensions, valueInformationField, valueInformationFieldExtensions);
                indexer = (byte)(indexer + dataInformationField.DataLength.Value);

                list.Add(new VariableDataRecord(dataBlock));
            }


            return list;
        }

        private List<byte> DecryptPayload(MBusHeader header, List<byte> payloadBody, byte[] decryptionKey)
        {
            var encryptionScheme = header.EncryptionScheme;
            if (encryptionScheme == EncryptionScheme.NoEncryption)
            {
                return payloadBody;
            }

            var initializationVector = GenerateIv(header);

            List<byte>? decryptionResult = null;
            IEnumerable<byte> bytesToBeDecrypted;
            var remainderBytes = new List<byte>();

            // For CTR we need to decrypt the entire payload from the Extended linklayer. Plus the two CRC bytes before the payload (these need to be removed after decryption)
            if (encryptionScheme == EncryptionScheme.AesCtr)
            {
                bytesToBeDecrypted = payloadBody;
            }
            else
            {
                // It is CBC, DES, or something else than CTR
                // These encryption schemes can be implemented in two ways:
                // 1 - All of the payload is encrypted, and if the payload isnt exactly a fit with encryption-block sizes, it puts "fill bytes" (2F) in the trailer of the (unencrypted) payload.
                // 2 - It encrypts what it can, and leaves a remainder of unencrypted bytes in the end of the payload.
                int decryptionBlockSize;

                // For 128-bit AES a decryption blocksize is 16
                // For 128-bit DES it is 8
                if (encryptionScheme == EncryptionScheme.DesCbcIvIsZero || encryptionScheme == EncryptionScheme.DesCbc)
                {
                    decryptionBlockSize = 8;
                }
                else
                {
                    decryptionBlockSize = 16;
                }

                // Find out how many encrypted blocks the payload contains
                var numberOfBlocks = header.Configuration.NumberOfEncryptedBlocks;

                // Find out how many of the bytes in the payload are actually encrypted
                var numberOfBytesToDecrypt = decryptionBlockSize * numberOfBlocks;

                // Retrieve the number of bytes to decrypted
                bytesToBeDecrypted = payloadBody.Take(numberOfBytesToDecrypt);

                // If there is a remainder, keep it for later use.
                remainderBytes = payloadBody.Skip(numberOfBytesToDecrypt).ToList();
            }

            decryptionResult = DecryptBytes(decryptionKey, initializationVector, bytesToBeDecrypted, encryptionScheme);

            int decryptionValidationSize = 2;
            // The first two bytes of CTR decryption is CRC bytes, and first two bytes of any other is "validation bytes" (2 x 0x2F), extract these in order to validate if decryption was successful.

            if (!DecryptionSuccessful(decryptionResult.ToArray(), encryptionScheme))
            {
                //throw new MBusDecryptionException($"Decryption-check failed. Key: {key.ToHexString()}, IV: {initializationVector.ToHexString()}");
                return null;
            }

            // Remove the two validation bytes
            decryptionResult = decryptionResult.Skip(decryptionValidationSize).ToList();

            // Add the last bunch of bytes which were sent unencrypted, if there is any
            decryptionResult.AddRange(remainderBytes);

            return decryptionResult;
        }

        private bool DecryptionSuccessful(byte[] decryptionResult, EncryptionScheme encryptionScheme)
        {
            if (encryptionScheme != EncryptionScheme.AesCtr)
            {
                //check if decryption verification is correct
                if (decryptionResult[0] != 0x2F || decryptionResult[1] != 0x2F)
                {
                    return false;
                }
            }
            else
            {
                var crc = CalculateCrc(decryptionResult.Skip(2));
                if (crc[0] != decryptionResult[0] && crc[1] != decryptionResult[1])
                {
                    return false;
                }
            }

            return true;
        }

        private static byte[] CalculateCrc(IEnumerable<byte> bytes)
        {
            return ComputeCrc(bytes, 0x3D65, 0x0000, 0xFFFF);
        }

        private static byte[] ComputeCrc(IEnumerable<byte> bytes, int poly, int initialValue, int xorValue)
        {
            var crcVal = initialValue;
            var crc = new byte[2];

            foreach (var b in bytes)
            {
                int i;
                for (i = 0x80; i != 0; i >>= 1)
                {
                    if ((crcVal & 0x8000) != 0)
                    {
                        crcVal = (crcVal << 1) ^ poly;
                    }
                    else
                    {
                        crcVal = crcVal << 1;
                    }

                    if ((b & i) != 0)
                    {
                        crcVal ^= poly;
                    }
                }
            }

            var xoredValue = crcVal & 0xffff ^ xorValue;

            var tmp = xoredValue.ToString("X4").HexStringToBytes();

            var tmpCrc = tmp.Reverse().ToArray();

            crc[0] = tmpCrc[0];
            crc[1] = tmpCrc[1];

            return crc;
        }

        private List<byte> DecryptBytes(byte[] decryptionKey, byte[] initializationVector, IEnumerable<byte> bytesToBeDecrypted, EncryptionScheme encryptionScheme)
        {
            switch (encryptionScheme)
            {
                case EncryptionScheme.DesCbcIvIsZero:
                    return MbusCryptoProvider.DecryptDes(decryptionKey, new byte[16], bytesToBeDecrypted.ToArray()).ToList();
                case EncryptionScheme.DesCbc:
                    return MbusCryptoProvider.DecryptDes(decryptionKey, initializationVector, bytesToBeDecrypted.ToArray()).ToList();
                case EncryptionScheme.AesCbc:
                    return MbusCryptoProvider.DecryptAes128Cbc(decryptionKey, initializationVector, bytesToBeDecrypted.ToArray()).ToList();
                case EncryptionScheme.AesCtr:
                    return MbusCryptoProvider.DecryptAes128Ctr(decryptionKey, initializationVector, bytesToBeDecrypted.ToArray()).ToList();
                default:
                    throw new ArgumentOutOfRangeException("Unhandled encryption scheme. Scheme was " + encryptionScheme);
            }
        }

        private byte[] GenerateIv(MBusHeader header)
        {
            if (header.EncryptionScheme == EncryptionScheme.AesCtr)
            {
                return GenerateCounterModeIV(header);
            }

            return GenerateGenericIV(header);
        }

        private byte[] GenerateCounterModeIV(MBusHeader header, byte[]? frameNumber = null, byte blockCounter = 0b00000000)
        {
            var extendedLinkLayer = header.ExtendedLinkLayer;

            if (frameNumber == null)
            {
                frameNumber = new byte[] { 0x00, 0x00 };
            }

            var initializationVector = new List<byte>();



            initializationVector.AddRange(header.GetMAVDBytes);
            byte commControl = extendedLinkLayer.GetCommunicationControlByte.Mask(0b1110_1111);

            initializationVector.Add(commControl);

            initializationVector.AddRange(extendedLinkLayer.GetSessionNumberBytes);
            initializationVector.AddRange(frameNumber);
            initializationVector.Add(blockCounter);

            return initializationVector.ToArray();
        }

        private byte[] GenerateGenericIV(MBusHeader header)
        {
            var initializationVector = new List<byte>();

            initializationVector.AddRange(header.GetMAVDBytes);

            for (var i = 0; i < 8; i++)
            {
                initializationVector.Add(header.GetAccessNumber); //Access number is one byte
            }

            return initializationVector.ToArray();
        }
    }
}