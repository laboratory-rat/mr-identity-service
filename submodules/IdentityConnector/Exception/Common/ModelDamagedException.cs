using MRIdentityClient.Exception.Basic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MRIdentityClient.Exception.Common
{
    public class ModelDamagedException : MRException
    {
        public ModelDamagedException(string property, string error) : base((int)ExceptionCodes.MODEL_DAMAGED, $"Property \"{property}\" {error}") { }
        public ModelDamagedException(string message) : base((int)ExceptionCodes.MODEL_DAMAGED, message) { }
    }
}
