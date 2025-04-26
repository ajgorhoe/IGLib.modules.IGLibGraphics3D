using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IGLib.Core
{

    /// <summary>Similar as <see cref="ModelParameterSetBaseFixed{ModelParameterType}"/>, except that 
    /// parameter objects can be added, replaced or removed after the parameter set is created.</summary>
    /// <typeparam name="ModelParameterType">Type of parameter objects contained in the set, must implement
    /// the <see cref="IModelParameter"/> interface. Commonly, the <see cref="ModelParameter"/> and the
    /// generic <see cref="ModelParameter{ParameterType}"/> are used.</typeparam>
    public abstract class ModelParameterSetBase<ModelParameterType> : ModelParameterSetBaseFixed<ModelParameterType>,
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
            get { return ParametersDictionaryInternal[ParameterNamesInternal[whichParameter]]; }
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



    /// <summary>Contains a set of model parameters as <typeparamref name="ModelParameterType"/> objects,
    /// accessible by string keys, and some metadata such as name, title, and description of the set.
    /// <para>Parameters can be accessed both by string key (usually parameter name) and integer
    /// index. Parameters are indexed according to order in which they were added to the set.</para>
    /// <para>In this class, parameters can only be added to the set via constructors, and cannot be removed 
    /// from the set (therefore the "Fixed" suffix).</para>
    /// <para>Order in which parameters are added is kept. <see cref="ParameterList"/> returns a readonly list
    /// of parameters sorted in the orded in which paremeters were specified in constructors.</para></summary>
    public abstract class ModelParameterSetBaseFixed<ModelParameterType> : IModelParameterSetBaseFixed<ModelParameterType>
        where ModelParameterType: IModelParameter
    {

        /// <summary>Constructor, initializes the current model parameter set, optionally providing parameter
        /// objects that are added to the set.</summary>
        /// <param name="title">Title of the parameter set, defines the <see cref="Title"/> property.</param>
        /// <param name="description">Description of the parameter set, defines the <see cref="Description"/> property.</param>
        /// <param name="parameters">Parameter objects to initalize the parameter set. Name of eacch paremeter object
        /// (<see cref="ModelParameterType.Name"/>) is used as access key for that parameter.</param>
        public ModelParameterSetBaseFixed(string title, string description, params ModelParameterType[] parameters)
        {
            CanAddParameters = false;
            Title = title;
            Description = description;
            AddParameters(false /* replacement of parameters is not allowed during initialization */,
                true /* isInitializing */, parameters);
        }


        /// <summary>Constructor, initializes the current model parameter set, optionally using tuples of names and 
        /// parameter objects that are added to the set.</summary>
        /// <param name="title">Title of the parameter set, defines the <see cref="Title"/> property.</param>
        /// <param name="description">Description of the parameter set, defines the <see cref="Description"/> property.</param>
        /// <param name="keysAndParameters">Tuples of names (used as keys keys) and the corresponding parameter objects 
        /// to initalize the parameter set.</param>
        public ModelParameterSetBaseFixed(string title, string description, 
            params (string Name, ModelParameterType Parameter)[]  keysAndParameters)
        {
            CanAddParameters = false;
            Title = title;
            Description = description;
            AddParameters(false /* replacement of parameters is not allowed during initialization */,
                true /* isInitializing */, keysAndParameters);
        }

        ///// <summary>The same as <see cref="ModelParameterSetBase(bool, string, string, 
        ///// (string Name, ModelParameterType Parameter)[])"/>, but with first boolean parameter (defining 
        ///// <see cref="CanAddParameters"/>) set to <see cref="DefaultCanAddParemeters"/>.</summary>
        //public ModelParameterSetBaseFixed(string title, string description,
        //    params (string Name, ModelParameterType Parameter)[] keysAndParameters) :
        //    this(DefaultCanAddParemeters, title, description, keysAndParameters)
        //{ }



        protected Dictionary<string, ModelParameterType> ParametersDictionaryInternal { get; } =
            new Dictionary<string, ModelParameterType>();

        protected List<string> ParameterNamesInternal { get; } = new List<string>();

        protected void AddParameters(bool canReplaceParameter, bool isInitializing, params ModelParameterType[] parameters)
        {
            if (parameters != null && parameters.Length > 0)
            {
                foreach (ModelParameterType parameter in parameters)
                    AddParameter(parameter.Name, parameter, canReplaceParameter, isInitializing);
            }
        }

        protected void AddParameters(bool canReplaceParameter, bool isInitializing,
            params (string Name, ModelParameterType Parameter)[] keysAndParameters)
        {
            if (keysAndParameters != null && keysAndParameters.Length > 0)
            {
                foreach ((string name, ModelParameterType parameter) in keysAndParameters)
                {
                    AddParameter(name, parameter, canReplaceParameter, isInitializing);
                }
            }
        }


        /// <summary>Adds the specified parameter <paramref name="parameter"/> with access key <paramref name="parameterKey"/>
        /// to the current parameter set. Exception is thrown if adding paremeter cannot be performed.</summary>
        /// <param name="parameterKey">Key under which parameter is added.<param>
        /// <param name="parameter">Parameter that is added.</param>
        /// <param name="canReplaceParameter">Whether or not this call is allowed to replace the paremeter that is already
        /// contained under the ket <paramref name="parameterKey"/>. If this paremeter is false and parameter under
        /// the key <paramref name="parameterKey"/> is already contained in the set then exception is thrown.</param>
        /// <param name="isInitializing">Whether the object is currently initializing. This information is used to form
        /// more meaningful messages in case of exceptions.</param>
        /// <exception cref="ArgumentNullException">When  <paramref name="parameterKey"/> is null.</exception>
        /// <exception cref="ArgumentException">When  <paramref name="parameterKey"/> is an empty string.</exception>
        /// <exception cref="InvalidOperationException">When parameter with the key <paramref name="parameterKey"/>
        /// is already contained in the set and replacing is not allowed (i.e., <paramref name="canReplaceParameter"/> 
        /// is false)</exception>
        protected void AddParameter(string parameterKey, ModelParameterType parameter, bool canReplaceParameter, bool isInitializing)
        {
            if (parameterKey == null)
            {
                throw new ArgumentNullException(parameterKey, "Cannot add a model parameter whose name is null.");
            }
            if (string.IsNullOrEmpty(parameterKey))
            {
                throw new ArgumentException("Cannot add a model parameter whose name is an empty string.", nameof(parameterKey));
            }
            bool alreadyContainsParameter = ParametersDictionaryInternal.ContainsKey(parameterKey);
            if (alreadyContainsParameter && !canReplaceParameter)
            {
                if (isInitializing)
                {
                    throw new InvalidOperationException($"Cannot add parameter \"{parameterKey}\" twice during initialization.");
                }
                throw new InvalidOperationException($"Parameter \"{parameterKey}\" is already contained in the set and it cannot be replaced.");
            }
            ParametersDictionaryInternal[parameterKey] = parameter;
            if (!alreadyContainsParameter)
            {
                ParameterNamesInternal.Add(parameterKey);
            }
        }

        public string Title { get; protected set; }

        public string Description { get; protected set; }

        /// <summary>Number of parameters currently contained in this set.</summary>
        public int Count => ParametersDictionaryInternal.Count;

        /// <inheritdoc/>
        public virtual ModelParameterType this[string parameterName]
        {
            get
            {
                return ParametersDictionaryInternal[parameterName];
            }
        }

        /// <inheritdoc/>
        public virtual ModelParameterType this[int whichParameter]
        {
            get { return ParametersDictionaryInternal[ParameterNamesInternal[whichParameter]]; }
        }

        public IReadOnlyList<ModelParameterType> ParameterList => ParameterNamesInternal
            .Select(name => ParametersDictionaryInternal[name]).ToList();

        /// <summary>The default value of <see cref="IModelParameterSetBaseFixed{ModelParameterType}.CanAddParameters"/>,
        /// used for consistent initialization across different ways of initialization and accross types for 
        /// which this property is not predetermined (fixed).</summary>
        public const bool DefaultCanAddParemeters = true;

        public bool CanAddParameters { get; init; } = DefaultCanAddParemeters;


        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"ParameterList set");
            sb.AppendLine($"  Title: \"{Title}\"");
            sb.AppendLine($"  Description: \"{Description}\"");
            var parameterList = ParameterList;
            for (int i = 0; i < parameterList.Count; ++i)
            {
                sb.AppendLine($"Parameter No. {i}:");
                sb.Append(ParameterList[i].ToString());
            }
            return sb.ToString();
        }



        /// <summary>Creates a sample <see cref="ModelParameterSet"/> object, prints its content to console,
        /// and returns it.</summary>
        public static ModelParameterSetBase<ModelParameterType> CreateExampleParameterSet2()
        {
            ModelParameterSetBase<ModelParameterType> paramSet = null;

            // paramSet = 
            //= new ModelParameterSetGeneric<ModelParameterType>(
            //"Parameters of Lisajous curve",
            //"This parameter set defines parameters of the Lisajous curve in parametric form, in cartesian coordinates.",
            //new ModelParameter<ModelParameterType>(
            //    "Param1", typeof(double), "Parameter Φ1, phase shift in the first direction.",
            //    "This parameter of type double specifies the phase shift of the 3D Lissajous curve in the direction X.",
            //    (double)0, Math.PI / 4.0
            //)
            //{
            //    //Title = "Parameter Φ1, phase shift in the first direction.",
            //    //Description = "This parameter of type double specifies the phase shift of the 3D Lissajous curve in the direction X.",
            //    //DefaultValueObject = (double)0,
            //    //IsValueDefined = false
            //}
            //,
            //new ModelParameter<double>("Param2", "Parameter Φ2, phase shift in the second direction.",
            //    "This parameter of type double specifies the phase shift of the 3D Lissajous curve in the direction Y.",
            //    (double)0, 22.44)
            //{
            //    //Title = "Parameter Φ2, phase shift in the first direction.",
            //    //Description = "This parameter of type double specifies the phase shift of the 3D Lissajous curve in the direction Y.",
            //    //DefaultValueObject = (double)0,
            //    //Value = 22.44,
            //    //IsValueDefined = true
            //}
            //);

            Console.WriteLine($"Created {paramSet.GetType()} object:\n{paramSet.ToString()}");
            return paramSet;
        }


    }
}
