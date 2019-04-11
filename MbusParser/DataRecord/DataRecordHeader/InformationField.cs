using MBus.Extensions;

namespace MBus.DataRecord.DataRecordHeader
{
    /// <summary>
    /// Baseclass containing all common functionality for InformationFields
    /// </summary>
    public abstract class InformationField
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InformationField"/> class.
        /// </summary>
        /// <param name="fieldByte">the byte of the field.</param>
        protected InformationField(byte fieldByte)
        {
            FieldByte = fieldByte;
        }

        /// <summary>
        /// Gets a value indicating whether the following byte in the packet is an extension to the current byte.
        /// </summary>
        internal bool HasExtensionBit => FieldByte.HasExtensionBit();

        /// <summary>
        /// Gets the field byte.
        /// </summary>
        protected byte FieldByte { get; private set; }
    }
}