using System;
using System.Collections.Generic;
using System.Text;

namespace MBus.DataRecord.DataRecordHeader.ValueInformationBlock.Extension
{
    public enum FBValueInformationExtension
    {
        EnergyMWh = 0x01, //E000 000n 10^(n-1) MWh
        ReactiveEnergy = 0x03, //E000 001n 10^(n) kVARh
        ApparentEnergy = 0x05, //E000 010n 10^(n) kVAh

        //E000 011n Reserved
        EnergyGj = 0x09, //E000 100n 10^(n-1) GJ

        //E000 101n Reserved
        EnergyMcal = 0x0F, //E000 11nn 10^(n-1) MCal
        VolumeCubicMeter = 0x11, //E001 000n 10^(n+2) m^3

        //E001 001n Reserved
        ReactivePower = 0x17, //E001 01nn 10^(nn-3) kVAR
        Mass = 0x19, //E001 100n 10^(n+2) t
        RelativeHumidity = 0x1B, //E001 101n 10^(n-1) %

        // ...
        //E001 1100 - E001 1111
        // ...
        VolumeCubicFeet = 0x20, //E010 0000 feet^3
        VolumeCubicFeet1 = 0x21, //E010 0001 0,1 feet^3

        //E010 0010 Reserved
        //E010 0011 Reserved
        //E010 0100 Reserved
        //E010 0101 Reserved
        //E010 0110 Reserved
        //E010 0111 Reserved
        PowerMw = 0x29, //E010 100n 10^(n-1) MW
        PowerPhaseUU = 0x2A, //E010 1010 Phase U-U (volt. to volt.)
        PowerPhaseUI = 0x2B, //E010 1011 Phase U-I (volt. to current)
        Frequency = 0x2F, //E010 11nn 10^(nn-3) Hz
        PowerGjh = 0x31, //E011 000n 10^(n-1) GJ/h

        //E011 001n Reserved
        ApparentPower = 0x37, //E011 01nn 10^(nn-3) kVA

        //E011 1000 – E101 0111 Reserved
        //E101 1000 – E110 0111 Reserved
        //E110 1nnn Reserved
        //E111 00nn Reserved
        ColdWarmTemperatureLimit = 0x77, //E111 01nn 10^(nn-3) °C
        CumulativeMaxActivePower = 0x7F, //E111 1nnn 10(nnn-3) W

        ReservedE001001n = 0x12,

        ReservedE0100010 = 0x22,
        ReservedE0100011 = 0x23,
        ReservedE0100100 = 0x24,
        ReservedE0100101 = 0x25,
        ReservedE0100110 = 0x26,
        ReservedE0100111 = 0x27,

        ReservedE0011100 = 0x1C,
        ReservedE0011101 = 0x1D,
        ReservedE0011110 = 0x1E,
        ReservedE0011111 = 0x1F,

        ReservedE0111000 = 0x38
    }
}
