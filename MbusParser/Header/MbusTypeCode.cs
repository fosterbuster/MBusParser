// <copyright file="MbusTypeCode.cs" company="Poul Erik Venø Hansen">
// Copyright (c) Poul Erik Venø Hansen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MBus.Header
{
    /// <summary>
    /// MBus sensor type.
    /// </summary>
    public enum MBusTypeCode
    {
#pragma warning disable SA1602 // Enumeration items should be documented
        Other = 0x00,
        Oil = 0x01,
        Electricity = 0x02,
        Gas = 0x03,
        HeatOutlet = 0x04,
        Steam = 0x05,
        WarmWater = 0x06,
        Water = 0x07,
        HeatCostAllocator = 0x08,
        CompressedAir = 0x09,
        CoolingLoadMeterOutlet = 0x0A,
        CoolingLoadMeterInlet = 0x0B,
        HeatInlet = 0x0C,
        HeatCoolingLoadMeter = 0x0D,
        BusSystem = 0x0E,
        UnknownMedium = 0x0F,
        HotWater = 0x15,
        ColdWater = 0x16,
        DualWater = 0x17,
        Pressure = 0x18,
        AdConverter = 0x19,
        SmokeDetector = 0x1A,
        RoomSensor = 0x1B,
        GasDetector = 0x1C,
        Breaker = 0x20,
        Valve = 0x21,
        CustomerUnit = 0x25,
        WasteWaterMeter = 0x28,
        Garbage = 0x29,
        CommunicationController = 0x31,
        UnidirectionalRepeater = 0x32,
        BidirectionalRepeater = 0x33,
        RadioConverterSystemSide = 0x36,
        RadioConverterMeterSide = 0x37,

#pragma warning restore SA1602 // Enumeration items should be documented

        /// <summary>
        ///     Not an actual part of the Mbus standard, but simply implemented since the parser is not built on an updated standard
        /// </summary>
        UnknownType = -1,

    }
}