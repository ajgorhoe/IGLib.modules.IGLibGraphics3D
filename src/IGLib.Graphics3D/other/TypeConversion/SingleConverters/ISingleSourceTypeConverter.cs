
#nullable disable

using System;

namespace IGLib.Core
{

    public interface ISingleSourceTypeConverter<SourceType> : ISingleSourceTypeConverter
    {
        TargetType ConvertTyped<TargetType>(SourceType source);

        bool TryConvertTyped<TargetType>(SourceType source, out TargetType target);

    }


    public interface ISingleSourceTypeConverter
    {
    
        Type SourceType { get; }
        
        object Convert(object source, Type targetType);

        bool TryConvert(object source, out object target, Type targetType);

    }

}
