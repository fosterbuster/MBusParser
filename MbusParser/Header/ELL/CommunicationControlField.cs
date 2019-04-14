// <copyright file="CommunicationControlField.cs" company="Poul Erik Venø Hansen">
// Copyright (c) Poul Erik Venø Hansen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using MBus.Extensions;

namespace MBus.Header.ELL
{
    /// <summary>
    /// Contains bits related to communication control. 
    /// </summary>
    public class CommunicationControlField
    {
        private readonly byte _ccFieldByte;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommunicationControlField"/> class.
        /// </summary>
        /// <param name="ccFieldByte">the byte of the field.</param>
        public CommunicationControlField(byte ccFieldByte)
        {
            _ccFieldByte = ccFieldByte;
        }

        /// <summary>
        /// Gets a value indicating whether the message is repeated? This field is reserved for use in repeated messages; see EN 13757-5. A Meter shall always set <see cref="RepeatedAccess"/> to 0 and may ignore this bit when received.
        /// </summary>
        public bool RepeatedAccess => _ccFieldByte.GetBit(1);

        /// <summary>
        /// Gets a value indicating whether access to the meter is unlimited or limited, at least until next Meter transmission. This bit has to be used in context with the <see cref="BiDirectional"/> bit. 0 indicates limited access.
        /// </summary>
        public bool Accessibility => _ccFieldByte.GetBit(2);

        /// <summary>
        /// Gets a value indicating whether  the frame is prioritised, i.e. the data shall be transported as fast as possible, and if necessary, delaying other frames in the system.
        /// Only frames containing alarms and other non-frequent data shall utilise this bit.
        /// </summary>
        public bool Priority => _ccFieldByte.GetBit(3);

        /// <summary>
        /// Gets a value indicating whether the frame is unidirectional or bidirectional. 0 indicates that this frame is a
        /// unidirectional frame. 1 indicates that this frame is a bidirectional frame, i.e. that the Meter will be ready to
        /// receive a response after a response delay. This bit has to be used in context with the <see cref="Accessibility"/> bit.
        /// </summary>
        public bool BiDirectional => _ccFieldByte.GetBit(7);

        /// <summary>
        /// Gets a value indicating whether the frame has been relayed by a repeater.
        /// </summary>
        public bool HopCount => _ccFieldByte.GetBit(4);

        /// <summary>
        /// Gets a value indicating whether the responding unit shall respond after a fast response delay. 1 = fast, 0 = slow. See Annex E of EN 13757-4:2013.
        /// </summary>
        public bool ResponseDelay => _ccFieldByte.GetBit(6);

        /// <summary>
        /// Gets a value indicating whether the frame is syncronized as specified in EN 13757-4:2013 11.6.2.
        /// </summary>
        public bool Synchronized => _ccFieldByte.GetBit(5);
    }
}