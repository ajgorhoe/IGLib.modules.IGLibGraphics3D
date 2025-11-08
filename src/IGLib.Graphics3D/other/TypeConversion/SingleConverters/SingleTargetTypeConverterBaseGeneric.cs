
#nullable disable

using System;
using System.Diagnostics;

namespace IGLib.Core
{

    public abstract class SingleTargetTypeConverterBase<TTarget> : ISingleTargetTypeConverter<TTarget>
    {

        public Type TargetType { get; } = typeof(TTarget);

        public abstract object Convert(object source);

        public bool TryConvert(object source, out object target)
        {
            try
            {
                target = Convert(source);
            }
            catch
            {
                target = null;
                return false;
            }
            return true;
        }

        public abstract TTarget ConvertTyped<TSource>(TSource source);

        public virtual bool TryConvertTyped<TSource>(TSource source, out TTarget target)
        {
            try
            {
                target = ConvertTyped<TSource>(source);
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
