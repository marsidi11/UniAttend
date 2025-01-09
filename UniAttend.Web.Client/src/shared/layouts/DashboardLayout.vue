<template>
  <div class="min-h-screen bg-gray-100">
    <!-- Mobile Menu Button -->
    <div class="lg:hidden fixed top-4 left-4 z-20">
      <button @click="isSidebarOpen = !isSidebarOpen" class="p-2 rounded-md text-white bg-indigo-600">
        <span class="material-icons">{{ isSidebarOpen ? 'close' : 'menu' }}</span>
      </button>
    </div>

    <!-- Sidebar -->
    <aside
      class="fixed inset-y-0 left-0 z-10 w-64 bg-indigo-700 transform transition-transform duration-300 ease-in-out"
      :class="[isSidebarOpen ? 'translate-x-0' : '-translate-x-full lg:translate-x-0']">
      <!-- Logo -->
      <div class="flex items-center justify-center h-16 bg-indigo-800">
        <router-link to="/dashboard" class="flex items-center space-x-3">
          <span class="material-icons text-white text-2xl">school</span>
          <span class="text-white text-xl font-bold">UniAttend</span>
        </router-link>
      </div>

      <!-- Navigation -->
      <nav class="flex-1 px-4 py-4 space-y-4 overflow-y-auto">
        <!-- Admin Navigation -->
        <template v-if="isAdmin">
          <router-link v-for="link in adminLinks" :key="link.to" :to="link.to"
            class="flex items-center px-4 py-2 text-white rounded-md hover:bg-indigo-600 transition-colors"
            :class="{ 'bg-indigo-800': route.path === link.to }">
            <span class="material-icons mr-3">{{ link.icon }}</span>
            <span>{{ link.text }}</span>
          </router-link>
        </template>

        <!-- Secretary Navigation -->
        <template v-if="isSecretary">
          <router-link v-for="link in secretaryLinks" :key="link.to" :to="link.to"
            class="flex items-center px-4 py-2 text-white rounded-md hover:bg-indigo-600 transition-colors"
            :class="{ 'bg-indigo-800': route.path === link.to }">
            <span class="material-icons mr-3">{{ link.icon }}</span>
            <span>{{ link.text }}</span>
          </router-link>
        </template>

        <!-- Professor Navigation -->
        <template v-if="isProfessor">
          <router-link v-for="link in professorLinks" :key="link.to" :to="link.to"
            class="flex items-center px-4 py-2 text-white rounded-md hover:bg-indigo-600 transition-colors"
            :class="{ 'bg-indigo-800': route.path === link.to }">
            <span class="material-icons mr-3">{{ link.icon }}</span>
            <span>{{ link.text }}</span>
          </router-link>
        </template>

        <!-- Student Navigation -->
        <template v-if="isStudent">
          <router-link v-for="link in studentLinks" :key="link.to" :to="link.to"
            class="flex items-center px-4 py-2 text-white rounded-md hover:bg-indigo-600 transition-colors"
            :class="{ 'bg-indigo-800': route.path === link.to }">
            <span class="material-icons mr-3">{{ link.icon }}</span>
            <span>{{ link.text }}</span>
          </router-link>
        </template>
      </nav>

      <!-- User Profile Section -->
      <div class="border-t border-indigo-800 p-4">
  <div class="flex items-center justify-between">
    <div>
      <p class="text-sm font-medium text-white">{{ userName }}</p>
      <p class="text-xs text-indigo-200 capitalize">{{ userRole }}</p>
    </div>
    <div class="relative user-menu">
      <button 
        @click.stop="toggleUserMenu"
        class="p-2 text-white rounded-full hover:bg-indigo-600 focus:outline-none"
      >
        <span class="material-icons">more_vert</span>
      </button>
      
      <div v-show="isUserMenuOpen"
        class="absolute right-0 bottom-12 w-48 rounded-md shadow-lg bg-white ring-1 ring-black ring-opacity-5 z-50">
        <div class="py-1">
          <router-link 
            to="/dashboard/profile" 
            class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
            @click="isUserMenuOpen = false"
          >
            Profile Settings
          </router-link>
          <button 
            @click="handleLogout"
            class="block w-full text-left px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
          >
            Sign Out
          </button>
        </div>
      </div>
    </div>
  </div>
</div>
    </aside>

    <!-- Main Content -->
    <div class="lg:pl-64 min-h-screen">
      <header class="bg-white shadow-sm">
        <div class="px-4 sm:px-6 lg:px-8 py-4">
          <div class="flex justify-between items-center">
            <h1 class="text-xl font-semibold text-gray-900">{{ pageTitle }}</h1>
            <slot name="header-actions" />
          </div>
        </div>
      </header>
      <main class="p-6">
        <slot />
      </main>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth.store'

// Router and Store Setup
const route = useRoute()
const router = useRouter()
const authStore = useAuthStore()

// State
const isSidebarOpen = ref(false)
const isUserMenuOpen = ref(false)

// Role-based Navigation Links
const adminLinks = [
  { to: '/dashboard/admin', icon: 'dashboard', text: 'Dashboard' },
  { to: '/dashboard/departments', icon: 'business', text: 'Departments' },
  { to: '/dashboard/users', icon: 'people', text: 'Users' },
  { to: '/dashboard/academic-years', icon: 'event', text: 'Academic Years' },
  { to: '/dashboard/subjects', icon: 'book', text: 'Subjects' },
  { to: '/dashboard/reports', icon: 'analytics', text: 'Reports' }
]

const secretaryLinks = [
  { to: '/dashboard/secretary', icon: 'dashboard', text: 'Dashboard' },
  { to: '/dashboard/students', icon: 'school', text: 'Students' },
  { to: '/dashboard/cards', icon: 'credit_card', text: 'Cards' },
  { to: '/dashboard/schedule', icon: 'schedule', text: 'Schedule' },
  { to: '/dashboard/groups', icon: 'groups', text: 'Groups' },
  { to: '/dashboard/classrooms', icon: 'meeting_room', text: 'Classrooms' }
]

const professorLinks = [
  { to: '/dashboard/professor', icon: 'dashboard', text: 'Dashboard' },
  { to: '/dashboard/attendance/manage', icon: 'how_to_reg', text: 'Manage Classes' },
  { to: '/dashboard/attendance/records', icon: 'fact_check', text: 'Attendance Records' },
  { to: '/dashboard/groups', icon: 'groups', text: 'My Groups' },
  { to: '/dashboard/reports', icon: 'analytics', text: 'Reports' }
]

const studentLinks = [
  { to: '/dashboard/student', icon: 'dashboard', text: 'Dashboard' },
  { to: '/dashboard/attendance/view', icon: 'schedule', text: 'My Attendance' },
  { to: '/dashboard/attendance/check-in', icon: 'qr_code_scanner', text: 'Check In' },
  { to: '/dashboard/attendance/otp', icon: 'pin', text: 'OTP Check-in' }
]

// Computed Properties
const userName = computed(() => {
  const firstName = authStore.user?.firstName || ''
  const lastName = authStore.user?.lastName || ''
  return `${firstName} ${lastName}`.trim()
})

const userRole = computed(() => authStore.user?.role || '')
const pageTitle = computed(() => route.meta.title || 'Dashboard')

const isAdmin = computed(() => userRole.value.toLowerCase() === 'admin')
const isSecretary = computed(() => userRole.value.toLowerCase() === 'secretary')
const isProfessor = computed(() => userRole.value.toLowerCase() === 'professor')
const isStudent = computed(() => userRole.value.toLowerCase() === 'student')

// Methods
const toggleUserMenu = () => {
  isUserMenuOpen.value = !isUserMenuOpen.value
}

const handleLogout = async () => {
  try {
    await authStore.logout()
    router.push('/login')
  } catch (error) {
    console.error('Logout failed:', error)
  }
}

const handleClickOutside = (event: MouseEvent) => {
  const target = event.target as HTMLElement
  if (!target.closest('.user-menu')) {
    isUserMenuOpen.value = false
  }
}

// Lifecycle Hooks
onMounted(() => {
  document.addEventListener('click', handleClickOutside)
})

onUnmounted(() => {
  document.removeEventListener('click', handleClickOutside)
})
</script>

<style scoped>
.material-icons {
  font-size: 20px;
}

@media (max-width: 1024px) {
  .body-overflow-hidden {
    overflow: hidden;
  }
}
</style>