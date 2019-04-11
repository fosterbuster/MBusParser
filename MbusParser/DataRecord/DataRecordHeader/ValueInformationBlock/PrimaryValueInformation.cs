namespace MBus.DataRecord.DataRecordHeader.ValueInformationBlock
{
    public enum PrimaryValueInformation
    {
#pragma warning disable SA1602 // Enumeration items should be documented
        EnergyWh = 0x07, // E000 0xxx

        EnergyJoule = 0x0F, // E000 1xxx
        Volume = 0x17, // E001 0xxx
        Mass = 0x1F, // E001 1xxx
        OnTime = 0x23, // E010 00xx
        OperatingTime = 0x27, // E010 01xx
        PowerW = 0x2F, // E010 1xxx
        PowerJh = 0x37, // E011 0xxx
        VolumeFlow = 0x3F, // E011 1xxx
        VolumeFlowExt = 0x47, // E100 0xxx
        VolumeFlowExtS = 0x4F, // E100 1xxx
        MassFlow = 0x57, // E101 0xxx
        InletFlowTemperature = 0x5B, // E101 10xx
        ReturnFlowTemperature = 0x5F, // E101 11xx
        TemperatureDifference = 0x63, // E110 00xx
        ExternalTemperature = 0x67, // E110 01xx
        Pressure = 0x6B, // E110 10xx
        Date = 0x6C, // E110 1100
        DateTimeGeneral = 0x6D, // E110 1101
        DateTime = 0x6D, // E110 1101
        ExtendedTime = 0x6D, // E110 1101
        ExtendedDateTime = 0x6D, // E110 1101
        UnitsForHCA = 0x6E, // E110 1110
        ReservedForFutureThirdTableOfValueInformationExtensions = 0x6F, // E110 1111
        AveragingDuration = 0x73, // E111 00xx
        ActualityDuration = 0x77, // E111 01xx
        FabricationNumber = 0x78, // E111 1000
        Identification = 0x79, // E111 1001
        Address = 0x7A, // E111 1010

        // NOT THE ONES FOR SPECIAL PURPOSES
        FBValueInformationExtension = 0xFB, // 1111 1011
        ValueInformationInFollowingString = 0x7C, // E111 1100
        FDValueInformationExtension = 0xFD, // 1111 1101
        ReservedForThirdExtensionOfValueInformationCodes = 0xEF, // 1110 1111
        AnyValueInformation = 0x7E, // E111 1110
        ManufacturerSpecific = 0x7F, // E111 1111

#pragma warning restore SA1602 // Enumeration items should be documented
    }
}