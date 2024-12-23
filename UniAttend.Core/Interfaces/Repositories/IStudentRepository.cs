using UniAttend.Core.Entities;
using System.Collections.Generic;

namespace UniAttend.Core.Interfaces.Repositories
{
    /// <summary>
    /// Interface for student repository to handle CRUD operations for Student entities.
    /// </summary>
    public interface IStudentRepository
    {
        Student? GetById(int id);
        List<Student> GetAll();
        void Add(Student student);
        void Update(Student student);
        void Delete(int id);
    }
}