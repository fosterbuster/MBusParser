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
        public MBusTelegram ParsePayload(MBusHeader header, IList<byte> payload, IList<byte>? decryptionKey = null, IList<byte>? initializationVector = null)
        {
            IList<VariableDataRecord>? records = this.ParseRecords(
                header,
                payload,
                decryptionKey,
                initializationVector);

            double? rssi = null;

            return new MBusTelegram(header, records, rssi);
        }

        /// <summary>
        /// Parse an entire mbus telegram.
        /// </summary>
        /// <param name="payload">the bytes of the telegram.</param>
        /// <param name="decryptionKey">optional decryption key</param>
        /// <param name="initializationVector">optional IV</param>
        /// <returns>A <see cref="MBusTelegram"/>.</returns>
        public MBusTelegram Parse(IList<byte> payload, IList<byte>? decryptionKey = null, IList<byte>? initializationVector = null)
        {
            var header = this.ParseHeader(payload);

            return this.ParsePayload(
                header,
                payload,
                decryptionKey,
                initializationVector);
        }

        /// <summary>
        /// parse the records.
        /// </summary>
        /// <param name="header">the mbus header. Useful for manufacturer specific parsing.</param>
        /// <param name="payload">the payload.</param>
        /// <param name="decryptionKey">the decryption key.</param>
        /// <param name="initializationVector">the initialization vector.</param>
        /// <returns>a IList<MbusRecord> containing the parse records.</returns>
        private IList<VariableDataRecord> ParseRecords(MBusHeader header, IList<byte> payload, IList<byte>? decryptionKey, IList<byte>? initializationVector)
        {
            var payloadBody = payload.Skip(header.PayloadStartsAtIndex).ToList();

            if (decryptionKey != null)
            {
                payloadBody = DecryptPayload(header, payloadBody, decryptionKey, initializationVector);
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

        private List<byte> DecryptPayload(MBusHeader header, List<byte> payloadBody, IList<byte> decryptionKey, IList<byte>? initializationVector)
        {
            throw new NotImplementedException();
        }
    }
}