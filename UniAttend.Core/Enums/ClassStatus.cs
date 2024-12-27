namespace UniAttend.Core.Enums
{
    /// <summary>
    /// Defines the possible states of a class session
    /// </summary>
    public enum ClassStatus
    {
        /// <summary>
        /// Class is scheduled but hasn't started
        /// </summary>
        Scheduled = 1,

        /// <summary>
        /// Class is currently in session
        /// </summary>
        InProgress = 2,

        /// <summary>
        /// Class has ended and attendance is being finalized
        /// </summary>
        PendingConfirmation = 3,

        /// <summary>
        /// Class has ended and attendance is confirmed
        /// </summary>
        Completed = 4,

        /// <summary>
        /// Class was cancelled
        /// </summary>
        Cancelled = 5
    }
}