using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace IGLib.Core
{

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
        public ModelParameter(string name, string title, string description,
            ParameterType defaultValue, ParameterType value) : base(name, typeof(ParameterType),
                title, description, defaultValue, value)
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


        /// <inheritdoc/>
        public ParameterType DefaultValue { get; protected set; }

        /// <inheritdoc/>
        public ParameterType Value { get; set; }

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
        internal static new ModelParameter<double> CreateExampleParameter1()
        {
            ModelParameter<double> param = new ModelParameter<double>("Param1")
            {
                Title = "Parameter Φ1, phase shift in the first direction.",
                Description = "This parameter of type double specifies the phase shift of the 3D Lissajous curve in the direction x.",
                DefaultValueObject = (double)0,
                Value = 22.44,
                IsValueDefined = true
            };
            Console.WriteLine($"Created {param.GetType()} ojbject:\n{param.ToString()}");
            return param;
        }

    }

    /// <inheritdoc/>
    class ModelParameter : IModelParameter
    {

        /// <summary>Comprehensive constructor, initializes all fields of the type.</summary>
        /// <param name="name">Name of the current model parameter, as is used in models
        /// (defines the <see cref="Name"/>) property.</param>
        /// <param name="type">Type of the current parameter (defines the <see cref="Type"/> property)</param>
        /// <param name="title">Title for parameter description, defines the <see cref="Title"/> property.</param>
        /// <param name="description">Parameter description, defines the <see cref="Description"/> property.</param>
        /// <param name="defaultValue">Default value of the parameter, defines the <see cref="DefaultValueObject"/> property.</param>
        /// <param name="value">Value of the parameter, defines the <see cref="ValueObject"/> property.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ModelParameter(string name, Type type, string title, string description,
            object defaultValue, object value)
        {
            Name = name;
            if (Name == null)
            {
                throw new ArgumentNullException(nameof(name), "Parameter name cannot be null.");
            }
            Type = type;
            Title = title;
            if (Title == null)
            {
                Title = name;
            }
            Description = description;
            if (Description == null)
            {
                Description = $"Model parameter {Name}.";
            }
            DefaultValueObject = defaultValue;
            if (DefaultValueObject == null)
                DefaultValueObject = default;
            ValueObject = value;
            IsValueDefined = value != null;
        }

        /// <summary>Minimal (terse) constructor.</summary>
        /// <param name="name">Name of the current model parameter, as is used in models
        /// (defines the <see cref="Name"/>) property.</param>
        /// <param name="type">Type of the current parameter (defines the <see cref="Type"/> property)</param>
        public ModelParameter(string name, Type type) : this(name, type, 
            $"Parameter {name}", $"Represents model parameter {name} of type ${type.Name}", null, null)
        {  }

        /// <inheritdoc/>
        public string Name { get; protected set; }

        /// <inheritdoc/>
        public string Title { get; protected set; }

        /// <inheritdoc/>
        public string Description { get; protected set; }

        /// <inheritdoc/>
        public virtual Type Type { get; protected set; }

        /// <inheritdoc/>
        public virtual object DefaultValueObject { get; protected set; }

        /// <inheritdoc/>
        public virtual object ValueObject { get; protected set; }

        /// <inheritdoc/>
        public virtual bool IsValueDefined { get; protected set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Model parameterr ${Name} (${Type.Name}):");
            sb.AppendLine($"  Title: \"{Title}\"");
            sb.AppendLine($"  Description: \"{Description}\"");
            sb.AppendLine($"  Default value: {(DefaultValueObject == null ? "null" : DefaultValueObject.ToString())}");
            // sb.AppendLine($"  Value: {(IsValueDefined ? (ValueObject == null ? "null" : ValueObject.ToString()) : "<Not defined.>")}");
            sb.AppendLine($"  Value: {(ValueObject == null ? "null" : ValueObject.ToString())}");
            sb.AppendLine($"  Is value defined: {IsValueDefined}");
            return sb.ToString();
        }

        /// <summary>Creates a sample <see cref="ModelParameter"/> object, prints its content to console,
        /// and returns it.</summary>
        internal static ModelParameter CreateExampleParameter1()
        {
            ModelParameter param = new ModelParameter("Param1", typeof(double))
            {
                Title = "Parameter Φ1, phase shift in the first direction.",
                Description = "This parameter of type double specifies the phase shift of the 3D Lissajous curve in the direction x.",
                DefaultValueObject = (double)0,
                IsValueDefined = false
            };
            Console.WriteLine($"Created {param.GetType()} ojbject:\n{param.ToString()}");
            return param;
        }

    }

}
