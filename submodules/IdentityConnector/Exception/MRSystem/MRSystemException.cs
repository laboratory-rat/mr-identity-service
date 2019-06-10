using MRIdentityClient.Exception.Basic;

namespace MRIdentityClient.Exception.MRSystem
{
    public class MRSystemException : MRException
    {
        public MRSystemException() : this("System error") { }
        public MRSystemException(string message) : base((int)ExceptionCodes.SYSTEM_EXCEPTION, message) { }
    }
}
