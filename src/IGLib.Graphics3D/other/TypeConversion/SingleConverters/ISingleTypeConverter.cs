using System;

namespace IGLib.Core
{

    public interface ISingleTypeConverter<SourceType, TargetType> : ISingleTypeConverter
    {
        TargetType ConvertTyped(SourceType source);

        bool TryConvertTyped(SourceType source, out TargetType target);

    }

    public interface ISingleTypeConverter
    {
    
        Type SrouceType { get; }
        
        Type TargetType { get; }

        Object Convert(object source);

        bool TryConvert(object source, out object target);

    }

}
