using System;

namespace IGLib.Core
{

    public interface IStringToTypeConverter<TargetType> : ISingleTypeConverter<string, TargetType>
    {  }

    public interface ITypeToStringConverter<SourceType> : ISingleTypeConverter<SourceType, string>
    {  }

    public interface ISingleTypeConverter<SourceType, TargetType> : ISingleTypeConverter
    {
        TargetType ConvertTyped(SourceType source);

        bool TryConvertTyped(SourceType source, out TargetType target);

    }

    public interface ISingleTypeConverter
    {
    
        Type SrouceType { get; }
        
        Object Convert(object source);

        bool TryConvert(object source, out object target);

    }

}
