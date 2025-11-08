
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IGLib.Core
{

    /// <summary>Similar as <see cref="ModelParameterSetFixedBase{ModelParameterType}"/>, except that 
    /// parameter objects can be added, replaced or removed after the parameter set is created.</summary>
    /// <typeparam name="ModelParameterType">Type of parameter objects contained in the set, must implement
    /// the <see cref="IModelParameter"/> interface. Commonly, the <see cref="ModelParameter"/> and the
    /// generic <see cref="ModelParameter{ParameterType}"/> are used.</typeparam>
    public abstract class ModelParameterSetBase<ModelParameterType> : ModelParameterSetFixedBase<ModelParameterType>,
        IModelParameterSetBase<ModelParameterType>
        where ModelParameterType : IModelParameter
    {

        /// <summary>Constructor, initializes the current model parameter set, optionally providing parameter
        /// objects that are added to the set.</summary>
        /// <param name="canAddParameters">Whether parameters can be added after initialization, defines the <see cref="CanAddParameters"/> property.</param>
        /// <param name="title">Title of the parameter set, defines the <see cref="Title"/> property.</param>
        /// <param name="description">Description of the parameter set, defines the <see cref="Description"/> property.</param>
        /// <param name="parameters">Parameter objects to initalize the parameter set. Name of eacch paremeter object
        /// (<see cref="ModelParameterType.Name"/>) is used as access key for that parameter.</param>
        public ModelParameterSetBase(string title, string description, params ModelParameterType[] parameters):
            base(title, description, parameters)
        {
            CanAddParameters = true;
        }

        /// <summary>Constructor, initializes the current model parameter set, optionally using tuples of names and 
        /// parameter objects that are added to the set.</summary>
        /// <param name="title">Title of the parameter set, defines the <see cref="Title"/> property.</param>
        /// <param name="description">Description of the parameter set, defines the <see cref="Description"/> property.</param>
        /// <param name="keysAndParameters">Tuples of names (used as keys keys) and the corresponding parameter objects 
        /// to initalize the parameter set.</param>
        public ModelParameterSetBase(string title, string description,
            params (string Name, ModelParameterType Parameter)[] keysAndParameters):
            base(title, description, keysAndParameters)
        {
            CanAddParameters = true;
        }

        /// <inheritdoc/>
        public new ModelParameterType this[string parameterName]
        {
            get
            {
                return ParametersDictionaryInternal[parameterName];
            }
            set
            {
                ParametersDictionaryInternal[parameterName] = value;
            }
        }

        /// <inheritdoc/>
        public new ModelParameterType this[int whichParameter]
        {
            get
            {
                return ParametersDictionaryInternal[ParameterNamesInternal[whichParameter]];
            }
            set
            {
                ParametersDictionaryInternal[ParameterNamesInternal[whichParameter]] = value;
            }
        }


        /// <inheritdoc/>
        public virtual void Add(params (string Key, ModelParameterType Parameter)[] keysAndParameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual void AddOrReplace(params (string Key, ModelParameterType Parameter)[] keysAndParameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual void Add(params ModelParameterType[] parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual void AddOrReplace(params ModelParameterType[] parameters)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc/>
        public virtual void Remove(params string[] keys)
        {
            throw new NotImplementedException();
        }

    }

}
