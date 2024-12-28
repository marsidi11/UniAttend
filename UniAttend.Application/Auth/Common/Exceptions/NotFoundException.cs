namespace UniAttend.Application.Common.Exceptions
{
    /// <summary>
    /// Exception thrown when a requested entity is not found in the system.
    /// </summary>
    public class NotFoundException : ApplicationException
    {
        public NotFoundException()
            : base()
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }
}