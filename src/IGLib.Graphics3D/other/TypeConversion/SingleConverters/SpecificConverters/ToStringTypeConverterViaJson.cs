using System;
using System.Text.Json;

namespace IGLib.Core
{

    /// <summary>
    /// Converts an object of any type to its JSON string representation using System.Text.Json.
    /// Supports an option to format the JSON string with indentation for readability.
    /// </summary>
    public class ToStringTypeConverterViaJson : ISingleTargetTypeConverter<string>
    {
        public Type TargetType => typeof(string);

        /// <summary>
        /// If true, JSON output is formatted with indentation. Default is false.
        /// </summary>
        public bool IndentedJson { get; set; }

        /// <summary>
        /// Constructs the converter, optionally enabling indented JSON formatting.
        /// </summary>
        public ToStringTypeConverterViaJson(bool indentedJson = false)
        {
            IndentedJson = indentedJson;
        }

        public string ConvertTyped<SourceType>(SourceType source)
            => ConvertTyped(source, IndentedJson);

        public string ConvertTyped<SourceType>(SourceType source, bool indented)
        {
            var options = new JsonSerializerOptions { WriteIndented = indented };
            return JsonSerializer.Serialize(source, options);
        }

        public bool TryConvertTyped<SourceType>(SourceType source, out string target)
            => TryConvertTyped(source, out target, IndentedJson);

        public bool TryConvertTyped<SourceType>(SourceType source, out string target, bool indented)
        {
            try
            {
                target = ConvertTyped(source, indented);
                return true;
            }
            catch
            {
                target = null;
                return false;
            }
        }

        public object Convert(object source) => Convert(source, IndentedJson);

        public string Convert(object source, bool indented)
        {
            var options = new JsonSerializerOptions { WriteIndented = indented };
            return JsonSerializer.Serialize(source, source?.GetType() ?? typeof(object), options);
        }

        public bool TryConvert(object source, out object target)
            => TryConvert(source, out target, IndentedJson);

        public bool TryConvert(object source, out object target, bool indented)
        {
            try
            {
                target = Convert(source, indented);
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