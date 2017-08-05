using Newtonsoft.Json.Linq;
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

        
    }
}