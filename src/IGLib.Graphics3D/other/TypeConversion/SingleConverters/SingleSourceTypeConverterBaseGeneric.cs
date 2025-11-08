
#nullable disable

using System;
using System.Diagnostics;

namespace IGLib.Core
{

    public abstract class SingleSourceTypeConverterBase<TSource> : ISingleSourceTypeConverter<TSource>
    {

        public Type SourceType { get; } = typeof(TSource);

        public abstract object Convert(object source, Type targetType);

        public bool TryConvert(object source, out object target, Type TargetType)
        {
            try
            {
                target = Convert(source, TargetType);
            }
            catch
            {
                target = null;
                return false;
            }
            return true;
        }

        public abstract TTarget ConvertTyped<TTarget>(TSource source);

        public virtual bool TryConvertTyped<TTarget>(TSource source, out TTarget target)
        {
            try
            {
                target = ConvertTyped<TTarget>(source);
            }
            catch
            {
                target = default;
                return false;
            }
            return true;
        }

    }


}
