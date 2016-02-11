using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;


public static class StringExtensions
{
    public static string ToCamelCase(this string str)
    {
        return string.Format("{0}{1}", str[0].ToString().ToLower(), str.Substring(1));
    }
}

public static class ObjectExtensions
{
    public static bool IsOfType<T>(this object obj)
    {
        return typeof(T).IsAssignableFrom(obj.GetType());
    }
}

public static class PropertyInfoExtensions
{
    public static bool IsOfType<T>(this PropertyInfo propertyInfo)
    {
        return typeof (T).IsAssignableFrom(propertyInfo.PropertyType);
    }

    public static object GetValue(this PropertyInfo propertyInfo, object obj, object[] index = null)
    {
        return propertyInfo.GetValue(obj, index);
    }
}


public static class ObjectToCSharpCodeExtensions
{
    public static string CreateCode(this object obj)
    {
        var sb = new StringBuilder();

        if (obj.IsOfType<IEnumerable>())
        {
            var genericTypeName = obj.IsOfType<Array>()
                                      ? obj.GetType().GetElementType()
                                      : obj.GetType().GetGenericArguments()[0];
            sb.AppendLine(string.Format("var list = new List<{0}>();", genericTypeName));

            foreach (var item in (IEnumerable) obj)
            {
                var childInstanceName = CreateCode(item, sb);
                sb.AppendLine(string.Format("list.Add({0});", childInstanceName));
            }

            sb.AppendLine(string.Format("var convertedObject = list{0};", obj.IsOfType<Array>() ? ".ToArray()" : ""));
        }
        else
        {
            CreateCode(obj, sb);
        }

        return sb.ToString();
    }

    private static string CreateCode(this object obj, StringBuilder sb)
    {
        var type = obj.GetType();
        // using string builder's length property to keep variable name unique
        string instanceName = string.Format("{0}{1}", type.Name, sb.Length).ToCamelCase();

        sb.AppendLine(string.Format("var {0} = new {1}();", instanceName, type.Name));

        foreach (var propertyInfo in type.GetProperties())
        {
            if (propertyInfo.IsOfType<string>())
            {
                sb.AppendLine(string.Format(@"{0}.{1} = ""{2}"";", instanceName, propertyInfo.Name,
                                            propertyInfo.GetValue(obj)));
            }
            else if (propertyInfo.IsOfType<int>())
            {
                sb.AppendLine(string.Format("{0}.{1} = {2};", instanceName, propertyInfo.Name,
                                            propertyInfo.GetValue(obj)));
            }
            else if (propertyInfo.IsOfType<bool>())
            {
                sb.AppendLine(string.Format("{0}.{1} = {2};", instanceName, propertyInfo.Name,
                                            propertyInfo.GetValue(obj).ToString().ToLower()));
            }
            else if (propertyInfo.IsOfType<IEnumerable>()
                     &&
                     (
                         (propertyInfo.IsOfType<Array>() && propertyInfo.PropertyType.GetArrayRank() == 1)
                         ||
                         (propertyInfo.PropertyType.GetGenericArguments().Count() == 1)
                     ))
            {
                var list = propertyInfo.GetValue(obj);
                if (list != null)
                {
                    var isArray = propertyInfo.IsOfType<Array>();
                    var genericTypeName = isArray
                                              ? propertyInfo.PropertyType.GetElementType()
                                              : propertyInfo.PropertyType.GetGenericArguments()[0];
                    sb.AppendLine(string.Format("var {0} = new List<{1}>();", propertyInfo.Name.ToCamelCase(),
                                                genericTypeName));

                    foreach (var child in (IEnumerable) propertyInfo.GetValue(obj))
                    {
                        var childInstanceName = CreateCode(child, sb);
                        sb.AppendLine(string.Format("{0}.Add({1});", propertyInfo.Name.ToCamelCase(), childInstanceName));
                    }

                    sb.AppendLine(string.Format("{0}.{1} = {2}{3};", instanceName, propertyInfo.Name,
                                                propertyInfo.Name.ToCamelCase(),
                                                isArray ? ".ToArray()" : ""));
                }
            }
            // ignore other unsupported types
        }

        return instanceName;
    }
}
