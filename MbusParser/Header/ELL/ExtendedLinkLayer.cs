using System;
using System.Collections.Generic;
using System.Text;

namespace MBus.Header.ELL
{
    public class ExtendedLinkLayer
    {
        private readonly byte[] _bytes;

        public ExtendedLinkLayer(byte[] bytes)
        {
            _bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));
        }


        /// <summary>
        /// Gets the <see cref="CommunicationControlField"/>.
        /// </summary>
        public CommunicationControlField CommunicationControl => new CommunicationControlField(_bytes[0]);

        public byte AccessNumber => _bytes[1];

        internal EncryptionScheme EncryptionScheme;
    }
}