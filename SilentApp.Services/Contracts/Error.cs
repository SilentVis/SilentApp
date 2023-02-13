namespace SilentApp.Services.Contracts
{
    public class Error
    {
        public Error(ErrorType type, string code, string message)
        {
            Type = type;
            Code = code;
            Message = message;
        }

        public ErrorType Type { get; }

        public string Code { get; }

        public string  Message { get; }
    }
}
