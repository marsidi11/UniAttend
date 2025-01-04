import { createRouter, createWebHistory } from 'vue-router';
import { useAuthStore } from '@/stores/auth.store';

// Layouts
import MainLayout from '@/shared/layouts/MainLayout.vue';
import AuthLayout from '@/shared/layouts/AuthLayout.vue';
import DashboardLayout from '@/shared/layouts/DashboardLayout.vue';

// Public pages
import HomePage from '@/features/home/pages/HomePage.vue';
import LoginPage from '@/features/auth/pages/LoginPage.vue';
import RegisterPage from '@/features/auth/pages/RegisterPage.vue';

// Dashboard pages
import AdminDashboard from '@/features/dashboard/pages/AdminDashboard.vue';
import SecretaryDashboard from '@/features/dashboard/pages/SecretaryDashboard.vue';
import ProfessorDashboard from '@/features/dashboard/pages/ProfessorDashboard.vue';
import StudentDashboard from '@/features/dashboard/pages/StudentDashboard.vue';

// Academic Management
import DepartmentList from '@/features/departments/pages/DepartmentList.vue';
import DepartmentDetails from '@/features/departments/pages/DepartmentDetails.vue';
import SubjectList from '@/features/subjects/pages/SubjectList.vue';
import SubjectDetails from '@/features/subjects/pages/SubjectDetails.vue';
import AcademicYearList from '@/features/academic/pages/AcademicYearList.vue';

// User Management
import UserManagement from '@/features/users/pages/UserManagement.vue';
import StudentList from '@/features/students/pages/StudentList.vue';
import StudentDetails from '@/features/students/pages/StudentDetails.vue';
import ProfessorList from '@/features/users/pages/ProfessorList.vue';
import CardManagement from '@/features/cards/pages/CardManagement.vue';

// Schedule & Groups
import ScheduleManager from '@/features/schedule/pages/ScheduleManager.vue';
import StudyGroupList from '@/features/groups/pages/StudyGroupList.vue';
import StudyGroupDetails from '@/features/groups/pages/StudyGroupDetails.vue';
import ClassroomList from '@/features/classrooms/pages/ClassroomList.vue';

// Attendance
import ProfessorAttendance from '@/features/attendance/pages/ProfessorAttendancePage.vue';
import StudentAttendance from '@/features/attendance/pages/StudentAttendancePage.vue';
import AttendanceRecords from '@/features/attendance/pages/AttendanceRecords.vue';
import OtpCheckIn from '@/features/attendance/pages/OtpCheckIn.vue';

// Reports
import Reports from '@/features/reports/pages/Reports.vue';
import AbsenceAlerts from '@/features/reports/pages/AbsenceAlerts.vue';

const routes = [
  {
    path: '/',
    component: MainLayout,
    children: [
      { path: '', name: 'home', component: HomePage },
      { path: 'login', name: 'login', component: LoginPage, meta: { requiresAuth: false, layout: 'auth' } },
      { path: 'register', name: 'register', component: RegisterPage, meta: { requiresAuth: false, layout: 'auth' } }
    ]
  },
  {
    path: '/dashboard',
    component: DashboardLayout,
    meta: { requiresAuth: true },
    children: [
      {
        path: '',
        name: 'dashboard',
        redirect: to => {
          const authStore = useAuthStore();
          const roleMap = {
            admin: 'admin-dashboard',
            secretary: 'secretary-dashboard',
            professor: 'professor-dashboard',
            student: 'student-dashboard'
          };
          return { name: roleMap[authStore.userRole.toLowerCase()] };
        }
      },
      // Admin routes
      {
        path: 'admin',
        name: 'admin-dashboard',
        component: AdminDashboard,
        meta: { roles: ['admin'] }
      },
      {
        path: 'departments',
        name: 'departments',
        component: DepartmentList,
        meta: { roles: ['admin'] }
      },
      {
        path: 'departments/:id',
        name: 'department-details',
        component: DepartmentDetails,
        meta: { roles: ['admin'] }
      },
      {
        path: 'subjects',
        name: 'subjects',
        component: SubjectList,
        meta: { roles: ['admin'] }
      },
      {
        path: 'academic-years',
        name: 'academic-years',
        component: AcademicYearList,
        meta: { roles: ['admin'] }
      },
      {
        path: 'users',
        name: 'users',
        component: UserManagement,
        meta: { roles: ['admin'] }
      },
      // Secretary routes
      {
        path: 'secretary',
        name: 'secretary-dashboard',
        component: SecretaryDashboard,
        meta: { roles: ['secretary'] }
      },
      {
        path: 'students',
        name: 'students',
        component: StudentList,
        meta: { roles: ['secretary'] }
      },
      {
        path: 'students/:id',
        name: 'student-details',
        component: StudentDetails,
        meta: { roles: ['secretary'] }
      },
      {
        path: 'cards',
        name: 'card-management',
        component: CardManagement,
        meta: { roles: ['secretary'] }
      },
      {
        path: 'schedule',
        name: 'schedule',
        component: ScheduleManager,
        meta: { roles: ['secretary'] }
      },
      {
        path: 'groups',
        name: 'study-groups',
        component: StudyGroupList,
        meta: { roles: ['secretary'] }
      },
      {
        path: 'groups/:id',
        name: 'group-details',
        component: StudyGroupDetails,
        meta: { roles: ['secretary'] }
      },
      {
        path: 'classrooms',
        name: 'classrooms',
        component: ClassroomList,
        meta: { roles: ['secretary'] }
      },
      // Professor routes
      {
        path: 'professor',
        name: 'professor-dashboard',
        component: ProfessorDashboard,
        meta: { roles: ['professor'] }
      },
      {
        path: 'attendance/manage',
        name: 'manage-attendance',
        component: ProfessorAttendance,
        meta: { roles: ['professor'] }
      },
      {
        path: 'attendance/records',
        name: 'attendance-records',
        component: AttendanceRecords,
        meta: { roles: ['professor', 'secretary'] }
      },
      // Student routes
      {
        path: 'student',
        name: 'student-dashboard',
        component: StudentDashboard,
        meta: { roles: ['student'] }
      },
      {
        path: 'attendance/view',
        name: 'view-attendance',
        component: StudentAttendance,
        meta: { roles: ['student'] }
      },
      {
        path: 'attendance/check-in',
        name: 'otp-check-in',
        component: OtpCheckIn,
        meta: { roles: ['student'] }
      },
      // Shared routes
      {
        path: 'reports',
        name: 'reports',
        component: Reports,
        meta: { roles: ['admin', 'professor', 'secretary'] }
      },
      {
        path: 'absence-alerts',
        name: 'absence-alerts',
        component: AbsenceAlerts,
        meta: { roles: ['admin', 'secretary'] }
      }
    ]
  },
  {
    path: '/:pathMatch(.*)*',
    redirect: '/'
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

router.beforeEach((to, from, next) => {
  const authStore = useAuthStore();
  const isAuthenticated = !!authStore.token;
  const userRole = authStore.userRole?.toLowerCase();

  // Handle authentication
  if (to.meta.requiresAuth && !isAuthenticated) {
    return next({ name: 'login' });
  }

  // Handle role-based access
  if (to.meta.roles && !to.meta.roles.includes(userRole)) {
    return next({ name: 'dashboard' });
  }

  // Handle authenticated users accessing auth pages
  if (isAuthenticated && to.meta.requiresAuth === false) {
    return next({ name: 'dashboard' });
  }

  next();
});

export default router;