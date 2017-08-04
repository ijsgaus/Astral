using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Astral.Schema
{
    public static class AstralSchema
    {
        public static string ToDottedName(this string pascalCaseName, bool stripFirstI)
        {
            var builder = new StringBuilder();
            var first = true;
            foreach (var letter in pascalCaseName)
            {
                if(first && stripFirstI && letter == 'I')
                    continue;
                if (char.IsUpper(letter))
                {
                    if (!first) builder.Append('.');
                    builder.Append(char.ToLower(letter));
                }
                else
                    builder.Append(letter);
                first = false;
            }
            return builder.ToString();

        }

        public static readonly IReadOnlyDictionary<string, Type> PrimitiveTypesByCode =
            new ReadOnlyDictionary<string, Type>(new Dictionary<string, Type>()
            {
                {"bool", typeof(bool)},
                {"uint8", typeof(byte) },
                {"int32", typeof(int) },
                {"int64", typeof(long) },
                {"string", typeof(string) },
                {"byteArray", typeof(byte[]) }
            });

        public static readonly IReadOnlyDictionary<Type, string> PrimitiveCodesByType =
            new ReadOnlyDictionary<Type, string>(PrimitiveTypesByCode.ToDictionary(p => p.Value, p => p.Key));
    }
}