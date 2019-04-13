// <copyright file="EnumUtils.cs" company="Poul Erik Venø Hansen">
// Copyright (c) Poul Erik Venø Hansen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System;

namespace MBus.Helpers
{
    /// <summary>
    /// Helper class for handling enums.
    /// </summary>
    public static class EnumUtils
    {
        /// <summary>
        /// Gets an enum of type <typeparamref name="T"/>, if it is defined.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="value">The value to get an enum from.</param>
        /// <param name="defaultValue">A fall-back value.</param>
        /// <returns><paramref name="value"/> as type <typeparamref name="T"/> or <paramref name="defaultValue"/></returns>
        public static T GetEnumOrDefault<T>(object value, T defaultValue)
        {
            if (!Enum.IsDefined(typeof(T), value))
            {
                return defaultValue;
            }

            return (T)value;
        }

        /// <summary>
        /// Tries to get an enum of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="value">The value to try and get as <typeparamref name="T"/>.</param>
        /// <returns><paramref name="value"/> as <typeparamref name="T"/></returns>
        public static T TryGetEnum<T>(object value)
            where T : struct
        {
            if (!Enum.IsDefined(typeof(T), value))
            {
                throw new ArgumentOutOfRangeException($"{value} is not a valid {typeof(T)}");
            }

            return (T)value;
        }
    }
}