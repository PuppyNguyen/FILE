using System;
using System.Collections.Generic;

namespace Ezbuy.Cdn.Model
{
    public class DetailError
    {
        public string ErrorCode { get; }

        public IDictionary<string, string> Parameters { get; }

        public DetailError(string errorCode, IDictionary<string, string> parameters)
        {
            ErrorCode = errorCode;
            Parameters = parameters;
        }
    }
    public class BaseResponse
    {
        public bool Status { get; set; }

        public IList<DetailError> DetailErrors { get; set; }

        public BaseResponse(bool status, IList<DetailError> detailErrors)
        {
            Status = status;
            DetailErrors = detailErrors;
        }

        public BaseResponse()
        {
        }

        public void Fail(IList<DetailError> detailErrors)
        {
            Status = false;
            DetailErrors = detailErrors;
        }
    }
}