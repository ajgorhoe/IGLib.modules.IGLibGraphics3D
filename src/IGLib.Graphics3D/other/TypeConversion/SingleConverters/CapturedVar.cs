using IGLib.Core.CollectionExtensions;
using System;
using System.Text;

namespace IGLib.Core
{

    public class CapturedVarS
    {

        public static CapturedVar<CapturedVariableType>
            CaptureVar<CapturedVariableType>(CapturedVariableType var)
        {
            return new CapturedVar<CapturedVariableType>(var);
        }

        public static Type VarType<TypeVar>(TypeVar var)
        {
            return CaptureVar(var)?.Type;
        }

    }

    public class CapturedVar<DeclaredType> : ICapturedVar<DeclaredType>, ICapturedVar
    {

        public CapturedVar(DeclaredType variable)
        {
            Value = variable;
        }

        public DeclaredType Value { get; init; }

        public object ValueObject => Value;

        public Type Type => typeof(DeclaredType);

        public string TypeName => Type.Name;

        public string TypeFullName => Type.FullName;

        public string TypeString => Type.ToString();

        public Type ValueType => Value?.GetType()??null;

        public string ValueTypeName => ValueType?.Name;

        public string ValueTypeFullName => ValueType?.FullName;

        public string ValueTypeString => ValueType?.ToString();


        public override string ToString()
        {
            return $"Variable: type = {TypeString}, actual type = {ValueTypeString}, "
                + $"\n    value = {CollectionExtensions.CollectionExtensions.ToReadableString(Value)}";
        }

        public virtual string ToStringLong(string indentation = "  ")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(indentation + $"{nameof(Type)}: {Type?.ToString()}");
            sb.AppendLine(indentation + $"{nameof(TypeName)}: {TypeName}");
            sb.AppendLine(indentation + $"{nameof(TypeFullName)}: {TypeFullName}");
            sb.AppendLine(indentation + $"{nameof(TypeString)}: {TypeString}");
            sb.AppendLine(indentation + $"{nameof(ValueTypeName)}: {ValueTypeName}");
            sb.AppendLine(indentation + $"{nameof(ValueTypeFullName)}: {ValueTypeFullName}");
            sb.AppendLine(indentation + $"{nameof(ValueTypeString)}: {ValueTypeString}");
            sb.AppendLine(indentation + $"{nameof(Value)}:");
            sb.AppendLine(indentation + indentation + $"{Value.ToReadableString()}:");
            return sb.ToString();
        }

    }


}
