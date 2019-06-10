using MRIdentityClient.Exception.Basic;
using System;

namespace MRIdentityClient.Exception.Common
{
    public class EntityNotFoundException : MRException
    {
        public EntityNotFoundException(string id, Type type, string uMessage = null, global::System.Exception inner = null)
            : base((int)ExceptionCodes.NOT_FOUND, $"{type.ToString()} with id {id} not found", uMessage, inner) { }
    }

    public class EntityNotFoundException<T> : MRException
    {
        public EntityNotFoundException(string id, string uMessage = null, global::System.Exception inner = null) 
            : base((int)ExceptionCodes.NOT_FOUND, $"{nameof(T)} with id {id} not found.", uMessage, inner) { }
    }

}
