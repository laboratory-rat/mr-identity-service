namespace MRIdentityClient.Response
{
    public class IdentityResponse<T>
        where T : class, new()
    {
        public T Response { get; set; }
        public bool IsSuccess => Response != null;
        public ApiError Error { get; set; }
    }
}
