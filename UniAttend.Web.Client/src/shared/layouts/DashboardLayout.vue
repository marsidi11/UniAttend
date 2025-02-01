<template>
  <div class="min-h-screen bg-gray-100">
    <!-- Backdrop Overlay -->
    <div v-if="isSidebarOpen" class="fixed inset-0 bg-gray-900/50 backdrop-blur-sm lg:hidden z-30"
      @click="closeSidebar"></div>

    <!-- Mobile Menu Button -->
    <div class="lg:hidden fixed top-4 left-4 z-40">
      <button @click="toggleSidebar"
        class="p-2 rounded-md text-white bg-indigo-600 hover:bg-indigo-700 transition-colors">
        <span class="material-icons">{{ isSidebarOpen ? 'close' : 'menu' }}</span>
      </button>
    </div>

    <!-- Sidebar -->
    <aside
      class="fixed inset-y-0 left-0 z-40 w-64 bg-gradient-to-b from-indigo-700 to-indigo-800 transform transition-all duration-300 ease-in-out flex flex-col"
      :class="[isSidebarOpen ? 'translate-x-0 shadow-2xl' : '-translate-x-full lg:translate-x-0 lg:shadow-xl']">
      <!-- Logo Section -->
      <div class="flex items-center justify-center h-16 bg-indigo-800/50 border-b border-indigo-600/30">
        <router-link to="/dashboard"
          class="flex items-center space-x-3 px-4 py-2 rounded-lg hover:bg-indigo-600/30 transition-colors">
          <span class="material-icons text-white text-2xl">school</span>
          <span class="text-white text-xl font-bold">UniAttend</span>
        </router-link>
      </div>

      <!-- Navigation Section -->
      <div class="flex-1 flex flex-col min-h-0">
        <!-- Search Box -->
        <div class="p-4">
          <div class="relative">
            <span class="absolute inset-y-0 left-0 pl-3 flex items-center text-indigo-300">
              <span class="material-icons text-sm">search</span>
            </span>
            <input v-model="searchQuery" type="text" placeholder="Search menu..."
              class="w-full pl-10 pr-4 py-2 text-sm bg-indigo-600/30 border border-indigo-600/20 rounded-lg text-white placeholder-indigo-300 focus:outline-none focus:ring-2 focus:ring-indigo-500">
          </div>
        </div>

        <!-- Navigation Links -->
        <nav
          class="flex-1 px-3 space-y-1 overflow-y-auto scrollbar-thin scrollbar-thumb-indigo-600 scrollbar-track-transparent">
          <template v-for="(section, role) in filteredNavigation" :key="role.toString()">
            <template v-if="userHasAccess(role.toString())">
              <div class="pt-2 pb-1">
                <p class="px-3 text-xs font-semibold text-indigo-300 uppercase tracking-wider">
                  {{ role }}
                </p>
              </div>
              <router-link v-for="link in section" :key="link.to" :to="link.to"
                class="flex items-center px-3 py-2 text-sm text-indigo-100 rounded-lg transition-colors group" :class="[
                  route.path === link.to
                    ? 'bg-indigo-800 text-white'
                    : 'hover:bg-indigo-600/50'
                ]">
                <span class="material-icons mr-3 text-[20px]">{{ link.icon }}</span>
                <span class="flex-1">{{ link.text }}</span>
                <span v-if="hasBadge(link)" class="ml-auto inline-block py-0.5 px-2 text-xs rounded-full"
                  :class="link.badge.variant === 'success' ? 'bg-green-500/20 text-green-300' : 'bg-indigo-500/20 text-indigo-300'">
                  {{ link.badge.text }}
                </span>
              </router-link>
            </template>
          </template>
        </nav>
      </div>

      <!-- User Profile Section -->
      <div class="flex flex-col border-t border-indigo-600/30 bg-indigo-800/30 p-4">
        <div class="relative">
          <div class="flex items-center gap-3">
            <div class="flex-shrink-0">
              <div class="w-10 h-10 rounded-full bg-indigo-600 flex items-center justify-center">
                <span class="text-white font-medium">
                  {{ userInitials }}
                </span>
              </div>
            </div>
            <div class="flex-1 min-w-0">
              <p class="text-sm font-medium text-white truncate">{{ userName }}</p>
              <p class="text-xs text-indigo-300 capitalize">{{ userRole }}</p>
            </div>
            <div class="relative">
              <button @click.stop="toggleUserMenu"
                class="p-2 text-indigo-300 rounded-lg hover:bg-indigo-600/50 hover:text-white transition-colors">
                <span class="material-icons">more_vert</span>
              </button>

              <div v-show="isUserMenuOpen"
                class="absolute right-0 bottom-full mb-2 w-56 rounded-lg shadow-lg bg-white ring-1 ring-black ring-opacity-5 z-50 overflow-hidden">
                <div class="py-1">
                  <router-link to="/dashboard/profile"
                    class="group flex items-center px-4 py-2 text-sm text-gray-700 hover:bg-indigo-50"
                    @click="closeUserMenu">
                    <span class="material-icons mr-3 text-gray-400 group-hover:text-indigo-500">person</span>
                    Profile Settings
                  </router-link>
                  <button @click="handleLogout"
                    class="w-full group flex items-center px-4 py-2 text-sm text-gray-700 hover:bg-indigo-50">
                    <span class="material-icons mr-3 text-gray-400 group-hover:text-indigo-500">logout</span>
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
    <div 
    class="lg:pl-64 min-h-screen flex flex-col transition-all duration-300"
    :class="{ 'blur-sm': isSidebarOpen && windowWidth < 1024 }"
  >
  <header class="bg-white shadow-sm sticky top-0 z-20">
      <div class="max-w-7xl mx-auto relative">
        <!-- Mobile menu button moved inside header -->
        <div class="absolute left-4 top-1/2 -translate-y-1/2 lg:hidden z-30">
          <button 
            @click="toggleSidebar"
            class="p-2 rounded-md text-gray-500 hover:bg-gray-100 transition-colors"
          >
            <span class="material-icons">{{ isSidebarOpen ? 'close' : 'menu' }}</span>
          </button>
        </div>

        <div class="px-4 sm:px-6 lg:px-8 py-4">
          <div class="flex items-center justify-between gap-4">
            <!-- Title with proper mobile padding -->
            <div class="flex-1 min-w-0 lg:ml-0 ml-12">
              <h1 class="text-xl font-semibold text-gray-900 truncate text-center lg:text-left">
                {{ pageTitle }}
              </h1>
            </div>
            
            <!-- Actions Section -->
            <div class="flex items-center shrink-0 space-x-4">
              <slot name="header-actions" />
            </div>
          </div>
        </div>
        
        <slot name="header-content" />
      </div>
    </header>
      <main class="flex-1 p-6">
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

const searchQuery = ref('')
const filteredNavigation = computed(() => {
  const query = searchQuery.value.toLowerCase()
  const result: NavigationSections = {}

  Object.entries(navigationSections).forEach(([role, links]) => {
    if (userHasAccess(role)) {
      const filteredLinks = links.filter(link =>
        link.text.toLowerCase().includes(query) ||
        (link.badge?.text.toLowerCase() || '').includes(query)
      )
      if (filteredLinks.length > 0) {
        result[role] = filteredLinks
      }
    }
  })

  return result
})

declare const window: Window
const windowWidth = ref(window.innerWidth)

const updateWindowWidth = () => {
  windowWidth.value = window.innerWidth
}

interface NavigationBadge {
  text: string;
  variant: 'success' | 'warning' | 'error' | 'info';
}

interface NavigationLink {
  to: string;
  icon: string;
  text: string;
  badge?: NavigationBadge;
}

interface NavigationSections {
  [key: string]: NavigationLink[];
}

// Navigation Sections
const navigationSections: NavigationSections = {
  Admin: [
    { to: '/dashboard/admin', icon: 'dashboard', text: 'Dashboard' },
    { to: '/dashboard/departments', icon: 'business', text: 'Departments' },
    { to: '/dashboard/users', icon: 'people', text: 'Users', badge: { text: 'New', variant: 'success' } },
    { to: '/dashboard/academic-years', icon: 'event', text: 'Academic Years' },
    { to: '/dashboard/subjects', icon: 'book', text: 'Subjects' },
    { to: '/dashboard/reports', icon: 'analytics', text: 'Reports' }
  ],
  Secretary: [
    { to: '/dashboard/secretary', icon: 'dashboard', text: 'Dashboard' },
    { to: '/dashboard/students', icon: 'school', text: 'Students' },
    { to: '/dashboard/schedule', icon: 'schedule', text: 'Schedule' },
    { to: '/dashboard/groups', icon: 'groups', text: 'Study Groups' },
    { to: '/dashboard/subjects', icon: 'book', text: 'Subjects' },
    { to: '/dashboard/classrooms', icon: 'meeting_room', text: 'Classrooms' },
    { to: '/dashboard/attendance/records', icon: 'fact_check', text: 'Attendance' },
    { to: '/dashboard/reports', icon: 'analytics', text: 'Reports' }
  ],
  Professor: [
    { to: '/dashboard/professor', icon: 'dashboard', text: 'Dashboard' },
    { to: '/dashboard/schedule', icon: 'schedule', text: 'My Schedule' },
    { to: '/dashboard/groups', icon: 'groups', text: 'My Groups' },
    { to: '/dashboard/reports', icon: 'analytics', text: 'Reports' }
  ],
  Student: [
    { to: '/dashboard/student', icon: 'dashboard', text: 'Dashboard' },
    { to: '/dashboard/schedule', icon: 'schedule', text: 'My Schedule' },
    { to: '/dashboard/attendance/view', icon: 'schedule', text: 'My Attendance' },
  ]
}

const isClient = typeof window !== 'undefined'

// Computed Properties
const userName = computed(() => {
  const firstName = authStore.user?.firstName || ''
  const lastName = authStore.user?.lastName || ''
  return `${firstName} ${lastName}`.trim()
})

const userInitials = computed(() => {
  const firstName = authStore.user?.firstName || ''
  const lastName = authStore.user?.lastName || ''
  return `${firstName.charAt(0)}${lastName.charAt(0)}`.toUpperCase()
})

const userRole = computed(() => authStore.user?.role || '')
const pageTitle = computed(() => route.meta.title || 'Dashboard')

// Role checking
const userHasAccess = (role: string): boolean => {
  const currentRole = userRole.value.toLowerCase()
  return role.toLowerCase() === currentRole
}

// Methods
const toggleSidebar = () => {
  isSidebarOpen.value = !isSidebarOpen.value
  updateBodyClass()
}

const closeSidebar = () => {
  isSidebarOpen.value = false
  updateBodyClass()
}

const toggleUserMenu = () => {
  isUserMenuOpen.value = !isUserMenuOpen.value
}

const closeUserMenu = () => {
  isUserMenuOpen.value = false
}

const updateBodyClass = () => {
  if (isClient) {
    if (isSidebarOpen.value && window.innerWidth < 1024) {
      document.body.classList.add('overflow-hidden')
    } else {
      document.body.classList.remove('overflow-hidden')
    }
  }
}

const hasBadge = (link: NavigationLink): link is NavigationLink & { badge: NavigationBadge } => {
  return 'badge' in link;
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
    closeUserMenu()
  }
}

// Lifecycle Hooks
onMounted(() => {
  if (typeof window !== 'undefined') {
    window.addEventListener('resize', updateWindowWidth)
    document.addEventListener('click', handleClickOutside)
  }
})

onUnmounted(() => {
  if (typeof window !== 'undefined') {
    window.removeEventListener('resize', updateWindowWidth)
    document.removeEventListener('click', handleClickOutside)
    document.body.classList.remove('overflow-hidden')
  }
})
</script>

<style scoped>
.scrollbar-thin::-webkit-scrollbar {
  width: 4px;
}

.scrollbar-thin::-webkit-scrollbar-track {
  background: transparent;
}

.scrollbar-thin::-webkit-scrollbar-thumb {
  background-color: rgba(255, 255, 255, 0.2);
  border-radius: 20px;
}

.scrollbar-thin {
  scrollbar-width: thin;
  scrollbar-color: rgba(255, 255, 255, 0.2) transparent;
}

@media (max-width: 1024px) {
  .overflow-hidden {
    overflow: hidden;
  }
}

header {
  @apply border-b border-gray-200;
  background: linear-gradient(to bottom, white, rgba(255, 255, 255, 0.98));
  backdrop-filter: blur(8px);
}

.header-shadow {
  box-shadow: 0 1px 3px 0 rgb(0 0 0 / 0.1), 0 1px 2px -1px rgb(0 0 0 / 0.1);
}

/* Optional: Add scroll shadow effect */
header::after {
  content: '';
  @apply absolute left-0 right-0 bottom-0 h-px bg-gradient-to-r from-transparent via-gray-200 to-transparent opacity-0 transition-opacity duration-200;
}

header.scrolled::after {
  @apply opacity-100;
}
</style>