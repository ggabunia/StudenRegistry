using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRegistryAPI.ViewModels
{
    public class ApiError
    {
        public int StatusCode { get; private set; }

        public string StatusDescription { get; private set; }
        public List<string> ErrorsList { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Message { get; private set; }

        public ApiError(int statusCode, string statusDescription)
        {
            this.StatusCode = statusCode;
            this.StatusDescription = statusDescription;
            this.ErrorsList = new List<string>();
        }

        public ApiError(int statusCode, string statusDescription, string message)
            : this(statusCode, statusDescription)
        {
            this.Message = message;
            this.ErrorsList = new List<string>();
        }

        public void AddErrors(List<string> errors)
        {
            this.ErrorsList.AddRange(errors);
        }
        public void AddError(string error)
        {
            this.ErrorsList.Add(error);
        }
    }
}
