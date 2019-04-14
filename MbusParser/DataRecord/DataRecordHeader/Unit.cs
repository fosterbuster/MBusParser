// <copyright file="Unit.cs" company="Poul Erik Venø Hansen">
// Copyright (c) Poul Erik Venø Hansen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MBus.DataRecord.DataRecordHeader
{
    /// <summary>
    /// SI-Units and filthy imperial units.
    /// </summary>
    public enum Unit
    {
#pragma warning disable SA1602 // Enumeration items should be documented
        Wh,
        J,
        M3,
        L,
        Kg,
        W,
        Jh,
        M3h,
        M3min,
        M3s,
        Kgh,
        C,
        K,
        Bar,
        Date,
        Time,
        DateTime,
        DateTimeSecondary,
        Seconds,
        Minutes,
        Hours,
        Days,
        None,
        V,
        A,
        VAR,
        VARh,
        VAh,
        Cal,
        Hz,
        VA,
        Percentage,
        Rad,
        M3celcius,

        //Filthy imperial units
        Feet,
        Feet3,
        Gallons,
        Fahrenheit,
        Btu,
        BtuPerSecond,
#pragma warning restore SA1602 // Enumeration items should be documented

    }
}