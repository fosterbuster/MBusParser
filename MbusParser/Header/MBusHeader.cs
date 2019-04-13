// <copyright file="MBusHeader.cs" company="Poul Erik Venø Hansen">
// Copyright (c) Poul Erik Venø Hansen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;

using MBus.Extensions;
using MBus.Header.ELL;
using MBus.Helpers;

namespace MBus.Header
{
    /// <summary>
    /// Logical encapsulation of both wired and wireless mbus-header.
    /// </summary>
    public sealed class MBusHeader
    {
        private const byte WiredMBusStartByte = 0x68;
        private readonly IList<byte> _payload;

        private readonly byte _ciFieldByte;

        private (int startIndex, int length) _manufacturerIndexer;
        private (int startIndex, int length) _serialNumberIndexer;
        private (int startIndex, int length) _versionIndexer;
        private (int startIndex, int length) _deviceTypeIndexer;
        private (int startIndex, int length) _controlInformationFieldIndexer;

        /// <summary>
        /// Initializes a new instance of the <see cref="MBusHeader"/> class.
        /// </summary>
        /// <param name="payload">the paylod bytes.</param>
        public MBusHeader(IList<byte> payload, bool messageLenghtIsFirstByte = false)
        {
            _payload = payload;
            _controlInformationFieldIndexer = (0, 1);

            bool isWireless = payload[0] != WiredMBusStartByte && payload[3] != WiredMBusStartByte;
            if (isWireless)
            {
                BytesBeforeHeader = 1;
                _manufacturerIndexer = (3, 2);
                _serialNumberIndexer = (5, 8);
                _versionIndexer = (9, 1);
                _deviceTypeIndexer = (10, 1);
            }
            else
            {
                if (messageLenghtIsFirstByte)
                {
                    BytesBeforeHeader = 1;
                }
                else
                {
                    BytesBeforeHeader = 6;
                }

                _manufacturerIndexer = (5, 2);
                _serialNumberIndexer = (1, 4);
                _versionIndexer = (7, 1);
                _deviceTypeIndexer = (8, 1);
            }

            _payload = _payload.Skip(BytesBeforeHeader).ToList();
            _ciFieldByte = _payload.Skip(_controlInformationFieldIndexer.startIndex).Take(_controlInformationFieldIndexer.length).First();

            if (HeaderType == HeaderType.ExtendedLinkLayer)
            {
                // Skips CI-field
                ExtendedLinkLayer = new ExtendedLinkLayer(_payload.Skip(1).Take(HeaderLength).ToArray());
            }
        }

        internal ExtendedLinkLayer? ExtendedLinkLayer { get; private set; }

        /// <summary>
        /// Gets the manufacturer name.
        /// </summary>
        public string ManufacturerName => GetManufacturerName();

        /// <summary>
        /// Gets the serial number.
        /// </summary>
        public int SerialNumber => GetSerialNumber();

        /// <summary>
        /// Gets the version number.
        /// </summary>
        public byte VersionNumber => _payload.Skip(_versionIndexer.startIndex).Take(_versionIndexer.length).First();

        /// <summary>
        /// Gets the mbus device type.
        /// </summary>
        public MBusTypeCode Type => EnumUtils.GetEnumOrDefault((int)_payload.Skip(_deviceTypeIndexer.startIndex).Take(_deviceTypeIndexer.length).First(), MBusTypeCode.UnknownType);

        /// <summary>
        /// Gets the <see cref="FrameType"/> of the variable data record.
        /// </summary>
        public FrameType FrameType => ControlInformationLookup.Find(_ciFieldByte).frameType;

        /// <summary>
        /// Gets the <see cref="HeaderType"/> of the <see cref="Header"/>
        /// </summary>
        public HeaderType HeaderType => ControlInformationLookup.Find(_ciFieldByte).headerType;

        internal EncryptionScheme EncryptionScheme => GetEncryptionScheme();

        private EncryptionScheme GetEncryptionScheme()
        {
            if (ExtendedLinkLayer != null)
            {
                return ExtendedLinkLayer.EncryptionScheme;
            }



            return EncryptionScheme.NoEncryption;
        }

        internal int HeaderLength => ControlInformationLookup.HeaderLength(_ciFieldByte);

        internal int BytesBeforeHeader { get; private set; }

        // + 1 is to take the CI-field into account.
        internal int PayloadStartsAtIndex => BytesBeforeHeader + HeaderLength + 1;

        private string GetManufacturerName()
        {
            return BitConverter.ToInt16(_payload.Skip(_manufacturerIndexer.startIndex).Take(_manufacturerIndexer.length).ToArray(), 0).ToManufacturerName();
        }

        private int GetSerialNumber()
        {
            return BitConverter.ToInt32(_payload.Skip(_serialNumberIndexer.startIndex).Take(_serialNumberIndexer.length).Reverse().ToArray());
        }
    }
}