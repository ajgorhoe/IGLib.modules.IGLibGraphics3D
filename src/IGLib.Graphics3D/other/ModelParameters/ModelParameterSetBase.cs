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

        /// <summary>Constructor, initializes the current model parameters set.</summary>
        /// <param name="title">Title of the parameters set, defines the <see cref="Title"/> property.</param>
        /// <param name="description">Descriptio nof the parameters set, defines the <see cref="Description"/> property.</param>
        /// <param name="modelParameters"></param>
        public ModelParameterSetBase(string title, string description, params ModelParameterType[] modelParameters)
        {
            Title = title;
            Description = description;
            if (modelParameters != null && modelParameters.Length > 0)
            {
                foreach (ModelParameterType parameter in modelParameters)
                    AddParameter(parameter.Name, parameter);
            }
        }

        protected Dictionary<string, ModelParameterType> ParametersDictionaryInternal { get; } =
            new Dictionary<string, ModelParameterType>();

        protected List<string> ParameterNamesInternal { get; } = new List<string>();

        protected void AddParameter(ModelParameterType parameter)
        {
            AddParameter(parameter?.Name, parameter);
        }

        protected void AddParameter(string parameterName, ModelParameterType parameter)
        {
            if (parameterName == null)
            {
                throw new ArgumentNullException(parameterName, "Cannot add a model parameter whose name is null.");
            }
            if (string.IsNullOrEmpty(parameterName))
            {
                throw new ArgumentException("Cannot add a model parameter whose name is an empty string.", nameof(parameterName));
            }
            if (ParametersDictionaryInternal.ContainsKey("name"))
            {
                throw new InvalidOperationException($"Parameter {parameterName} is already contained in the set, you can only add a parameter once.");
            }
            else
            {
                ParametersDictionaryInternal[parameterName] = parameter;
                ParameterNamesInternal.Add(parameterName);
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
