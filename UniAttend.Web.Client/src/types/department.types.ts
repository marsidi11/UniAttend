export interface Department {
    id: number;
    name: string;
    isActive: boolean;
    subjects?: Subject[];
    students?: Student[];
    professors?: Professor[];
}

export interface Subject {
    id: number;
    name: string;
    description?: string;
    credits: number;
    departmentId: number;
}

export interface Student {
    id: number;
    studentId: string;
    firstName: string;
    lastName: string;
    email: string;
    departmentId: number;
}

export interface Professor {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
    departmentId: number;
}

// DTOs for API requests
export interface CreateDepartmentRequest {
    name: string;
}

export interface UpdateDepartmentRequest {
    name: string;
    isActive: boolean;
}

// DTOs for API responses
export interface DepartmentResponse extends Department {
    subjectsCount: number;
    studentsCount: number;
    professorsCount: number;
}

export interface DepartmentDetailResponse extends Department {
    subjects: Subject[];
    students: Student[];
    professors: Professor[];
}