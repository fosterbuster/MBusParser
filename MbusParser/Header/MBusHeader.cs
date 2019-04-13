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
        private readonly bool _isWireless = false;

        private readonly int _bytesBeforeHeader;

        private (int startIndex, int length) _manufacturerIndexer;
        private (int startIndex, int length) _serialNumberIndexer;
        private (int startIndex, int length) _versionIndexer;
        private (int startIndex, int length) _deviceTypeIndexer;

        /// <summary>
        /// Initializes a new instance of the <see cref="MBusHeader"/> class.
        /// </summary>
        /// <param name="payload">the paylod bytes.</param>
        public MBusHeader(IList<byte> payload, bool controlFieldIsFirstByte)
        {
            _payload = payload;

            if (!controlFieldIsFirstByte)
            {
                _isWireless = payload[0] != WiredMBusStartByte && payload[3] != WiredMBusStartByte;
            }

            if (controlFieldIsFirstByte)
            {
                _ciFieldByte = _payload[2];
                _bytesBeforeHeader = 3;
            }
            else if (_isWireless)
            {
                _ciFieldByte = _payload[10];
                _bytesBeforeHeader = 11;
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
                // Skips CI-field
                ExtendedLinkLayer = new ExtendedLinkLayer(_payload.Take(HeaderLength).ToArray());
            }
        }

        private void ConfigureIndexers()
        {
            if (_isWireless)
            {
                _manufacturerIndexer = (0, 2);
                _serialNumberIndexer = (2, 4);
                _versionIndexer = (6, 1);
                _deviceTypeIndexer = (7, 1);
            }
            else
            {
                if (HeaderType == HeaderType.Long)
                {
                    _serialNumberIndexer = (0, 4);
                    _manufacturerIndexer = (4, 2);
                    _versionIndexer = (6, 1);
                    _deviceTypeIndexer = (7, 1);

                }
                else if (HeaderType == HeaderType.Short)
                {
                    throw new NotSupportedException("Short header not supported");
                }
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

        internal ExtendedLinkLayer? ExtendedLinkLayer { get; private set; }

        internal EncryptionScheme EncryptionScheme => GetEncryptionScheme();

        internal int HeaderLength => ControlInformationLookup.HeaderLength(_ciFieldByte);

        // + 1 is to take the CI-field into account.
        internal int PayloadStartsAtIndex => _bytesBeforeHeader + HeaderLength;

        private string GetManufacturerName()
        {
            return BitConverter.ToInt16(_payload.Skip(_manufacturerIndexer.startIndex).Take(_manufacturerIndexer.length).ToArray(), 0).ToManufacturerName();
        }

        private int GetSerialNumber()
        {
            return BitConverter.ToInt32(_payload.Skip(_serialNumberIndexer.startIndex).Take(_serialNumberIndexer.length).Reverse().ToArray());
        }

        private EncryptionScheme GetEncryptionScheme()
        {
            if (ExtendedLinkLayer != null)
            {
                return ExtendedLinkLayer.EncryptionScheme;
            }

            if (HeaderType == HeaderType.Long)
            {

            }

            return EncryptionScheme.NoEncryption;
        }
    }
}