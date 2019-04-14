using MBus.Extensions;
using MBus.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MBus.Header
{
    public sealed class Status
    {
        private readonly byte _statusByte;

        public Status(byte statusByte)
        {
            _statusByte = statusByte;
        }

        /// <summary>
        /// Gets a value indicating whether a signal interruption of external power supply or the end of battery life.
        /// </summary>
        public bool LowPower => _statusByte.GetBit(2);

        /// <summary>
        ///  Gets a value indicating whether the meter signals a fatal device error (which requires a service action). Error can be reset only by a service action.
        /// </summary>
        public bool PermanentError => _statusByte.GetBit(3);

        /// <summary>
        /// Gets a value indicating whether the meter signals a slight error condition (which not immediately requires a service action). This error condition may later disappear.
        /// </summary>
        public bool TemporaryError => _statusByte.GetBit(4);

        /// <summary>
        /// Gets the application status.
        /// </summary>
        public ApplicationStatus ApplicationStatus => EnumUtils.TryGetEnum<ApplicationStatus>(_statusByte.Mask(0b11));

        /// <summary>
        /// Gets a value indicating whether ??? specific to manufacturer.
        /// </summary>
        public bool ManufacturerSpecific1 => _statusByte.GetBit(5);

        /// <summary>
        /// Gets a value indicating whether ??? specific to manufacturer.
        /// </summary>
        public bool ManufacturerSpecific2 => _statusByte.GetBit(6);

        /// <summary>
        /// Gets a value indicating whether ??? specific to manufacturer.
        /// </summary>
        public bool ManufacturerSpecific3 => _statusByte.GetBit(7);
    }
}