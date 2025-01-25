interface RolePermissions {
  [key: string]: string[];
}

export const rolePermissions: RolePermissions = {
  Admin: [
    'manageDepartments',     // Create/Update/Delete departments
    'manageSubjects',        // Manage curriculum and subjects
    'manageAcademicYears',   // Control academic year settings
    'manageStaff',          // Create/Manage secretary and professor accounts
    'viewSystemReports',     // Access all system reports
    'manageSettings',        // System-wide settings
    'viewAuditLogs'         // View system audit logs
  ],
  
  Secretary: [
    'manageStudents',       // Register/Update student information
    'manageGroups',         // Create/Manage study groups
    'manageSchedule',       // Set timetables and classroom assignments
    'manageCards',          // Assign/Manage RFID/Barcode cards
    'printReports',         // Generate attendance reports
    'viewAttendanceRecords', // View all attendance records
    'manageClassrooms',     // Manage classroom settings
    'assignProfessors',     // Assign professors to groups/subjects
    'viewAbsenceAlerts'     // View student absence alerts
  ],
  
  Professor: [
    'manageAttendance',     // Open/Close class sessions
    'viewGroupReports',     // View attendance for assigned groups
    'confirmAttendance',    // Confirm daily attendance records
    'generateAttendanceLists', // Generate group attendance lists
    'viewOwnSchedule',      // View assigned schedule
    'viewOwnGroups',        // View assigned groups
    'manageOwncourseSessions'      // Manage own class sessions
  ],
  
  Student: [
    'viewAttendance',       // View personal attendance records
    'checkIn',              // Check-in to courseSessions (card/OTP)
    'viewSchedule',         // View personal schedule
    'viewAbsenceStatus',    // View absence percentages
    'useOtpSystem',         // Use OTP for attendance
    'viewEnrolledGroups'    // View enrolled groups/subjects
  ]
};

// Helper functions for permission checking
export const hasPermission = (role: string, permission: string): boolean => {
  return rolePermissions[role]?.includes(permission) ?? false;
};

export const hasAnyPermission = (role: string, permissions: string[]): boolean => {
  return permissions.some(permission => hasPermission(role, permission));
};

export const hasAllPermissions = (role: string, permissions: string[]): boolean => {
  return permissions.every(permission => hasPermission(role, permission));
};