using UniAttend.Core.Entities.Base;

namespace UniAttend.Core.Entities
{
    public class Classroom : Entity
    {
        public Classroom(string name, string? readerDeviceId)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ReaderDeviceId = readerDeviceId;
        }
        public string Name { get; }
        public string? ReaderDeviceId { get; set; }
    }
}