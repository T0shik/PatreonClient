using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;

namespace PatreonClient
{
    public class Field
    {
        public string Type { get; }
        public List<string> Fields { get; } = new List<string>();

        public Field(string type, Expression selector)
        {
            Type = type;
            var lambda = (LambdaExpression)selector;
            if (lambda.Body is NewExpression ne)
            {
                foreach (var arg in ne.Arguments)
                {
                    if (arg is MemberExpression meme)
                    {
                        var attr = meme.Member.GetCustomAttributes().FirstOrDefault();

                        if (attr is JsonPropertyNameAttribute j)
                        {
                            Fields.Add(j.Name);
                        }
                    }
                }
            };
        }

        public string ToString(string prefix)
        {
            if (Fields.Count > 0)
            {
                return string.Concat(prefix, "fields%5B", Type, "%5D=", string.Join(',', Fields));
            }
            return "";
        }
    }
}