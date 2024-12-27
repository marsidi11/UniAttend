namespace UniAttend.Core.Enums
{
    /// <summary>
    /// Defines the roles available in the system with their corresponding access levels
    /// </summary>
    public enum UserRole
    {
        /// <summary>
        /// Has full system access and management capabilities
        /// </summary>
        Admin = 1,

        /// <summary>
        /// Manages student records and academic schedules
        /// </summary>
        Secretary = 2,

        /// <summary>
        /// Manages classes and attendance for assigned courses
        /// </summary>
        Professor = 3,

        /// <summary>
        /// Basic access for attendance and viewing their records
        /// </summary>
        Student = 4
    }
}