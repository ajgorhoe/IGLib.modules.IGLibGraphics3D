using System;

namespace IGLib.Core
{


    public interface ICapturedVar<DeclaredType> : ICapturedVar
    {

        DeclaredType Value { get; init; }

    }


    public interface ICapturedVar
    {

        object ValueObject { get; }

        Type Type { get; }

        string TypeName { get; }

        string TypeFullName { get; }

        string TypeString { get; }


        Type ValueType { get; }

        string ValueTypeName { get; }

        string ValueTypeFullName { get; }

        string ValueTypeString { get; }


        string ToString();

        string ToStringLong(string indentation = "  ");

    }


}
