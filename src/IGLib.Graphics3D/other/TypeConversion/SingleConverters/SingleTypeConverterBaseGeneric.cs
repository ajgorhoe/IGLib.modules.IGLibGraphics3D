using System;
using System.Diagnostics;

namespace IGLib.Core
{

    public abstract class SingleTypeConverterBase<TSource, TTarget> : ISingleTypeConverter<TSource, TTarget>
    {
        public Type SrouceType { get; } = typeof(TSource);

        public Type TargetType { get; } = typeof(TTarget);

        Type SourceType { get; } = typeof(TSource);

        Type Targetype { get; } = typeof(TTarget);

        public object Convert(object source)
        {
            return ConvertTyped((TSource)source);
        }

        public bool TryConvert(object source, out object target)
        {
            try
            {
                target = ConvertTyped((TSource)source);
            }
            catch
            {
                target = null;
                return false;
            }
            return true;
        }

        public abstract TTarget ConvertTyped(TSource source);

        public virtual bool TryConvertTyped(TSource source, out TTarget target)
        {
            try
            {
                target = ConvertTyped(source);
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
