using UniAttend.Core.Entities.Base;
using UniAttend.Core.Entities.Identity;
using UniAttend.Core.Exceptions;

namespace UniAttend.Core.Entities
{
    public class Student : Entity
    {
        private Student() { } // For domain events/serialization

        public Student(string studentId, int departmentId, User user)
        {
            ValidateStudentId(studentId);
            ValidateUser(user);
            
            StudentId = studentId;
            DepartmentId = departmentId;
            User = user;
        }

        // Identity properties - immutable
        public string StudentId { get; private init; }
        public string? CardId { get; private set; }
        public int DepartmentId { get; private init; }

        // Domain references without EF Core annotations - nullable for EF Core
        public User? User { get; private set; }
        public Department? Department { get; private set; }

        // Rest of the code remains the same...
        public void AssignCard(string cardId)
        {
            ValidateCardId(cardId);
            CardId = cardId;
        }

        public void RemoveCard()
        {
            CardId = null;
        }

        private static void ValidateStudentId(string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId))
                throw new DomainException("Student ID cannot be empty");
            if (studentId.Length > 20)
                throw new DomainException("Student ID cannot exceed 20 characters");
        }

        private static void ValidateUser(User user)
        {
            if (user == null)
                throw new DomainException("User cannot be null");
        }

        private static void ValidateCardId(string cardId)
        {
            if (string.IsNullOrWhiteSpace(cardId))
                throw new DomainException("Card ID cannot be empty");
            if (cardId.Length > 50)
                throw new DomainException("Card ID cannot exceed 50 characters");
        }
    }
}