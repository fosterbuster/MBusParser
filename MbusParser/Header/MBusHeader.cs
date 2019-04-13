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
        

        private readonly int _bytesBeforeHeader;

        private (int StartIndex, int Length) _manufacturerIndexer;
        private (int StartIndex, int Length) _serialNumberIndexer;
        private (int StartIndex, int Length) _versionIndexer;
        private (int StartIndex, int Length) _deviceTypeIndexer;

        private (int StartIndex, int Length) _statusIndexer;
        private (int StartIndex, int Length) _accessNumberIndexer;
        private (int StartIndex, int Length) _configurationIndexer;

        /// <summary>
        /// Initializes a new instance of the <see cref="MBusHeader"/> class.
        /// </summary>
        /// <param name="payload">the paylod bytes.</param>
        public MBusHeader(IList<byte> payload, bool controlFieldIsFirstByte)
        {
            _payload = payload;

            if (!controlFieldIsFirstByte)
            {
                IsWireless = payload[0] != WiredMBusStartByte && payload[3] != WiredMBusStartByte;
            }

            if (controlFieldIsFirstByte)
            {
                _ciFieldByte = _payload[2];
                _bytesBeforeHeader = 3;
            }
            else if (IsWireless)
            {
                _ciFieldByte = _payload[10];
                _bytesBeforeHeader = 2;
            }
            else
            {
                _ciFieldByte = _payload[6];
                _bytesBeforeHeader = 7;
            }

            _payload = _payload.Skip(_bytesBeforeHeader).ToList();

            ConfigureIndexers();

            if (HeaderType == HeaderType.ExtendedLinkLayer)
            {
                // Skips length of wmbus linklayer
                ExtendedLinkLayer = new ExtendedLinkLayer(_payload.Skip(8).Take(HeaderLength).ToArray());
            }
        }

        private void ConfigureIndexers()
        {
            if (IsWireless)
            {
                _manufacturerIndexer = (0, 2);
                _serialNumberIndexer = (2, 4);
                _versionIndexer = (6, 1);
                _deviceTypeIndexer = (7, 1);
            }
            else if (HeaderType == HeaderType.Long)
            {
                _serialNumberIndexer = (0, 4);
                _manufacturerIndexer = (4, 2);
                _versionIndexer = (6, 1);
                _deviceTypeIndexer = (7, 1);
                _accessNumberIndexer = (8, 1);
                // todo status
                _configurationIndexer = (10, 2);
            }
        }

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
        public byte VersionNumber => _payload.Skip(_versionIndexer.StartIndex).Take(_versionIndexer.Length).First();

        /// <summary>
        /// Gets the mbus device type.
        /// </summary>
        public MBusTypeCode Type => EnumUtils.GetEnumOrDefault((int)_payload.Skip(_deviceTypeIndexer.StartIndex).Take(_deviceTypeIndexer.Length).First(), MBusTypeCode.UnknownType);

        /// <summary>
        /// Gets the <see cref="FrameType"/> of the variable data record.
        /// </summary>
        public FrameType FrameType => ControlInformationLookup.Find(_ciFieldByte).frameType;

        /// <summary>
        /// Gets the <see cref="HeaderType"/> of the <see cref="Header"/>.
        /// </summary>
        public HeaderType HeaderType => ControlInformationLookup.Find(_ciFieldByte).headerType;

        public Configuration Configuration => new Configuration(_payload.Skip(_configurationIndexer.StartIndex).Take(_configurationIndexer.Length).ToArray());

        internal byte[] GetMAVDBytes => _payload.Take(8).ToArray();

        internal byte GetAccessNumber => _payload.Skip(_accessNumberIndexer.StartIndex).First();

        internal ExtendedLinkLayer? ExtendedLinkLayer { get; private set; }

        internal EncryptionScheme EncryptionScheme => GetEncryptionScheme();

        internal int HeaderLength => ControlInformationLookup.HeaderLength(_ciFieldByte);

        internal bool IsWireless = false;

        // + 1 is to take the CI-field into account.
        internal int PayloadStartsAtIndex => _bytesBeforeHeader + HeaderLength;

        private string GetManufacturerName()
        {
            return BitConverter.ToInt16(_payload.Skip(_manufacturerIndexer.StartIndex).Take(_manufacturerIndexer.Length).ToArray(), 0).ToManufacturerName();
        }

        private int GetSerialNumber()
        {
            return int.Parse(_payload.Skip(_serialNumberIndexer.StartIndex).Take(_serialNumberIndexer.Length).Reverse().ToArray().ToHexString());
        }

        private EncryptionScheme GetEncryptionScheme()
        {
            if (ExtendedLinkLayer != null && ExtendedLinkLayer.SessionNumberField != null)
            {
                return ExtendedLinkLayer.SessionNumberField.EncryptionScheme;
            }

            if (HeaderType == HeaderType.Long)
            {

            }

            return EncryptionScheme.NoEncryption;
        }
    }
}