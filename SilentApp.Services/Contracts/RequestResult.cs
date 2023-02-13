namespace SilentApp.Services.Contracts
{
    public class RequestResult
    {
        public RequestResult() { }

        public RequestResult(Error error)
        {
            Error = error;
        }


        public Error? Error { get; }

        public bool IsSuccessful => Error == null;
    }

    public class RequestResult<T> : RequestResult
    {
        public RequestResult(T? data)
        {
            Data = data;
        }

        public RequestResult(Error error) : base(error) { }

        private T? Data { get; }
    }
}
