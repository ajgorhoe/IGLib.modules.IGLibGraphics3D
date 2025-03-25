using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace IGLib.Core
{


    class ModelParameter<ParameterType>: ModelParameter
    {
        public ModelParameter(string name, string title, string description,
            ParameterType defaultValue, ParameterType value) : base(name, typeof(ParameterType),
                title, description, defaultValue, value)
        {
            // also set typed values:
            DefaultValue = defaultValue;
            Value = value;
        }

        public ParameterType DefaultValue { get; protected set; }

        public ParameterType Value { get; set; }

    }

    /// <summary>Represents a single parameter of a parametric model.
    /// <para>Contains information about a specific model parameter, such as 
    /// parameter name, title, descriptin, type, default value, and current 
    /// value.</para></summary>
    class ModelParameter : IModelParameter
    {

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
            DesCription = description;
            if (DesCription == null)
            {
                DesCription = $"Model parameter {Name}.";
            }
            DefaultValueObject = defaultValue;
            if (DefaultValueObject == null)
                DefaultValueObject = default;
            ValueObject = value;
            IsValueDefined = value != null;
        }

        public string Name { get; protected set; }

        public string Title { get; protected set; }

        public string DesCription { get; protected set; }

        public virtual Type Type { get; protected set; }

        public virtual object DefaultValueObject { get; protected set; }

        public virtual object ValueObject { get; protected set; }

        public virtual bool IsValueDefined { get; protected set; }


    }

}
