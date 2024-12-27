namespace UniAttend.Core.Enums
{
    /// <summary>
    /// Defines the possible states of a student's attendance record
    /// </summary>
    public enum AttendanceStatus
    {
        /// <summary>
        /// Student has checked in and attendance is confirmed
        /// </summary>
        Present = 1,

        /// <summary>
        /// Student has checked in but attendance needs confirmation
        /// </summary>
        PendingConfirmation = 2,

        /// <summary>
        /// Student was marked as absent
        /// </summary>
        Absent = 3,

        /// <summary>
        /// Student has an excused absence
        /// </summary>
        Excused = 4
    }
}