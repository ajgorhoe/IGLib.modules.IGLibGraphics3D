using System;

namespace IGLib.Core
{
    public interface ISingleTypeConverterRegistry
    {

        void Register(Type sourceType, Type targetType, ISingleTypeConverter converter);

        ISingleTypeConverter this[(Type SourceType, Type TargetType) key] { get; }

        public ISingleTypeConverter HasTypeConverter(Type sourceType, Type targetType);

    }
}
