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
    class ModelParameter<ParameterType> : ModelParameter, IModelParameter<ParameterType>
    {

        /// <summary>Comprehensive constructor, initializes all fields of the type.</summary>
        /// <param name="name">Name of the current model parameter, as is used in models
        /// (defines the <see cref="Name"/>) property.</param>
        /// <param name="type">Type of the current parameter (defines the <see cref="Type"/> property)</param>
        /// <param name="title">Title for parameter description, defines the <see cref="Title"/> property.</param>
        /// <param name="description">Parameter description, defines the <see cref="Description"/> property.</param>
        /// <param name="defaultValue">Default value of the parameter, defines the <see cref="DefaultValue"/> property.</param>
        /// <param name="value">Value of the parameter, defines the <see cref="Value"/> property.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ModelParameter(string name, string title, string description, ParameterType defaultValue, ParameterType value, 
            bool isDefaultWhenValueNotDefined = ModelParameter.DefaultIsDefaultWhenValueNotDefined) : 
            base(name, typeof(ParameterType), title, description, defaultValue, value, isDefaultWhenValueNotDefined)
        {
            // also set typed values:
            DefaultValue = defaultValue;
            Value = value;
            IsValueDefined = true;
        }

        /// <summary>Constructor, with value undefined (sets <see cref="IsValueDefined"/> to false and
        /// <see cref="ValueObject"/> to null). Meaning of parameters is the same as with
        /// <see cref="ModelParameter{ParameterType}.ModelParameter(string, string, string, ParameterType, ParameterType)"/>,
        /// except there is no parameter dor <see cref="Value"/>.</summary>
        public ModelParameter(string name, string title, string description, ParameterType defaultValue) : 
            this(name, title, description, defaultValue, default)
        {
            // also set typed values:
            DefaultValue = defaultValue;
            IsValueDefined = false;
            ValueObject = null;
        }


        /// <summary>Minimal (terse) constructor.</summary>
        /// <param name="name">Name of the current model parameter, as is used in models
        /// (defines the <see cref="Name"/>) property.</param>
        /// <param name="type">Type of the current parameter (defines the <see cref="Type"/> property)</param>
        public ModelParameter(string name) : 
            this(name, $"Parameter {name}",  $"Represents model parameter {name} of type ${typeof(ParameterType).Name}", 
                default)
        { }


        private ParameterType _defaultValue = default;

        /// <inheritdoc/>
        public virtual ParameterType DefaultValue 
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
                _defaultValue = value;
                IsDefaultValueDefined = true;
            }
        }

        ParameterType _value = default;

        /// <inheritdoc/>
        public virtual ParameterType Value 
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
                _value = value;
                    IsValueDefined = true;
            }
        }



        /// <inheritdoc/>
        public override object DefaultValueObject
        {
            get
            {
                return _value;
            }
            protected set
            {
                if (value == null)
                {
                    DefaultValue = default;
                    IsDefaultValueDefined = false;
                    return;
                }
                DefaultValue = (ParameterType)value;
            }
        }

        private object _valueObject = null;

        /// <inheritdoc/>
        public override object ValueObject
        {
            get
            {
                return Value;
            }
            set
            {
                if (value == null)
                {
                    Value = default;
                    IsValueDefined= false;
                    return;
                }
                Value = (ParameterType)value;
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
        public virtual IModelParameter<ParameterType> UpdateValue(ParameterType newValue)
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
        public virtual IModelParameter<ParameterType> UpdateDefaultValue(ParameterType newDefaultValue)
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
        internal static new ModelParameter<double> CreateExampleParameter12()
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
