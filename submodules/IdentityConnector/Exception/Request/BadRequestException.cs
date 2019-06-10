using MRIdentityClient.Exception.Basic;

namespace MRIdentityClient.Exception.Request
{
    public class BadRequestException : MRException
    {
        public BadRequestException() : base((int)ExceptionCodes.BAD_REQUEST, "Bad request") { }
    }
}
