
#nullable disable

using System;
using System.Diagnostics;

namespace IGLib.Core
{

    /// <summary>Abstract base implementation of <see cref="ISingleTypeConverter{TSource, TTarget}"/>.
    /// Sets <see cref="SourceType"/> and <see cref="TargetType"/> properties according to generic type parameters
    /// <typeparamref name="TSource"/> and <typeparamref name="TTarget"/>, respectively, and implements all methods
    /// by calling their counterparts. Implementatkion based on this base class must override and properly implement
    /// either the <see cref="TryConvertTyped(TSource, out TTarget)"/> or the <see cref="ConvertTyped(TSource)"/>
    /// method.</summary>
    /// <typeparam name="TSource">Type of the objects that can be converted to <typeparamref name="TTarget"/> by
    /// the current converter.</typeparam>
    /// <typeparam name="TTarget">Type to which source object of type <typeparamref name="TSource"/> are converted
    /// by the current converter.</typeparam>
    public abstract class SingleTypeConverterBase<TSource, TTarget> : ISingleTypeConverter<TSource, TTarget>
    {

        /// <inheritdoc/>
        public Type SourceType { get; } = typeof(TSource);

        /// <inheritdoc/>
        public Type TargetType { get; } = typeof(TTarget);

        /// <summary>Counter of calls to prevent infinite recursion, which would occur if none of the methods are
        /// overridden and properly implemented (because all method in the class are implemented by calling another
        /// method of the class, such that derived classes can implement either of the typed methods).</summary>
        private int numCalls = 0;

        /// <inheritdoc/>
        public virtual object Convert(object source)
        {
            if (++numCalls > 2)
            {
                throw new InvalidOperationException($"{GetType().Name}.{nameof(Convert)}: Infinite recursion: this class is not implemented correctly.");
            }
            return ConvertTyped((TSource)source);
        }

        /// <inheritdoc/>
        public virtual bool TryConvert(object source, out object target)
        {
            if (++numCalls > 2)
            {
                throw new InvalidOperationException($"{GetType().Name}.{nameof(Convert)}: Infinite recursion: this class is not implemented correctly.");
            }
            if (source is TSource typedSource)
            {
                TTarget typedTarget;
                bool success = TryConvertTyped(typedSource, out typedTarget);
                if (success)
                {
                    target = typedTarget;
                    return true;
                }
            }
            target = default;
            return false;
        }

        /// <inheritdoc/>
        public virtual TTarget ConvertTyped(TSource source)
        {
            if (++numCalls > 2)
            {
                throw new InvalidOperationException($"{GetType().Name}.{nameof(Convert)}: Infinite recursion: this class is not implemented correctly.");
            }
            TTarget target;
            bool success = TryConvertTyped(source, out target);
            if (success)
            {
                return target;
            }
            if (source == null)
            {
                throw new ArgumentNullException("source", $"{this.GetType().Name}.{nameof(ConvertTyped)}: Failed to convert null object of type {SourceType.Name} to type {TargetType.Name}.");
            }
            throw new InvalidOperationException($"{this.GetType().Name}.{nameof(ConvertTyped)}: Failed to convert object of type {SourceType.Name} to type {TargetType.Name}.");
        }

        /// <inheritdoc/>
        public virtual bool TryConvertTyped(TSource source, out TTarget target)
        {
            if (++numCalls > 2)
            {
                throw new InvalidOperationException($"{GetType().Name}.{nameof(Convert)}: Infinite recursion: this class is not implemented correctly.");
            }
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
