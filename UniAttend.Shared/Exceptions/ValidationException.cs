namespace UniAttend.Shared.Exceptions
{
    public class ValidationException : Exception
    {
        public IEnumerable<string> Errors { get; }

        public ValidationException() : base()
        {
            Errors = new List<string>();
        }

        public ValidationException(string message)
            : base(message)
        {
            Errors = new[] { message };
        }

        public ValidationException(string message, IEnumerable<string> errors)
            : base(message)
        {
            Errors = errors;
        }
    }
}