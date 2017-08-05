using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Astral.Schema
{
    public static class WellKnownTypes
    {
        public static readonly IReadOnlyDictionary<string, Type> TypeByCode =
            new ReadOnlyDictionary<string, Type>(new Dictionary<string, Type>()
            {
                {"bool", typeof(bool)},
                {"u8", typeof(byte) },
                {"i8", typeof(sbyte) },
                {"u16", typeof(ushort) },
                {"i6", typeof(short) },
                {"i32", typeof(int) },
                {"u32", typeof(uint) },
                {"i64", typeof(long) },
                {"u64", typeof(ulong) },
                {"double", typeof(double) },
                {"float", typeof(float) },
                {"string", typeof(string) },
                {"bytes", typeof(byte[]) },
                {"json", typeof(JToken) },
                {"jobject", typeof(JObject) },
                {"jarray", typeof(JArray) },
                {"jvalue", typeof(JValue) },
            });

        public static readonly IReadOnlyDictionary<Type, string> CodeByType =
            new ReadOnlyDictionary<Type, string>(TypeByCode.ToDictionary(p => p.Value, p => p.Key));

        public static string UnitTypeCode { get; } = "unit";
        public static string FailTypeCode { get; } = "error";
    }
}
