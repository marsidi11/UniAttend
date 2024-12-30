namespace UniAttend.Application.Common.Exceptions
{
    public class SecurityException : ApplicationException
    {
        public SecurityException(string message) : base(message) { }
        public SecurityException(string message, Exception inner) : base(message, inner) { }
    }
}