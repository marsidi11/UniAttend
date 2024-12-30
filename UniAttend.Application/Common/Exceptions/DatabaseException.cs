namespace UniAttend.Application.Common.Exceptions
{
    public class DatabaseException : ApplicationException
    {
        public DatabaseException(string message) : base(message) { }
        public DatabaseException(string message, Exception inner) : base(message, inner) { }
    }
}