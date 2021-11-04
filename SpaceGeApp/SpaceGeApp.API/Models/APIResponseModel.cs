using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SpaceGeApp.API.Models
{
    /// <summary>
    /// For response status code.
    /// </summary>
    public enum ApiResponseCode
    {
        /// <summary>
        /// Success api operation.
        /// </summary>
        Success = 310,

        /// <summary>
        /// Restricted operation for some restriction rule.
        /// </summary>
        Restriction = 320,
        /// <summary>
        /// Input validation error.
        /// </summary>
        Validation = 330,
        /// <summary>
        /// Dependent internal error.
        /// </summary>
        InternalResource = 340,

        /// <summary>
        /// Dependent external as (network,database fr.) error.
        /// </summary>
        ExternalResource = 350,

        /// <summary>
        /// Fail api operation.
        /// </summary>
        Fail = 350
    }

    /// <summary>
    /// Api search result (output) model
    /// </summary>
    [Serializable]
    [DataContract(Name = "ApiResponse")]
    public class ApiResponseModel
    {
        /// <summary>
        /// Response error messages.
        /// </summary>
        [DataMember(Name = "ErrorMessages")]
        public IEnumerable<string> ErrorMessages { get; internal set; }

        /// <summary>
        /// For status convention detail.
        /// </summary>
        [DataMember(Name = "Code")]
        public ApiResponseCode Code { get; internal set; }

        /// <summary>
        /// Has any error. Can only true or false.
        /// </summary>
        [DataMember(Name = "IsSuccess")]
        public bool IsSuccess
        {
            get
            {
                if (this.ErrorMessages != null && this.ErrorMessages.Count() > 0)
                    return false;
                else
                    return true;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiResponseModel() { }

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiResponseModel(ApiResponseCode code, IEnumerable<string> errorMessages)
        {
            this.Code = code;
            this.ErrorMessages = errorMessages;
        }

        public static ApiResponseModel Create(ApiResponseCode code, params string[] errorMessages)
        {
            var errors = new List<string>();
            errors.AddRange(errorMessages);
            return new ApiResponseModel(code, errors);
        }

        public static ApiResponseModel Create(ApiResponseCode code, ModelStateDictionary modelState)
        {
            var errors = new List<string>();
            foreach (var item in modelState)
                errors.AddRange(item.Value.Errors.Select(x => x.ErrorMessage));

            return new ApiResponseModel(code, errors);
        }
    }

    /// <summary>
    /// Api search result (output) model. Success model with response data.
    /// </summary>
    [Serializable]
    [DataContract(Name = "ApiResponse")]
    public class ApiResponseModel<TData> : ApiResponseModel
    {
        /// <summary>
        /// Found document.
        /// </summary>
        [DataMember(Name = "Data")]
        public TData Data { get; internal set; }


        /// <summary>
        /// Constructor
        /// </summary>
        public ApiResponseModel(TData data, ApiResponseCode code = ApiResponseCode.Success)
        {
            this.Code = code;
            this.Data = data;
        }
    }
}
