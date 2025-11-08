
#nullable disable

using System;
using System.Text.Json;

namespace IGLib.Core
{

    /// <summary>
    /// Converts a JSON string to a strongly typed object using System.Text.Json.
    /// </summary>
    public class FromStringTypeConverterViaJson : ISingleSourceTypeConverter<string>
    {
        public Type SourceType => typeof(string);

        public TargetType ConvertTyped<TargetType>(string source)
        {
            return JsonSerializer.Deserialize<TargetType>(source);
        }

        public bool TryConvertTyped<TargetType>(string source, out TargetType target)
        {
            try
            {
                target = JsonSerializer.Deserialize<TargetType>(source);
                return true;
            }
            catch
            {
                target = default;
                return false;
            }
        }

        public object Convert(object source, Type targetType)
        {
            return JsonSerializer.Deserialize((string)source, targetType);
        }

        public bool TryConvert(object source, out object target, Type targetType)
        {
            try
            {
                target = JsonSerializer.Deserialize((string)source, targetType);
                return true;
            }
            catch
            {
                target = null;
                return false;
            }
        }

    }

}