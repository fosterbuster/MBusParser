// <copyright file="DataBlock.cs" company="Poul Erik Venø Hansen">
// Copyright (c) Poul Erik Venø Hansen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MBus.DataRecord
{
    public enum ValueDescription
    {
        None,
        Mass,
        MassFlow,
        Energy,
        ApperentEnergy,
        ReactiveEnergy,
        Power,
        ReactivePower,
        ApperentPower,
        Temperature,
        TemperatureDifference,
        ExternalTemperature,
        Volume,
        VolumeFlow,
        ElectricalCurrent, //Amps
        ElectricalPotential, //Volts
        Frequency,
        RelativeHumidity,
        Time,
        PhasePotentialToPotential,
        PhasePotentialToCurrent,
        InletFlowTemperature,
        ReturnFlowTemperature,
        Pressure,
        AveragingDuration,
        ActualityDuration,
        InletEnergy,
        OutletEnergy,
        ErrorFlag,

        // Custom, not part of the Mbus Standard below this point

        AverageInletTemperature, // E8 * Volume, Kamstrup-specific
        AverageOutletTemperature, // E9 * Volume, Kamstrup-specific
        Cooling // Custom for denoting (InletFlow - ReturnFlow) temperature difference (the cooling)
    }
}