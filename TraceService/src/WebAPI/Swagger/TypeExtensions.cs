using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TraceService.WebAPI.Swagger
{
    public static class TypeExtensions
    {
        private static readonly Dictionary<string, string> _friendlyName = new();

        public static string MyFriendlyName(this Type type)
        {
            KeyValuePair<string, string> find = _friendlyName.FirstOrDefault(x => x.Key == type.FullName);

            if (find.Key != null)
            {
                return find.Value;
            }

            string name = type.FullName.Replace("+", ".");

            if (type.GetTypeInfo().IsGenericType)
            {
                string[] genericArgumentIds = type.GetGenericArguments()
                    .Select(t => t.FriendlyId(false))
                    .ToArray();

                string name2 = new StringBuilder(name)
                    .Replace(string.Format("`{0}", genericArgumentIds.Count()), string.Empty)
                    .Append(string.Format("[{0}]", string.Join(",", genericArgumentIds).TrimEnd(',')))
                    .ToString();

                name = name2.Replace("[", "_").Replace("]", "").Replace(",", "_");
            }

            string[] modelPath = name.Split(".");
            string myName = "";
            int i = modelPath.Length - 3;
            i = i < 0 ? 0 : i;
            for (; i < modelPath.Length; i++)
            {
                if (!string.IsNullOrEmpty(myName))
                {
                    myName += ".";
                }

                myName += modelPath[i];
            }

            name = myName;
            int findSameNameCount = _friendlyName.Count(x => x.Value == name);
            name = findSameNameCount > 0 ? $"{name}_{findSameNameCount + 1}" : name;


            _friendlyName.Add(type.FullName, name);
            return name;
        }


        public static string FriendlyId(this Type type, bool fullyQualified = false)
        {
            string typeName = fullyQualified
                ? type.FullNameSansTypeArguments().Replace("+", ".")
                : type.Name;

            if (type.GetTypeInfo().IsGenericType)
            {
                string[] genericArgumentIds = type.GetGenericArguments()
                    .Select(t => t.FriendlyId(fullyQualified))
                    .ToArray();

                return new StringBuilder(typeName)
                    .Replace(string.Format("`{0}", genericArgumentIds.Count()), string.Empty)
                    .Append(string.Format("[{0}]", string.Join(",", genericArgumentIds).TrimEnd(',')))
                    .ToString();
            }

            return typeName;
        }

        internal static bool IsNullable(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        internal static bool IsFSharpOption(this Type type)
        {
            return type.FullNameSansTypeArguments() == "Microsoft.FSharp.Core.FSharpOption`1";
        }

        private static string FullNameSansTypeArguments(this Type type)
        {
            if (string.IsNullOrEmpty(type.FullName))
            {
                return string.Empty;
            }

            string fullName = type.FullName;
            int chopIndex = fullName.IndexOf("[[");
            return chopIndex == -1 ? fullName : fullName[..chopIndex];
        }
    }
}
