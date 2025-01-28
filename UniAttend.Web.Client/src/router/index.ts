import { 
  createRouter, 
  createWebHistory
} from 'vue-router';
import { useAuthStore } from '@/stores/auth.store';

// Public pages
import HomePage from '@/features/home/pages/HomePage.vue';
import LoginPage from '@/features/auth/pages/LoginPage.vue';
import ForgotPasswordPage from '@/features/auth/pages/ForgotPasswordPage.vue';

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
import UserProfile from '@/features/users/pages/UserProfile.vue';
import StudentList from '@/features/students/pages/StudentList.vue';
import StudentDetails from '@/features/students/pages/StudentDetails.vue';

// Schedule & Groups
import ScheduleManager from '@/features/schedule/pages/ScheduleManager.vue';
import StudyGroupList from '@/features/studyGroups/pages/StudyGroupList.vue';
import StudyGroupDetails from '@/features/studyGroups/pages/StudyGroupDetails.vue';
import ClassroomList from '@/features/classrooms/pages/ClassroomList.vue';

// Attendance
import StudentAttendance from '@/features/attendance/pages/StudentAttendancePage.vue';
import AttendanceRecords from '@/features/attendance/pages/AttendanceRecords.vue';
import OtpCheckIn from '@/features/attendance/pages/OtpCheckIn.vue';
// import CardCheckIn from '@/features/attendance/pages/CardCheckIn.vue';

// Reports
import Reports from '@/features/reports/pages/Reports.vue';
import AbsenceAlerts from '@/features/reports/pages/AbsenceAlerts.vue';

const dashboardRedirectMap: Record<string, string> = {
  admin: 'admin-dashboard',
  secretary: 'secretary-dashboard',
  professor: 'professor-dashboard',
  student: 'student-dashboard'
};

const routes = [
  {
    path: '/',
    name: 'home',
    component: HomePage
  },
  {
    path: '/login',
    name: 'login',
    component: LoginPage,
    meta: { requiresAuth: false, layout: 'auth' }
  },
  {
    path: '/forgot-password',
    name: 'ForgotPassword',
    component: ForgotPasswordPage,
    meta: { requiresAuth: false, layout: 'auth' }
  },
  {
    path: '/dashboard',
    meta: { requiresAuth: true },
    children: [
      {
        path: '',
        name: 'dashboard',
        redirect: () => {
          const authStore = useAuthStore();
          const userRole = authStore.userRole?.toLowerCase() || '';
          return { name: dashboardRedirectMap[userRole] || 'login' };
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
        path: 'schedule',
        name: 'schedule',
        component: ScheduleManager,
        meta: { roles: ['secretary', 'professor'] }
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
      {
        path: 'attendance/otp-check-in/:sessionId',
        name: 'otp-check-in',
        component: OtpCheckIn,
        meta: { roles: ['student'] }
      },
      // {
      //   path: 'attendance/check-in/:sessionId',
      //   name: 'card-check-in',
      //   component: CardCheckIn,
      //   meta: { roles: ['student'] }
      // },
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
      },
      {
        path: 'subjects',
        name: 'subjects',
        component: SubjectList,
        meta: { roles: ['admin', 'secretary'] }  
      },
      {
        path: 'subjects/:id',
        name: 'subject-details',
        component: SubjectDetails,
        meta: { roles: ['admin', 'secretary'] }
      },
      {
        path: 'profile',
        name: 'user-profile', 
        component: UserProfile,
        meta: { requiresAuth: true }
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

router.beforeEach((to, _from, next) => {
  const authStore = useAuthStore();
  const isAuthenticated = !!authStore.token && !!authStore.user;
  const userRole = authStore.userRole?.toLowerCase();
  
  console.log('Route guard:', {
    path: to.path,
    isAuthenticated,
    userRole,
    user: authStore.user,
    token: !!authStore.token
  });

  // Handle auth required routes
  if (to.meta.requiresAuth && !isAuthenticated) {
    console.log('Not authenticated, redirecting to login');
    return next({ name: 'login' });
  }

  // Handle public routes when authenticated
  if (to.name === 'login' && isAuthenticated) {
    const redirectTarget = dashboardRedirectMap[userRole as string] || 'dashboard';
    return next({ name: redirectTarget });
  }

  // Check role access
  if (to.meta.roles && Array.isArray(to.meta.roles)) {
    if (!userRole || !to.meta.roles.includes(userRole)) {
      console.log('Unauthorized access attempt:', {
        userRole,
        requiredRoles: to.meta.roles
      });
      if (isAuthenticated) {
        return next({ name: dashboardRedirectMap[userRole as string] });
      }
      return next({ name: 'login' });
    }
  }

  next();
});

export default router;