
#nullable disable

using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// namespace IGLib.Core.Experimental
namespace YourNamespace
{


    /// <inheritdoc/>
    public class ModelParameter : IModelParameter
    {

        /// <inheritdoc/>
        public string Name { get; init; }

        /// <inheritdoc/>
        public virtual Type Type { get; init; }

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
                //if (IsConstant && IsDefaultValueDefined)
                //{
                //    throw new InvalidOperationException($"Cannot redefine default value of parameter {Name} because it is constant.");
                //}
                _defaultValueObject = value;
                if (value != null)
                {
                    IsDefaultValueDefined = true;
                }
                else
                {
                    IsDefaultValueDefined = false;
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
        public virtual bool IsValueDefined { get; protected set; } = false;


    }

}
