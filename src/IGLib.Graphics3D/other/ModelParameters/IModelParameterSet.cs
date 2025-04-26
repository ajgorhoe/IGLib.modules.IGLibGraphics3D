using System.Collections.Generic;

namespace IGLib.Core
{

    /// <summary>Contains infomrmation on modle parameters in a specific parameter set. This
    /// usually represents all parameters that are used in the certain parameterized model,
    /// or a related group of model parameters.</summary>
    public interface IModelParameterSetFixed: IModelParameterSetBaseFixed<IModelParameter>
    {  }

}