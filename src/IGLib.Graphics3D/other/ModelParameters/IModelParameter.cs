using System;

namespace IGLib.Core
{

    internal interface IModelParameter
    {
        object DefaultValueObject { get; }
        string DesCription { get; }
        bool IsValueDefined { get; }
        string Name { get; }
        string Title { get; }
        Type Type { get; }
        object ValueObject { get; }
    }
}