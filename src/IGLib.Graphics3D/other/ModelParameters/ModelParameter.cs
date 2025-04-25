using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IGLib.Core
{


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
        public ModelParameter(string name, Type type, string title, string description = null,
            object defaultValue = null, object value = null, 
            bool isDefaultWhenValueNotDefined = ModelParameter.DefaultIsDefaultWhenValueNotDefined)
        {
            if (string.IsNullOrEmpty(name))
            {
                if (Name == null)
                {
                    throw new ArgumentNullException(nameof(name), "Parameter name cannot be null.");
                }
                throw new ArgumentException("Parameter name cannot be an empty string.", nameof(name));
            }
            if (Type == null)
            {
            }
            Name = name;
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
            ValueObject = value;
            IsDefaultWhenValueNotDefined = isDefaultWhenValueNotDefined;
        }

        /// <summary>Minimal (terse) constructor.</summary>
        /// <param name="name">Name of the current model parameter, as is used in models
        /// (defines the <see cref="Name"/>) property.</param>
        /// <param name="type">Type of the current parameter (defines the <see cref="Type"/> property)</param>
        public ModelParameter(string name, Type type) : this(name, type, 
            $"Parameter {name}", $"Represents model parameter {name} of type ${type.Name}")
        {  }

        /// <summary>A terse constructor that also defines the default value.</summary>
        /// <param name="name">Name of the current model parameter, as is used in models
        /// (defines the <see cref="Name"/>) property.</param>
        /// <param name="type">Type of the current parameter (defines the <see cref="Type"/> property)</param>
        /// <param name="defaultValue">Defalult value of the parameter.</param>
        public ModelParameter(string name, Type type, object defaultValue) : this(name, type, 
            $"Parameter {name}", $"Represents model parameter {name} of type ${type.Name}")
        {
            DefaultValueObject = defaultValue;
        }

        /// <summary>A constant that provedes default value of <see cref="IModelParameter.IsDefaultWhenValueNotDefined"/>,
        /// such that default initialization can be unified across implementations of <see cref="IModelParameter"/>.</summary>
        public const bool DefaultIsDefaultWhenValueNotDefined = true;

        /// <inheritdoc/>
        public string Name { get; init; }

        /// <inheritdoc/>
        public virtual Type Type { get; init; }

        /// <inheritdoc/>
        public string Title { get; protected set; }

        /// <inheritdoc/>
        public string Description { get; protected set; }
        private object _defaultValueObject = null;

        /// <inheritdoc/>
        public virtual object DefaultValueObject 
        {
            get
            {
                if (IsDefaultValueDefined)
                {
                    return _defaultValueObject;
                }
                throw new InvalidOperationException($"Default value of parameter {Name} is not defined.");
            }
            protected set
            {
                _defaultValueObject = value;
                if (value != null)
                {
                    IsValueDefined = true;
                }
                else
                {
                    IsValueDefined = false;
                }
            }
        }

        /// <inheritdoc/>
        public virtual bool IsDefaultValueDefined { get; protected set; } = false;

        private object _valueObject = null;

        /// <inheritdoc/>
        public virtual object ValueObject 
        {
            get
            {
                if (IsValueDefined)
                {
                    return _valueObject;
                }
                if (IsDefaultWhenValueNotDefined && IsDefaultValueDefined)
                {
                    // When value is not defined, we can replace it by the default value:
                    return DefaultValueObject;
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
                _valueObject = value;
                if (value != null)
                {
                    IsValueDefined = true;
                }
                else
                {
                    IsValueDefined = false;
                }
            }
        }

        /// <inheritdoc/>
        public virtual bool IsDefaultWhenValueNotDefined { get; protected set; } 
            = ModelParameter.DefaultIsDefaultWhenValueNotDefined;

        /// <inheritdoc/>
        public virtual bool IsValueDefined { get; protected set; } = false;


        /// <inheritdoc/>
        public virtual IModelParameter ClearValue()
        {
            ValueObject = default;
            IsValueDefined = false;
            return this;
        }

        /// <inheritdoc/>
        public virtual IModelParameter UpdateValue(object newValue)
        {
            ValueObject = newValue;
            return this;
        }

        /// <inheritdoc/>
        public virtual IModelParameter ClearDefaultValue()
        {
            DefaultValueObject = default;
            IsDefaultValueDefined = false;
            return this;
        }

        /// <inheritdoc/>
        public virtual IModelParameter UpdateDefaultValue(object newDefaultValue)
        {
            DefaultValueObject = newDefaultValue;
            return this;
        }

        /// <inheritdoc/>
        public virtual IModelParameter UpdateTitle(string newTitle)
        {
            Title = newTitle;
            return this;
        }

        /// <inheritdoc/>
        public virtual IModelParameter UpdateDescription(string newDescription)
        {
            Description = newDescription;
            return this;
        }



        /// <inheritdoc/>
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
