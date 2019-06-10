using MRIdentityClient.Exception.Basic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MRIdentityClient.Exception.MRSystem
{
    public class NotEnoughRightsException : MRException
    {
        public NotEnoughRightsException() : base((int)ExceptionCodes.NOT_ENOUGH_RIGHTS, "Not enough rights") { }
    }
}
