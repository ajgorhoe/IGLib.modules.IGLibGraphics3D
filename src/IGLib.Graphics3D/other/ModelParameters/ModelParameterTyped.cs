
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using static IGLib.Core.ModelParameter;

namespace IGLib.Core
{

#if false
    public class TestStatic111
    {

        public bool Test()
        {
            return DefaultIsDefaultWhenValueNotDefined;
        }
    }
#endif


    /// <inheritdoc/>
    public class ModelParameter<ValueType> : ModelParameter, IModelParameter<ValueType>
    {

        /// <summary>Comprehensive constructor, initializes all fields of the parameter object. It calls base constructor
        /// <see cref="ModelParameter.ModelParameter(string, Type, string, string, object, object, bool, bool)"/> to
        /// initialize most properties. The type propety is inferred from type parameter (<typeparamref name="ValueType"/>).</summary>
        /// <param name="name">Name of the current model parameter, as is used in models
        /// (defines the <see cref="Name"/>) property.</param>
        /// <param name="title">Title for parameter description, defines the <see cref="Title"/> property.</param>
        /// <param name="description">Parameter description, defines the <see cref="Description"/> property.</param>
        /// <param name="defaultValue">Default value of the parameter, defines the <see cref="DefaultValue"/> property.</param>
        /// <param name="value">Value of the parameter, defines the <see cref="Value"/> property.</param>
        /// <param name="isConstant">Whether the constructed parameter object represents a constant parameter. Optional, 
        /// default is <see cref="DefaultIsConstant"/>.</param>
        /// <param name="isDefaultWhenValueNotDefined">Specifies whether default value can be returned as value when
        /// the value is not defined but the default value is. Optional, default is <see cref="DefaultIsDefaultWhenValueNotDefined"/>.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ModelParameter(string name, string title, string description, ValueType defaultValue, ValueType value,
            bool isConstant = DefaultIsConstant,
            bool isDefaultWhenValueNotDefined = DefaultIsDefaultWhenValueNotDefined) : 
            base(name, typeof(ValueType), title, description, defaultValue, value, 
                isConstant, isDefaultWhenValueNotDefined)
        {
            // Set typed values (setting other values is deferred to base constructor):
            DefaultValue = defaultValue;
            IsDefaultValueDefined = true;
            Value = value;
            IsValueDefined = true;
        }

        /// <summary>Minimal (very terse) constructor.
        /// <paramref name="name"/>Meaning and behavior of parameters that are defined for this constructor is the same as for
        /// <see cref="ModelParameter{ValueType}.ModelParameter(string name, string, string, ValueType, ValueType,
        /// bool, bool)"/>.</summary>
        public ModelParameter(string name,
            bool isConstant = DefaultIsConstant,
            bool isDefaultWhenValueNotDefined = DefaultIsDefaultWhenValueNotDefined) : 
            this(name, null, null, (ValueType)default, (ValueType)default)
        {
            DefaultValueObject = null;
            ValueObject = null;
            IsDefaultValueDefined = false;
            IsValueDefined = false;
        }

        /// <summary>Constructor, with value undefined (sets <see cref="IsValueDefined"/> to false and <see cref="ValueObject"/> to null)
        /// but with default value specified.
        /// <para>Meaning of parameters is the same as with
        /// <see cref="ModelParameter{ParameterType}.ModelParameter(string, string, string, ParameterType, ParameterType, bool, bool)"/>,
        /// except that some are not defined.</para></summary>
        public ModelParameter(string name, ValueType defaultValue,
            bool isConstant = DefaultIsConstant,
            bool isDefaultWhenValueNotDefined = DefaultIsDefaultWhenValueNotDefined) : 
            this(name, null, null, defaultValue, (ValueType)default,
                isConstant, isDefaultWhenValueNotDefined)
        {
            // also set typed values:
            DefaultValue = defaultValue;
            IsDefaultValueDefined = true;
            ValueObject = null;
            IsValueDefined = false;
        }



        private ValueType _defaultValue = default;

        /// <inheritdoc/>
        public virtual ValueType DefaultValue 
        { 
            get
            {
                if (IsDefaultValueDefined)
                {
                    return _defaultValue;
                }
                throw new InvalidOperationException($"Default value of parameter {Name} is not defined.");
            }
            protected set
            {
                if (IsConstant && IsDefaultValueDefined)
                {
                    throw new InvalidOperationException($"Cannot redefine default value of parameter {Name} because it is constant.");
                }
                _defaultValue = value;
                IsDefaultValueDefined = true;
            }
        }

        ValueType _value = default;

        /// <inheritdoc/>
        public virtual ValueType Value 
        {
            get
            {
                if (IsValueDefined)
                {
                    return _value;
                }
                if (IsDefaultWhenValueNotDefined && IsDefaultValueDefined)
                {
                    // When value is not defined, we can replace it by the default value:
                    return DefaultValue;
                }
                // Could not get the value, therefore throw:
                if (IsDefaultWhenValueNotDefined)
                {
                    throw new InvalidOperationException($"Value of parameter {Name} is not defined, and default value is also not defined.");
                }
                throw new InvalidOperationException($"Value of parameter {Name} is not defined and it is not replaceable by its default value.");
            }
            set
            {
                if (IsConstant && IsDefaultValueDefined)
                {
                    throw new InvalidOperationException($"Cannot redefine value of parameter {Name} because it is constant.");
                }
                _value = value;
                    IsValueDefined = true;
            }
        }



        /// <inheritdoc/>
        /// <remarks>Implementation from the base class <see cref="ModelParameter"/> is overriden here because
        /// this property is bound to the new strongly typed property <see cref="DefaultValue"/>.</remarks>
        public override object DefaultValueObject
        {
            get
            {
                return _value;
            }
            protected set
            {
                if (IsConstant && IsDefaultValueDefined)
                {
                    throw new InvalidOperationException($"Cannot redefine default value of parameter {Name} because it is constant.");
                }
                if (value == null)
                {
                    DefaultValue = default;
                    IsDefaultValueDefined = false;
                    return;
                }
                DefaultValue = (ValueType)value;
            }
        }

        /// <inheritdoc/>
        /// <remarks>Implementation from the base class <see cref="ModelParameter"/> is overriden here because
        /// this property is bound to the new strongly typed property <see cref="Value"/>.</remarks>
        public override object ValueObject
        {
            get
            {
                return Value;
            }
            set
            {
                if (IsConstant && IsValueDefined)
                {
                    throw new InvalidOperationException($"Can not redefine value of parameter {Name} because it is constant.");
                }
                if (value == null)
                {
                    Value = default;
                    IsValueDefined= false;
                    return;
                }
                Value = (ValueType)value;
            }
        }


        /// <inheritdoc/>
        public override IModelParameter ClearValue()
        {
            Value = default;
            IsValueDefined = false;
            return this;
        }

        /// <inheritdoc/>
        public virtual IModelParameter<ValueType> UpdateValue(ValueType newValue)
        {
            Value = newValue;
            return this;
        }

        /// <inheritdoc/>
        public override IModelParameter ClearDefaultValue()
        {
            DefaultValue = default;
            IsDefaultValueDefined = false;
            return this;
        }

        /// <inheritdoc/>
        public virtual IModelParameter<ValueType> UpdateDefaultValue(ValueType newDefaultValue)
        {
            DefaultValue = newDefaultValue;
            return this;
        }



        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.AppendLine($"Typed default value: {DefaultValue}");
            sb.AppendLine($"Typed value: {Value}");
            return sb.ToString();
        }

        /// <summary>Creates a sample <see cref="ModelParameter{Double}"/> object, prints its content to console,
        /// and returns it.</summary>
        internal static ModelParameter<double> CreateExampleParameter12()
        {
            ModelParameter<double> param = new ModelParameter<double>("Param1")
            {
                Title = "Parameter Φ1, phase shift in the first direction.",
                Description = "This parameter of type double specifies the phase shift of the 3D Lissajous curve in the direction x.",
                DefaultValue = 0,
                Value = 22.44,
                IsValueDefined = true
            };
            Console.WriteLine($"Created {param.GetType()} ojbject:\n{param.ToString()}");
            return param;
        }

    }

}
