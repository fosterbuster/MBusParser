using System;
using MBus.Extensions;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MBus.Header.ELL
{
    public class ExtendedLinkLayer
    {
        private readonly byte[] _bytes;
        private readonly byte[] _sessionNumberBytes;

        public ExtendedLinkLayer(byte[] bytes)
        {
            _bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));
            _sessionNumberBytes = _bytes.Skip(3).Take(4).ToArray();
        }


        /// <summary>
        /// Gets the <see cref="CommunicationControlField"/>.
        /// </summary>
        public CommunicationControlField CommunicationControl => new CommunicationControlField(_bytes[1]);

        public byte AccessNumber => _bytes[1];

        public SessionNumberField? SessionNumberField => GetSessionNumberField();

        internal byte[] GetSessionNumberBytes => _sessionNumberBytes;

        internal byte GetCommunicationControlByte => _bytes[1];

        private SessionNumberField? GetSessionNumberField()
        {
            if (_bytes.Length == 8)
            {
                return new SessionNumberField(_sessionNumberBytes);
            }
            else if (_bytes.Length == 16)
            {
                return new SessionNumberField(_sessionNumberBytes);
            }

            return null;
        }
    }
}