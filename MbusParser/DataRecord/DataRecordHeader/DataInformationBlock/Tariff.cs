// <copyright file="Tariff.cs" company="Poul Erik Venø Hansen">
// Copyright (c) Poul Erik Venø Hansen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace MBus.DataRecord.DataRecordHeader.DataInformationBlock
{
    public sealed class Tariff
    {
        private readonly byte _tariffValue;

        public Tariff(byte tariffValue)
        {
            _tariffValue = tariffValue;
        }
    }
}