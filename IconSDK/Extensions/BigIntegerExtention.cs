using System;
using System.Numerics;
using System.Globalization;
using Newtonsoft.Json;

namespace IconSDK.Extensions
{
    public static class BigIntegerExtention
    {
        public static string ToHex(this BigInteger bigInteger)
        {
            if (bigInteger >= 0)
            {
                var result = bigInteger.ToString("x");
                if (result.Length > 1 && result[0] == '0')
                    result = result.Substring(1);
                return result;
            }
            else
            {
                bigInteger = -bigInteger;
                return $"-{bigInteger.ToString("x")}";
            }
        }

        public static string ToHex0x(this BigInteger bigInteger)
        {
            if (bigInteger >= 0)
            {
                return $"0x{bigInteger.ToHex()}";
            }
            else
            {
                bigInteger = -bigInteger;
                return $"-0x{bigInteger.ToString("x")}";
            }
        }

        public static BigInteger ToBigInteger(this string hex)
        {
            hex = hex.Replace("0x", "00");
            bool isNegative = (hex[0] == '-');
            if (isNegative)
                hex = hex.Replace("-", string.Empty);

            var result = BigInteger.Parse(hex, NumberStyles.AllowHexSpecifier);
            return isNegative ? -result : result;
        }
    }

    public class BigIntegerConverter : JsonConverter<BigInteger>
    {
        public override BigInteger ReadJson(JsonReader reader, Type objectType, BigInteger existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string s = (string)reader.Value;
            return s.ToBigInteger();
        }

        public override void WriteJson(JsonWriter writer, BigInteger value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToHex0x());
        }
    }
}