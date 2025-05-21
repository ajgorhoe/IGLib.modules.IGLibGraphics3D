using System;
using System.Text;
using IGLib.Core.CollectionExtensions;

namespace IGLib.Core
{

    /// <summary>This class just hosts static methods that work with <see cref="CapturedVar{DeclaredType}"/>:
    /// <para>* <see cref="Capture{CapturedVariableType}(CapturedVariableType)"/>, which captures a 
    /// variable, property, field, or value or expression.</para>
    /// <para>* <see cref="VarType{TypeVar}(TypeVar)"/>, which returns the DECLARED type of an entity (a
    /// variable, property, field, or value or expression of a known type.)</para></summary>
    public static class CapturedVar
    {

        /// <summary>Creates a <see cref="CapturedVar{CapturedVariableType}"/> object that captures the
        /// specified variable, property, field, or value.
        /// <para>The created object stores the current value 
        /// of the captured entity (a reference for reference types, value for value types), its declared
        /// type (either the declared type, which is inferred at compile type, or a compatible caller-provided
        /// declared type).</para>
        /// <para>In most use cases, the caller will not provide the type parameter (<typeparamref name="CapturedVariableType"/>),
        /// such that it is inferred by the compiler. In this way, the type parameter becomes the declared type 
        /// of the captured entity (because the compiiler is aware of it at compile time), therefore <see cref="Capture"/></para></summary>
        /// <typeparam name="CapturedVariableType">Specifies the declared type of the captured variable, 
        /// property, field, or value. In most use cases, it is not stated explicitly, but inferred by the compiler.</typeparam>
        /// <param name="capturedEntity">The entity whose value and actual type at the time of capture (call to this meethod), plus
        /// declared type, are captured by the returned object.</param>
        /// <returns>Object that captures contains the current value and actual type, plus declared type, of the
        /// entity captured (<paramref name="capturedEntity"/>).</returns>
        public static CapturedVar<CapturedVariableType>
            Capture<CapturedVariableType>(CapturedVariableType capturedEntity)
        {
            return new CapturedVar<CapturedVariableType>(capturedEntity);
        }

        /// <summary>Returns the DECLARED type of the specified entity <paramref name="entity"/> (a variable, property, 
        /// field, or value or expression with a known type).
        /// <para>In order to correctly obtain the declared type of <see cref="var"/>, the caller MUST NOT
        /// specify the type parameter <typeparamref name="TypeVar"/>, such that the type is inferred
        /// and provided by the compiler (otherwise, the provided type is enforced and returned).</para>
        /// <para>The <see cref="CapturedVar{DeclaredType}"/> is used in order to obtain the declared type.</para></summary>
        /// <typeparam name="TypeVar">Type of the variable. It is typically not provided by the caller, but instead
        /// left unspecified, to be inferred and provided by the compiler.</typeparam>
        /// <param name="entity">The entity whose DECLARED type is returned (a variable, property, field, or
        /// a literal value or an expression with a known type).
        /// <para>The generic parameter should normally not be specified and should be inferred by the compiiler. In this
        /// way, the returned type is actually the declared type of the provided entity (<paramref name="entity"/>).</para>
        /// <para>If null is provided as parameter and the generic type parameter is not provided, the parameter must be 
        /// explicitly  cast to a type (which becomes the declared type), otherwise compiler error is generated because 
        /// the compiler cannot infer the type of the bare null entity.</para></param>
        /// <returns>The DECLARED type of the specified entity (<paramref name="entity"/>). The returned type can be 
        /// enforced by stating the generic parameter <typeparamref name="TypeVar"/>, but in most use cases the
        /// generic type is not specified, such that it is inferred by the compiler and the inferred type is
        /// returned (which in this case will actually be the declared type of the provided entity when it is a
        /// variable, field, or property, or inferred or explicitly cast type when it is a value or expression).</returns>
        public static Type VarType<TypeVar>(TypeVar entity)
        {
            // Remark: we don't need to store the value because we just want to use the compiler to infer the
            // declared type of the entity.
            return (new CapturedVar<TypeVar>(entity, doStoreValue: false)).Type;
        }


    }

    /// <summary>An <see cref="ICapturedVar{DeclaredType}"/> where declared type of the captured entity
    /// (variable, property, field, value) is <see cref="object"/>.
    /// <para>This class is also used for referencing fields of <see cref="ICapturedVar{DeclaredType}"/>,
    /// such that it is not necessary to state the generic type.</para></summary>
    public class CapturedVarObject : CapturedVar<object>
    {
        public CapturedVarObject(object variable): base(variable) { }
    }


    /// <summary>Captures the declared type of the specified entity, its value at the time of capture, and 
    /// its actual type. The declared type is the generic type paramer  (<typeparamref name="DeclaredType"/>)
    /// of the class.
    /// <para>In practice, the generic type usually not specified by the user, but is inferred by the compiler.
    /// For this reason, this class is almost never instantiated by its constructor, but by calling the static
    /// factory method <see cref="CapturedVar.Capture{CapturedVariableType}(CapturedVariableType)"/>.
    /// Beside this meethod, the class is also used by the static method <see cref="CapturedVar.VarType{TypeVar}(TypeVar)"/>,
    /// which returns the declared type of its argument (a variable, property, field, or valur or expression
    /// whose type can be inferred).</para></summary>
    /// <typeparam name="DeclaredType">The declared type of the captured entity. In most practical scenarios,
    /// the type is not specified explicitly but is left to be inferred by the compiler. This is usually achieved
    /// by instantiating the object via the static method <see cref="CapturedVar.Capture{CapturedVariableType}(CapturedVariableType)"/>.</typeparam>
    public class CapturedVar<DeclaredType> : ICapturedVar<DeclaredType>, ICapturedVar
    {

        public CapturedVar(DeclaredType variable): this(variable, doStoreValue: true) 
        {  }

        /// <summary>Constructor; enables the instance of this class not to store the value of the provided
        /// entity by setting <paramref name="doStoreValue"/> to false.</summary>
        /// <param name="variable">The entity to be captered by the created instance.</param>
        /// <param name="doStoreValue">Whether or not to store the value of the captured entity. False is 
        /// used only exceptionally, e.g. when an instance of this class is used only to infer the declared 
        /// type of the provided entity (<paramref name="variable"/>), such as in 
        /// <see cref="CapturedVar.VarType{TypeVar}(TypeVar)"/>.</param>
        internal CapturedVar(DeclaredType variable, bool doStoreValue)
        {
            if (doStoreValue)
            {
                Value = variable;
            }
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
            return $"Captured entity: declared type = {TypeString}, actual type = {ValueTypeString}, "
                + $"\n    value = {Value.ToReadableString()}";
        }

        public virtual string ToStringLong(string indentation = "  ")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(indentation + $"Declared type of captured entity ({nameof(Type)}):\n{indentation + indentation}{Type?.ToString()}");
            sb.AppendLine(indentation + $"{nameof(TypeName)}: {TypeName}");
            sb.AppendLine(indentation + $"{nameof(TypeFullName)}: {TypeFullName}");
            sb.AppendLine(indentation + $"{nameof(TypeString)}: {TypeString}");
            
            
            sb.AppendLine(indentation + $"Actual type of the captured value ({nameof(ValueType)}):\n{indentation + indentation}{ValueType?.ToString()}");
            sb.AppendLine(indentation + $"{nameof(ValueTypeName)}: {ValueTypeName}");
            sb.AppendLine(indentation + $"{nameof(ValueTypeFullName)}: {ValueTypeFullName}");
            sb.AppendLine(indentation + $"{nameof(ValueTypeString)}: {ValueTypeString}");
            sb.AppendLine(indentation + $"{nameof(Value)}:");
            sb.AppendLine(indentation + indentation + $"{Value.ToReadableString()}:");
            return sb.ToString();
        }

    }


}
