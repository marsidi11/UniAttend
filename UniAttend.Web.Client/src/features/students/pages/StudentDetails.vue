<template>
  <div class="space-y-6">
    <!-- Header Section -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Student Details</h1>
      <div class="flex space-x-3">
        <Button @click="openEditModal">Edit Student</Button>
        <Button variant="secondary" @click="$router.push('/dashboard/students')">
          Back to List
        </Button>
      </div>
    </div>

    <div v-if="isLoading">
      <Spinner :size="6" />
    </div>

    <div v-else-if="error" class="text-red-600">
      {{ error }}
    </div>

    <div v-else class="grid grid-cols-1 md:grid-cols-3 gap-6">
      <!-- Student Info Card -->
      <div class="col-span-1 bg-white shadow rounded-lg p-6">
        <h2 class="text-lg font-medium mb-4">Student Information</h2>
        <dl class="space-y-4">
          <div>
            <dt class="text-sm font-medium text-gray-500">Student ID</dt>
            <dd class="mt-1 text-sm text-gray-900">{{ student?.studentId }}</dd>
          </div>
          <div>
            <dt class="text-sm font-medium text-gray-500">Full Name</dt>
            <dd class="mt-1 text-sm text-gray-900">
              {{ student?.firstName }} {{ student?.lastName }}
            </dd>
          </div>
          <div>
            <dt class="text-sm font-medium text-gray-500">Email</dt>
            <dd class="mt-1 text-sm text-gray-900">{{ student?.email }}</dd>
          </div>
          <div>
            <dt class="text-sm font-medium text-gray-500">Department</dt>
            <dd class="mt-1 text-sm text-gray-900">{{ student?.departmentName }}</dd>
          </div>
          <div>
            <dt class="text-sm font-medium text-gray-500">Card ID</dt>
            <dd class="mt-1 text-sm text-gray-900">{{ student?.cardId || 'Not Assigned' }}</dd>
          </div>
          <div>
            <dt class="text-sm font-medium text-gray-500">Status</dt>
            <dd class="mt-1">
              <Badge :status="student?.isActive ? 'success' : 'error'">
                {{ student?.isActive ? 'Active' : 'Inactive' }}
              </Badge>
            </dd>
          </div>
        </dl>
      </div>

      <!-- Attendance Stats -->
      <div class="col-span-2 space-y-6">
        <div class="bg-white shadow rounded-lg p-6">
          <h2 class="text-lg font-medium mb-4">Attendance Overview</h2>
          <div class="grid grid-cols-3 gap-4">
            <StatCard title="Total courseSessions" :value="formattedStats.totalCourseSessions" />
            <StatCard title="courseSessions Attended" :value="formattedStats.attendedCourseSessions" />
            <StatCard title="Absence Rate" :value="`${formattedStats.absenceRate}%`" />
          </div>
        </div>

        <!-- Enrolled Groups -->
        <div class="bg-white shadow rounded-lg p-6">
          <h2 class="text-lg font-medium mb-4">Enrolled Groups</h2>
          <DataTable :data="groups" :columns="groupColumns" :loading="isLoadingGroups" />
        </div>

        <!-- Recent Attendance -->
        <div class="bg-white shadow rounded-lg p-6">
          <h2 class="text-lg font-medium mb-4">Recent Attendance</h2>
          <AttendanceList :records="attendance" :loading="isLoadingAttendance" />
        </div>
      </div>
    </div>

    <!-- Edit Student Modal -->
    <Modal v-model="showEditModal" title="Edit Student">
      <StudentForm v-if="showEditModal" :student="student" :departments="departments" @submit="handleUpdateStudent"
        @cancel="showEditModal = false" />
    </Modal>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { storeToRefs } from 'pinia'
import { useStudentStore } from '@/stores/student.store'
import { useDepartmentStore } from '@/stores/department.store'
import { useUserStore } from '@/stores/user.store'
import { useReportStore } from '@/stores/report.store'
import type {
  UserDetailsDto,
  UpdateUserCommand,
  AttendanceRecordDto,
  UserGroupDto,
  AttendanceStatsDto
} from '@/api/generated/data-contracts'

interface ExtendedUserDetailsDto extends UserDetailsDto {
  studentId?: string
  cardId?: string | null
}

interface ExtendedUserGroupDto extends UserGroupDto {
  id: number
  attendanceRate: number
}

// UI Components
import Button from '@/shared/components/ui/Button.vue'
import Badge from '@/shared/components/ui/Badge.vue'
import Spinner from '@/shared/components/ui/Spinner.vue'
import StatCard from '@/shared/components/ui/StatCard.vue'
import DataTable from '@/shared/components/ui/DataTable.vue'
import Modal from '@/shared/components/ui/Modal.vue'
import StudentForm from '../components/StudentForm.vue'
import AttendanceList from '@/features/attendance/components/AttendanceList.vue'

// Column type for DataTable
interface Column {
  key: string
  label: string
  render?: (value: any) => string
}

// Store setup
const route = useRoute()
const studentStore = useStudentStore()
const departmentStore = useDepartmentStore()
const userStore = useUserStore()
const reportStore = useReportStore()

// Store refs
const { departments } = storeToRefs(departmentStore)
const { isLoading, error } = storeToRefs(studentStore)

// Component state
const showEditModal = ref(false)
const isLoadingGroups = ref(false)
const isLoadingAttendance = ref(false)
const student = ref<ExtendedUserDetailsDto | null>(null)
const groups = ref<ExtendedUserGroupDto[]>([])
const attendance = ref<AttendanceRecordDto[]>([])
const stats = ref<AttendanceStatsDto>({
  totalCourseSessions: 0,
  attendedCourseSessions: 0,
  attendanceRate: 0
})

// Computed properties
const formattedStats = computed(() => ({
  totalCourseSessions: stats.value.totalCourseSessions || 0,
  attendedCourseSessions: stats.value.attendedCourseSessions || 0,
  absenceRate: 100 - (stats.value.attendanceRate || 0)
}))

// Table columns
const groupColumns: Column[] = [
  { key: 'groupName', label: 'Group' },
  { key: 'subjectName', label: 'Subject' },
  { 
    key: 'attendanceRate',
    label: 'Attendance Rate',
    render: (value: number) => `${value}%`
  }
]

// Methods remain the same but with proper type annotations
async function loadStudentData(studentId: number) {
  try {
    const [userData, reportData] = await Promise.all([
      userStore.fetchUserDetails(studentId),
      reportStore.getStudentReport(studentId)
    ])

    if (userData) {
      student.value = {
        ...userData,
        studentId: userData.username,
        cardId: null // Initialize with null since it's not in UserDetailsDto
      } as ExtendedUserDetailsDto

      if (userData.groups) {
        groups.value = userData.groups.map(g => ({
          ...g,
          id: g.studyGroupId || 0,
          studyGroupId: g.studyGroupId || 0,
          groupName: g.groupName || '',
          subjectName: g.subjectName || '',
          attendanceRate: 0
        })) as ExtendedUserGroupDto[]
      }
    }

    if (reportData) {
      stats.value = {
        totalCourseSessions: reportData.totalCourseSessions || 0,
        attendedCourseSessions: reportData.totalAttendance || 0,
        attendanceRate: reportData.attendanceRate || 0
      }
    }

    await loadStudentAttendance(studentId)
  } catch (err) {
    console.error('Failed to load student data:', err)
  }
}

async function loadStudentAttendance(studentId: number) {
  isLoadingAttendance.value = true
  try {
    const records = await studentStore.fetchStudentAttendance(studentId)
    attendance.value = records ?? [] 
  } catch (err) {
    console.error('Failed to load attendance records:', err)
    attendance.value = []
  } finally {
    isLoadingAttendance.value = false
  }
}

async function handleUpdateStudent(updatedStudent: Partial<UpdateUserCommand>) {
  try {
    if (student.value?.id) {
      await userStore.updateUser(student.value.id, {
        ...updatedStudent,
        id: student.value.id
      })
      await loadStudentData(student.value.id)
      showEditModal.value = false
    }
  } catch (err) {
    console.error('Failed to update student:', err)
  }
}

function openEditModal() {
  showEditModal.value = true
}

// Lifecycle hooks
onMounted(async () => {
  const studentId = Number(route.params.id)
  if (studentId) {
    await loadStudentData(studentId)
  }
})
</script>