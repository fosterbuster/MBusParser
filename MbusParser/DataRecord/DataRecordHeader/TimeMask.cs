// <copyright file="TimeMask.cs" company="Poul Erik Venø Hansen">
// Copyright (c) Poul Erik Venø Hansen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MBus.DataRecord.DataRecordHeader
{
    /// <summary>
    /// Helper masks for time stuff.
    /// </summary>
    public enum TimeMask
    {
        // Date masks
#pragma warning disable SA1602 // Enumeration items should be documented
        Date = 0b0010, // 0010 Type G
        DateTime = 0b0100, // 0100 Type F
        TimeExtended = 0b0011, // 0011 Type J
        DateTimeExtended = 0b0110, // 0110 Type I

        // Helper masks
        DayMask = 0x1F,
        DifSummertime = 0xC0,
        HourMask = 0x1F,
        CenturyMask = 0xC0,
        LeapYear = 0x80,
        SecondMask = 0x3F,
        MinuteMask = SecondMask,
        MonthMask = 0x0F,
        Summertime = 0x40,
        TimeInvalid = 0x80,
        Week = 0x3F,
        WeekDay = 0xE0,
        YearMask = 0xE0,
        YearMask2 = 0xF0,
#pragma warning restore SA1602 // Enumeration items should be documented
    }
}