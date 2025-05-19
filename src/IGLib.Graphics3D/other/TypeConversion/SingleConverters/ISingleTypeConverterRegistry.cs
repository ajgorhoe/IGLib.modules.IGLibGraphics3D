using System;

namespace IGLib.Core
{
    public interface ISingleTypeConverterRegistry
    {

        void Register(Type sourceType, Type targetType, ISingleTypeConverter converter);

        ISingleTypeConverter this[(Type sourceType, Type targetType) key] { get; }

        ISingleTypeConverter HasConverter(Type sourceType, Type targetType);

        ISingleTypeConverter<SourceType, TargetType> GetGenericConverter<SourceType, TargetType>();

        bool HasGenericConverter<SourceType, TargetType>();

        object Convert(object source, Type sourceType, Type targetType);

        bool TryConvert(object source, out object target, Type sourceType, Type targetType);

    }
}
