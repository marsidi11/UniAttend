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
            <StatCard 
              title="Total Classes" 
              :value="stats.totalClasses"
            />
            <StatCard 
              title="Classes Attended" 
              :value="stats.attendedClasses"
            />
            <StatCard 
              title="Absence Rate" 
              :value="`${stats.absenceRate}%`"
            />
          </div>
        </div>

        <!-- Enrolled Groups -->
        <div class="bg-white shadow rounded-lg p-6">
          <h2 class="text-lg font-medium mb-4">Enrolled Groups</h2>
          <DataTable
            :data="groups"
            :columns="groupColumns"
            :loading="isLoadingGroups"
          />
        </div>

        <!-- Recent Attendance -->
        <div class="bg-white shadow rounded-lg p-6">
          <h2 class="text-lg font-medium mb-4">Recent Attendance</h2>
          <AttendanceList 
            :records="attendance"
            :loading="isLoadingAttendance"
          />
        </div>
      </div>
    </div>

    <!-- Edit Student Modal -->
    <Modal v-model="showEditModal" title="Edit Student">
      <StudentForm
        v-if="showEditModal"
        :student="student"
        :departments="departments"
        @submit="handleUpdateStudent"
        @cancel="showEditModal = false"
      />
    </Modal>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { storeToRefs } from 'pinia'
import { useStudentStore } from '@/stores/student.store'
import { useDepartmentStore } from '@/stores/department.store'
import type { Student } from '@/types/student.types'
import type { AttendanceRecord } from '@/types/attendance.types'
import type { StudentGroupDetails } from '@/types/student.types'
import type { TableItem } from '@/types/tableItem.types'
import Button from '@/shared/components/ui/Button.vue'
import Badge from '@/shared/components/ui/Badge.vue'
import Spinner from '@/shared/components/ui/Spinner.vue'
import StatCard from '@/shared/components/ui/StatCard.vue'
import DataTable from '@/shared/components/ui/DataTable.vue'
import Modal from '@/shared/components/ui/Modal.vue'
import StudentForm from '../components/StudentForm.vue'
import AttendanceList from '@/features/attendance/components/AttendanceList.vue'

const route = useRoute()
const studentStore = useStudentStore()
const departmentStore = useDepartmentStore()

// Store refs
const { currentStudent: student, isLoading, error } = storeToRefs(studentStore)
const { departments } = storeToRefs(departmentStore)

// Component state
const showEditModal = ref(false)
const isLoadingGroups = ref(false)
const isLoadingAttendance = ref(false)
const groups = ref<(StudentGroupDetails & TableItem)[]>([])
const attendance = ref<AttendanceRecord[]>([])
const stats = ref({
  totalClasses: 0,
  attendedClasses: 0,
  absenceRate: 0
})

// Table columns
const groupColumns = [
  { key: 'groupName', label: 'Group' },
  { key: 'subjectName', label: 'Subject' },
  { key: 'professorName', label: 'Professor' },
  { key: 'attendanceRate', label: 'Attendance Rate',
    render: (value: number) => `${value}%`
  }
]

// Lifecycle hooks
onMounted(async () => {
  const studentId = Number(route.params.id)
  if (studentId) {
    await loadStudentData(studentId)
  }
})

// Methods
async function loadStudentData(studentId: number) {
  try {
    await Promise.all([
      studentStore.fetchStudentById(studentId),
      loadStudentGroups(studentId),
      loadStudentStats(studentId),
      loadStudentAttendance(studentId)
    ])
  } catch (err) {
    console.error('Failed to load student data:', err)
  }
}

async function loadStudentGroups(studentId: number) {
  isLoadingGroups.value = true
  try {
    const data = await studentStore.fetchStudentGroups(studentId)
    groups.value = data.map((group: StudentGroupDetails) => ({
      ...group,
      id: group.groupId
    }))
  } catch (err) {
    console.error('Failed to load student groups:', err)
  } finally {
    isLoadingGroups.value = false
  }
}

async function loadStudentStats(studentId: number) {
  try {
    const data = await studentStore.getAttendanceStats(studentId)
    stats.value = data
  } catch (err) {
    console.error('Failed to load attendance stats:', err)
  }
}

async function loadStudentAttendance(studentId: number) {
  isLoadingAttendance.value = true
  try {
    const data = await studentStore.fetchStudentAttendance(studentId)
    attendance.value = data
  } catch (err) {
    console.error('Failed to load attendance records:', err)
  } finally {
    isLoadingAttendance.value = false
  }
}

async function handleUpdateStudent(updatedStudent: Partial<Student>) {
  try {
    if (student.value?.id) {
      await studentStore.updateStudent(student.value.id, updatedStudent)
      showEditModal.value = false
    }
  } catch (err) {
    console.error('Failed to update student:', err)
  }
}

function openEditModal() {
  showEditModal.value = true
}
</script>