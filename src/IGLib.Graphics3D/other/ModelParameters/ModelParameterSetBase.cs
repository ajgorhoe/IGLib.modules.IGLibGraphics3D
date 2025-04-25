using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IGLib.Core
{

    /// <summary>Contains a set of model parameters as <see cref="ModelParameter"/> objects.
    /// <para>It contains metadata such as name, title, and description of the set.</para>
    /// <para>Parameters can be referenced both by parameter name and parameter index. Therefore,
    /// parameters can be ordered.</para></summary>
    /// 
    /// <inheritdoc/>
    /// <remarks>Parameters can only be added to the set via constructors. Parameters cannot be removed from the set.
    /// <para>Stating parameter of certain name twice throws exception.</para>
    /// <para>Order in which parameters are added is kept. <see cref="ParameterList"/> returns a readonly list
    /// of parameters sorted in the orded in which paremeters were specified in constructors.</para></remarks>
    class ModelParameterSetBase<ModelParameterType> : IModelParameterSet<ModelParameterType>
        where ModelParameterType: IModelParameter
    {

        /// <summary>Constructor, initializes the current model parameter set, optionally providing parameter
        /// objects that are added to the set.</summary>
        /// <param name="title">Title of the parameter set, defines the <see cref="Title"/> property.</param>
        /// <param name="description">Description of the parameter set, defines the <see cref="Description"/> property.</param>
        /// <param name="parameters">Parameter objects to initalize the parameter set. Name of eacch paremeter object
        /// (<see cref="ModelParameterType.Name"/>) is used as access key for that parameter.</param>
        public ModelParameterSetBase(string title, string description, params ModelParameterType[] parameters)
        {
            Title = title;
            Description = description;
        }

        /// <summary>Constructor, initializes the current model parameter set, optionally using tuples of names and 
        /// parameter objects that are added to the set.</summary>
        /// <param name="title">Title of the parameter set, defines the <see cref="Title"/> property.</param>
        /// <param name="description">Description of the parameter set, defines the <see cref="Description"/> property.</param>
        /// <param name="namesAndParameters">Tuples of names (used as keys keys) and the corresponding parameter objects 
        /// to initalize the parameter set.</param>
        public ModelParameterSetBase(string title, string description, 
            params (string Name, ModelParameterType Parameter)[]  namesAndParameters)
        {
            Title = title;
            Description = description;
            if (namesAndParameters != null && namesAndParameters.Length > 0)
            {
                foreach ((string name, ModelParameterType parameter) in namesAndParameters)
                    AddParameter(name, parameter, false /* replacement of parameters is not allowed during initialization */,
                        true /* isInitializing */);
            }
        }



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


        /// <summary>Adds the specified parameter <paramref name="parameter"/> with access key <paramref name="parameterKey"/>
        /// to the current parameter set. Exception is thrown if adding paremeter cannot be performed.</summary>
        /// <param name="parameterKey">Key under which parameter is added.<param>
        /// <param name="parameter">Parameter that is added.</param>
        /// <param name="canReplaceParameter">Whether or not this call is allowed to replace the paremeter that is already
        /// contained under the ket <paramref name="parameterKey"/>. If this paremeter is false and parameter under
        /// the key <paramref name="parameterKey"/> is already contained in the set then exception is thrown.</param>
        /// <param name="isInitializing">Whether the object is currently initializing. This information is only used to form
        /// more meaningful messages in case of exceptions.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
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

        public ModelParameterType this[string parameterName]
        {
            get
            {
                return ParametersDictionaryInternal[parameterName];
            }
        }

        public ModelParameterType this[int whichParameter]
        {
            get { return ParametersDictionaryInternal[ParameterNamesInternal[whichParameter]]; }
        }

        public IReadOnlyList<ModelParameterType> ParameterList => ParameterNamesInternal
            .Select(name => ParametersDictionaryInternal[name]).ToList();

        /// <summary>The default value of <see cref="IModelParameterSet{ModelParameterType}.CanAddParameters"/>,
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
