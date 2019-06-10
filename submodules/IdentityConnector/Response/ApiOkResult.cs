using System;
using System.Collections.Generic;
using System.Text;

namespace MRIdentityClient.Response
{
    public class ApiOkResult
    {
        public bool Success { get; set; }

        public ApiOkResult() : this(true) { }
        public ApiOkResult(bool result)
        {
            Success = result;
        }
    }
}
