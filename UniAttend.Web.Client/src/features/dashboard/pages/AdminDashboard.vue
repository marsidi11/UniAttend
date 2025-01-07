<template>
  <div class="space-y-6">
    <!-- Header -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">System Administration</h1>
    </div>

    <!-- Key System Stats -->
    <div class="grid grid-cols-3 gap-4">
      <StatCard
        title="Academic Year Status"
        :value="stats.currentYear"
        :status="stats.isAcademicYearActive ? 'success' : 'warning'"
        :subtitle="stats.isAcademicYearActive ? 'Active' : 'Not Set'"
      />
      <StatCard
        title="Departments"
        :value="stats.activeDepartments"
        :total="stats.totalDepartments"
        subtitle="Active/Total"
      />
      <StatCard
        title="Staff Members"
        :value="stats.activeStaff"
        :subtitle="`${stats.secretaryCount} Secretaries, ${stats.professorCount} Professors`"
      />
    </div>

    <!-- Main Management Sections -->
    <div class="grid grid-cols-2 gap-6">
      <!-- Department Management -->
      <div class="bg-white p-6 rounded-lg shadow">
        <div class="flex justify-between items-center mb-4">
          <h2 class="text-lg font-medium">Department Management</h2>
          <Button @click="router.push('/dashboard/departments')" variant="primary">
            Manage Departments
          </Button>
        </div>
        <div class="space-y-2">
          <div 
            v-for="dept in departments" 
            :key="dept.id" 
            class="flex items-center justify-between p-3 bg-gray-50 rounded-lg"
          >
            <div>
              <span class="font-medium">{{ dept.name }}</span>
            </div>
            <div class="flex items-center space-x-2">
              <Badge :status="dept.isActive ? 'success' : 'error'">
                {{ dept.isActive ? 'Active' : 'Inactive' }}
              </Badge>
              <Button 
                @click="handleToggleDepartment(dept)"
                variant="secondary"
                size="sm"
              >
                {{ dept.isActive ? 'Deactivate' : 'Activate' }}
              </Button>
            </div>
          </div>
        </div>
      </div>

      <!-- Academic Year Management -->
      <div class="bg-white p-6 rounded-lg shadow">
        <div class="flex justify-between items-center mb-4">
          <h2 class="text-lg font-medium">Academic Year</h2>
          <Button 
            @click="handleOpenAcademicYear" 
            variant="primary"
            :disabled="stats.isAcademicYearActive"
          >
            Set Academic Year
          </Button>
        </div>
        <div v-if="currentAcademicYear" class="p-4 bg-gray-50 rounded-lg">
          <div class="flex justify-between items-center">
            <div>
              <h3 class="font-medium">{{ currentAcademicYear.name }}</h3>
              <p class="text-sm text-gray-600">
                {{ formatDate(currentAcademicYear.startDate) }} - 
                {{ formatDate(currentAcademicYear.endDate) }}
              </p>
            </div>
            <Button 
              v-if="currentAcademicYear.isActive"
              @click="handleCloseAcademicYear" 
              variant="danger"
              size="sm"
            >
              Close Year
            </Button>
          </div>
        </div>
      </div>

      <!-- Staff Management -->
      <div class="bg-white p-6 rounded-lg shadow">
        <div class="flex justify-between items-center mb-4">
          <h2 class="text-lg font-medium">Staff Management</h2>
          <Button @click="router.push('/dashboard/users')" variant="primary">
            Manage Staff
          </Button>
        </div>
        <div class="space-y-4">
          <div v-for="staff in recentStaffMembers" :key="staff.id" class="p-3 bg-gray-50 rounded-lg">
            <div class="flex justify-between items-center">
              <div>
                <p class="font-medium">{{ staff.firstName }} {{ staff.lastName }}</p>
                <p class="text-sm text-gray-600">{{ staff.role }}</p>
              </div>
              <Badge :status="staff.isActive ? 'success' : 'error'">
                {{ staff.isActive ? 'Active' : 'Inactive' }}
              </Badge>
            </div>
          </div>
        </div>
      </div>

      <!-- System Reports -->
      <div class="bg-white p-6 rounded-lg shadow">
        <div class="flex justify-between items-center mb-4">
          <h2 class="text-lg font-medium">System Reports</h2>
        </div>
        <div class="grid grid-cols-2 gap-4">
          <div 
            v-for="report in systemReports" 
            :key="report.id"
            @click="handleGenerateReport(report)"
            class="p-4 bg-gray-50 rounded-lg cursor-pointer hover:bg-gray-100"
          >
            <h3 class="font-medium">{{ report.name }}</h3>
            <p class="text-sm text-gray-600">{{ report.description }}</p>
          </div>
        </div>
      </div>
    </div>

    <!-- Academic Year Modal -->
    <Modal v-model="showAcademicYearModal" title="Set Academic Year">
      <AcademicYearForm
        v-if="showAcademicYearModal"
        @submit="handleAcademicYearSubmit"
        @cancel="showAcademicYearModal = false"
      />
    </Modal>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { storeToRefs } from 'pinia'
import { useDepartmentStore } from '@/stores/department.store'
import { useAcademicYearStore } from '@/stores/academicYear.store'
import { useUserStore } from '@/stores/user.store'
import { formatDate } from '@/utils/dateUtils'

// Types
import type { Department } from '@/types/department.types'
import type { AcademicYear, CreateAcademicYearRequest } from '@/types/academicYear.types'

// Components
import StatCard from '@/shared/components/ui/StatCard.vue'
import Button from '@/shared/components/ui/Button.vue'
import Badge from '@/shared/components/ui/Badge.vue'
import Modal from '@/shared/components/ui/Modal.vue'
import AcademicYearForm from '@/features/academic/components/AcademicYearForm.vue'

// Store setup
const router = useRouter()
const departmentStore = useDepartmentStore()
const academicYearStore = useAcademicYearStore()
const userStore = useUserStore()

// Store refs
const { departments } = storeToRefs(departmentStore)
const { currentYear: currentAcademicYear } = storeToRefs(academicYearStore)
const { users } = storeToRefs(userStore)

// Local state
const showAcademicYearModal = ref(false)
const stats = ref({
  currentYear: '',
  isAcademicYearActive: false,
  activeDepartments: 0,
  totalDepartments: 0,
  activeStaff: 0,
  secretaryCount: 0,
  professorCount: 0
})

// Computed properties
const recentStaffMembers = computed(() => 
  users.value.slice(0, 5)
)

const systemReports = computed(() => [
  { id: 'attendance', name: 'Attendance Report', description: 'Overall attendance statistics' },
  { id: 'academic', name: 'Academic Report', description: 'Academic performance overview' }
])

// Methods
async function loadDashboardData() {
  try {
    await Promise.all([
      departmentStore.fetchDepartments(),
      academicYearStore.fetchCurrentAcademicYear(), // Changed from getCurrentYear
      userStore.fetchUsers()
    ])
    updateDashboardStats()
  } catch (error) {
    console.error('Failed to load dashboard data:', error)
  }
}

function updateDashboardStats() {
  if (!departments.value || !currentAcademicYear.value) return

  const activeDepts = departments.value.filter(d => d.isActive)
  const staffMembers = users.value.filter(u => u.role.toLowerCase() !== 'student')
  
  stats.value = {
    currentYear: currentAcademicYear.value?.name || 'Not Set',
    isAcademicYearActive: currentAcademicYear.value?.isActive ?? false,
    activeDepartments: activeDepts.length,
    totalDepartments: departments.value.length,
    activeStaff: staffMembers.filter(s => s.isActive).length,
    secretaryCount: staffMembers.filter(s => s.role.toLowerCase() === 'secretary').length,
    professorCount: staffMembers.filter(s => s.role.toLowerCase() === 'professor').length
  }
}

async function handleToggleDepartment(department: Department) {
  try {
    await departmentStore.updateDepartment(department.id, {
      ...department,
      isActive: !department.isActive
    })
    await loadDashboardData()
  } catch (error) {
    console.error('Failed to toggle department status:', error)
  }
}

function handleOpenAcademicYear() {
  showAcademicYearModal.value = true
}

async function handleAcademicYearSubmit(data: Partial<AcademicYear>) {
  try {
    const createRequest: CreateAcademicYearRequest = {
      name: data.name || '',
      startDate: data.startDate || new Date(),
      endDate: data.endDate || new Date(),
      isActive: data.isActive
    }
    await academicYearStore.createAcademicYear(createRequest)
    showAcademicYearModal.value = false
    await loadDashboardData()
  } catch (error) {
    console.error('Failed to create academic year:', error)
  }
}

async function handleCloseAcademicYear() {
  if (!currentAcademicYear.value) return
  
  try {
    await academicYearStore.updateAcademicYear(currentAcademicYear.value.id, {
      ...currentAcademicYear.value,
      isActive: false
    })
    await loadDashboardData()
  } catch (error) {
    console.error('Failed to close academic year:', error)
  }
}

async function handleGenerateReport(report: { id: string; name: string }) {
  try {
    // Implementation will depend on your reporting system
    console.log(`Generating report: ${report.name}`)
  } catch (error) {
    console.error(`Failed to generate ${report.name}:`, error)
  }
}

// Lifecycle hooks
onMounted(() => {
  loadDashboardData()
})
</script>