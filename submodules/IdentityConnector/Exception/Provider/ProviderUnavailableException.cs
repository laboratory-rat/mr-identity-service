using MRIdentityClient.Exception.Basic;

namespace MRIdentityClient.Exception.Provider
{
    public class ProviderUnavaliableException : MRException
    {
        public ProviderUnavaliableException(string name) : base((int)ExceptionCodes.PROVIDER_UNAVAILABLE, $"Provider {name} is unavailable.") { }
    }
}
