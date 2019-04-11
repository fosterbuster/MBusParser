// <copyright file="ByteExtensions.cs" company="Poul Erik Venø Hansen">
// Copyright (c) Poul Erik Venø Hansen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;

namespace MBus.Extensions
{
    /// <summary>
    /// Bytemanipulation related extensions.
    /// </summary>
    public static class ByteExtensions
    {
        /// <summary>
        /// Transforms a hex-formatted string to a <see cref="byte[]"/>.
        /// </summary>
        /// <param name="hexString">the hex-formatted string</param>
        /// <returns><see cref="byte[]"/>.</returns>
        public static byte[] HexStringToBytes(this string hexString)
        {
            var sanitizedHex = hexString.ToUpper();

            if (sanitizedHex.Length % 2 == 1)
            {
                throw new ArgumentException("The binary key cannot have an odd number of digits");
            }

            var arr = new byte[sanitizedHex.Length >> 1];

            for (var i = 0; i < sanitizedHex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(sanitizedHex[i << 1]) << 4) +
                                      GetHexVal(sanitizedHex[(i << 1) + 1]));
            }

            return arr;
        }

        /// <summary>
        /// Transforms a byte to a hex-formatted string.
        /// </summary>
        /// <param name="b">the byte.</param>
        /// <returns><see cref="string"/>.</returns>
        public static string ToHexString(this byte b)
        {
            return b.ToString("X2");
        }

        /// <summary>
        /// Transforms a <see cref="IList{T}"/> of <see cref="byte"/> to a hex-formatted string.
        /// </summary>
        /// <param name="bytes">the bytes.</param>
        /// <returns><see cref="string"/>.</returns>
        public static string ToHexString(this IList<byte> bytes)
        {
            var c = new char[bytes.Count * 2];
            for (var i = 0; i < bytes.Count; i++)
            {
                var b = bytes[i] >> 4;
                c[i * 2] = (char)(55 + b + (((b - 10) >> 31) & -7));
                b = bytes[i] & 0xF;
                c[(i * 2) + 1] = (char)(55 + b + (((b - 10) >> 31) & -7));
            }

            return new string(c);
        }

        /// <summary>
        /// Gets an integer representation of a manufacturer code string.
        /// </summary>
        /// <param name="manId">the manufacturer code string.</param>
        /// <returns>integer representation of <paramref name="manId"/></returns>
        public static int ToManufacturerCode(this string manId)
        {
            if (manId.Length != 3)
            {
                throw new ArgumentException("Manufacturer Name must no more and no less than 3 letters");
            }

            return ((manId[0] - 64) * 1024) + ((manId[1] - 64) * 32) + (manId[2] - 64);
        }

        public static string ToManufacturerName(this short manId)
        {
            return new string(
               new[]
               {
                    (char)((manId / 1024) + 64),
                    (char)(((manId % 1024) / 32) + 64),
                    (char)((manId % 32) + 64),
               });
        }

        public static string ToManufacturerName(this int manId)
        {
            return ((short)manId).ToManufacturerName();
        }

        public static bool HasExtensionBit(this byte b)
        {
            return b.Mask(0b1000_0000) == 0b1000_0000;
        }

        public static byte Mask(this byte b, byte mask)
        {
            return (byte)(b & mask);
        }

        public static byte Or(this byte b, byte other)
        {
            return (byte)(b | other);
        }

        public static byte ShiftRight(this byte b, byte places)
        {
            return (byte)(b >> places);
        }

        public static byte ShiftLeft(this byte b, byte places)
        {
            return (byte)(b << places);
        }

        private static int GetHexVal(char hex)
        {
            var val = (int)hex;
            //For uppercase A-F letters:
            return val - (val < 58 ? 48 : 55);
        }
    }
}
