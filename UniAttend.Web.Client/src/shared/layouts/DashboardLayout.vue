<template>
  <div class="min-h-screen bg-gray-100">
    <!-- Sidebar -->
    <aside class="fixed inset-y-0 left-0 w-64 bg-indigo-700 z-10">
      <div class="flex flex-col h-full">
        <!-- Logo -->
        <div class="flex items-center justify-center h-16 bg-indigo-800">
          <router-link to="/dashboard" class="text-white text-xl font-bold">
            UniAttend
          </router-link>
        </div>

        <!-- Navigation -->
        <nav class="flex-1 px-2 py-4 space-y-1 overflow-y-auto">
          <!-- Dashboard Link -->
          <router-link
            to="/dashboard"
            class="flex items-center px-4 py-2 text-white rounded-md hover:bg-indigo-600"
            :class="{ 'bg-indigo-800': $route.path === '/dashboard' }"
          >
            <span class="material-icons mr-3">dashboard</span>
            <span>Dashboard</span>
          </router-link>

          <!-- Attendance Link -->
          <router-link
            to="/dashboard/attendance"
            class="flex items-center px-4 py-2 text-white rounded-md hover:bg-indigo-600"
            :class="{ 'bg-indigo-800': $route.path.includes('attendance') }"
          >
            <span class="material-icons mr-3">how_to_reg</span>
            <span>Attendance</span>
          </router-link>

          <!-- Admin Links -->
          <template v-if="isAdmin">
            <router-link
              to="/dashboard/departments"
              class="flex items-center px-4 py-2 text-white rounded-md hover:bg-indigo-600"
              :class="{ 'bg-indigo-800': $route.path.includes('departments') }"
            >
              <span class="material-icons mr-3">business</span>
              <span>Departments</span>
            </router-link>

            <router-link
              to="/dashboard/users"
              class="flex items-center px-4 py-2 text-white rounded-md hover:bg-indigo-600"
              :class="{ 'bg-indigo-800': $route.path.includes('users') }"
            >
              <span class="material-icons mr-3">people</span>
              <span>Users</span>
            </router-link>
          </template>

          <!-- Secretary Links -->
          <template v-if="isSecretary">
            <router-link
              to="/dashboard/students"
              class="flex items-center px-4 py-2 text-white rounded-md hover:bg-indigo-600"
              :class="{ 'bg-indigo-800': $route.path.includes('students') }"
            >
              <span class="material-icons mr-3">school</span>
              <span>Students</span>
            </router-link>

            <router-link
              to="/dashboard/schedule"
              class="flex items-center px-4 py-2 text-white rounded-md hover:bg-indigo-600"
              :class="{ 'bg-indigo-800': $route.path.includes('schedule') }"
            >
              <span class="material-icons mr-3">schedule</span>
              <span>Schedule</span>
            </router-link>
          </template>

          <!-- Reports Link -->
          <router-link
            v-if="canViewReports"
            to="/dashboard/reports"
            class="flex items-center px-4 py-2 text-white rounded-md hover:bg-indigo-600"
            :class="{ 'bg-indigo-800': $route.path.includes('reports') }"
          >
            <span class="material-icons mr-3">analytics</span>
            <span>Reports</span>
          </router-link>
        </nav>

        <!-- User Menu -->
        <div class="p-4 border-t border-indigo-800">
          <div class="flex items-center">
            <div class="flex-1">
              <p class="text-sm font-medium text-white">{{ userName }}</p>
              <p class="text-xs text-indigo-200">{{ userRole }}</p>
            </div>
            <div class="relative">
              <button
                @click="isUserMenuOpen = !isUserMenuOpen"
                class="p-1 text-indigo-200 hover:text-white"
              >
                <span class="material-icons">more_vert</span>
              </button>

              <!-- Dropdown Menu -->
              <div
                v-if="isUserMenuOpen"
                class="absolute right-0 bottom-full mb-2 w-48 rounded-md shadow-lg bg-white ring-1 ring-black ring-opacity-5"
              >
                <div class="py-1">
                  <router-link
                    to="/dashboard/profile"
                    class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
                  >
                    Profile Settings
                  </router-link>
                  <button
                    @click="logout"
                    class="block w-full text-left px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
                  >
                    Sign Out
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </aside>

    <!-- Main Content -->
    <div class="pl-64">
      <!-- Top Header -->
      <header class="bg-white shadow">
        <div class="px-4 py-4 sm:px-6 flex justify-between items-center">
          <h1 class="text-lg font-semibold text-gray-900">
            {{ pageTitle }}
          </h1>
          <div class="flex items-center space-x-4">
            <!-- Add any header actions here -->
            <slot name="actions" />
          </div>
        </div>
      </header>

      <!-- Page Content -->
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

const route = useRoute()
const router = useRouter()
const authStore = useAuthStore()
const isUserMenuOpen = ref(false)

// Computed properties
const userName = computed(() => {
  const firstName = authStore.user?.firstName || ''
  const lastName = authStore.user?.lastName || ''
  return `${firstName} ${lastName}`.trim()
})

const userRole = computed(() => authStore.user?.role || '')
const pageTitle = computed(() => route.meta.title || 'Dashboard')

const isAdmin = computed(() => userRole.value === 'admin')
const isSecretary = computed(() => userRole.value === 'secretary')
const canViewReports = computed(() => 
  ['admin', 'professor'].includes(userRole.value.toLowerCase())
)

// Methods
const logout = async () => {
  try {
    await authStore.logout()
    router.push('/login')
  } catch (error) {
    console.error('Logout failed:', error)
  }
}

// Close user menu when clicking outside
const closeUserMenu = (event: MouseEvent) => {
  const target = event.target as HTMLElement
  if (!target.closest('.user-menu')) {
    isUserMenuOpen.value = false
  }
}

// Lifecycle hooks
onMounted(() => {
  document.addEventListener('click', closeUserMenu)
})

onUnmounted(() => {
  document.removeEventListener('click', closeUserMenu)
})
</script>

<style scoped>
.material-icons {
  font-size: 20px;
}
</style>