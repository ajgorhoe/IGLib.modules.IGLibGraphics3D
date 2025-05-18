using System;
using System.Diagnostics;

namespace IGLib.Core
{

    public class SingleTypeConverterFromDelegate<TSource, TTarget>: SingleTypeConverterBase<TSource, TTarget>, 
        ISingleTypeConverter<TSource, TTarget>
    {

        public SingleTypeConverterFromDelegate(Func<TSource, TTarget> conversionDelegate)
        {
            if (conversionDelegate == null)
            {
                throw new ArgumentNullException(nameof(conversionDelegate), "Conversion delegate cannot be null.");
            }
            ConversionDelegate = conversionDelegate;
        }

        Func<TSource, TTarget> ConversionDelegate { get; init; }

        public override TTarget ConvertTyped(TSource source)
        {
            return ConversionDelegate(source);
        }

    }


}
