
#nullable disable

using System;

namespace IGLib.Core
{

    public interface ISingleTargetTypeConverter<TargetType> : ISingleTargetTypeConverter
    {
        TargetType ConvertTyped<SourceType>(SourceType source);

        bool TryConvertTyped<SourceType>(SourceType source, out TargetType target);

    }

    public interface ISingleTargetTypeConverter
    {
    
        Type TargetType { get; }
        
        object Convert(object source);

        bool TryConvert(object source, out object target);

    }

}
