namespace MBus.Header
{
    public enum ApplicationStatus
    {
        NoError = 0b00,
        ApplicationBusy = 0b01,

        /// <summary>
        ///  A failure during the interpretation or the execution of a received command, e.g. if a not decrypt able message was received
        /// </summary>
        AnyApplicationError = 0b10,

        /// <summary>
        /// A correct working application detects an abnormal behaviour like a permanent flow of water by a water meter.
        /// </summary>
        AbnormalConditionOrAlarm = 0b11,
    }
}