using UniAttend.Core.Entities.Base;
using UniAttend.Core.Exceptions;

namespace UniAttend.Core.Entities
{
    public class Classroom : Entity
    {
        private Classroom() { } // For domain events/serialization
    
        public Classroom(string name, string? readerDeviceId)
        {
            ValidateName(name);
            
            Name = name;
            ReaderDeviceId = readerDeviceId;
        }
    
        // Identity properties - immutable
        public string Name { get; private set; }
        public string? ReaderDeviceId { get; private set; }
    
        // Domain methods
        public void AssignReader(string deviceId)
        {
            ValidateDeviceId(deviceId);
            ReaderDeviceId = deviceId;
        }
    
        public void RemoveReader()
        {
            ReaderDeviceId = null;
        }
    
        // Domain validation
        private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Classroom name cannot be empty");
            if (name.Length > 100)
                throw new DomainException("Classroom name cannot exceed 100 characters");
        }
    
        private void ValidateDeviceId(string deviceId)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
                throw new DomainException("Device ID cannot be empty");
        }
    }
}