<template>
  <div class="space-y-6">
    <!-- Header with Actions -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Group Details</h1>
      <div class="flex space-x-3">
        <Button v-if="isAdminOrSecretary" @click="openEditModal">Edit Group</Button>
        <Button variant="secondary" @click="$router.push('/dashboard/groups')">
          Back to List
        </Button>
      </div>
    </div>

    <!-- Loading and Error States -->
    <div v-if="isLoading">
      <Spinner :size="6" />
    </div>

    <div v-else-if="error" class="text-red-600">
      {{ error }}
    </div>

    <!-- Main Content -->
    <div v-else class="grid grid-cols-1 md:grid-cols-3 gap-6">
      <!-- Group Info Card -->
      <div class="col-span-1 bg-white shadow rounded-lg p-6">
        <h2 class="text-lg font-medium mb-4">Group Information</h2>
        <dl class="space-y-4">
          <div>
            <dt class="text-sm font-medium text-gray-500">Name</dt>
            <dd class="mt-1 text-sm text-gray-900">{{ group?.name }}</dd>
          </div>
          <div>
            <dt class="text-sm font-medium text-gray-500">Subject</dt>
            <dd class="mt-1 text-sm text-gray-900">{{ group?.subjectName }}</dd>
          </div>
          <div>
            <dt class="text-sm font-medium text-gray-500">Professor</dt>
            <dd class="mt-1 text-sm text-gray-900">{{ group?.professorName }}</dd>
          </div>
          <div>
            <dt class="text-sm font-medium text-gray-500">Status</dt>
            <dd class="mt-1">
              <Badge :status="group?.isActive ? 'success' : 'error'">
                {{ group?.isActive ? 'Active' : 'Inactive' }}
              </Badge>
            </dd>
          </div>
        </dl>
      </div>

      <!-- Stats and Students Section -->
      <div class="col-span-2 space-y-6">
        <!-- Stats Cards -->
        <div class="grid grid-cols-3 gap-4">
          <StatCard title="Total Students" :value="group?.totalStudents || 0" />
          <StatCard title="Average Attendance" :value="`${group?.averageAttendance || 0}%`" />
          <StatCard title="Total Classes" :value="group?.totalClasses || 0" />
        </div>

        <!-- Students List -->
        <div class="bg-white shadow rounded-lg p-6">
          <div class="flex justify-between items-center mb-4">
            <h2 class="text-lg font-medium">Enrolled Students</h2>
            <Button v-if="isAdminOrSecretary" variant="secondary" @click="openEnrollModal">
              Enroll Students
            </Button>
          </div>
          <DataTable :data="students" :columns="studentColumns" :loading="isLoadingStudents"
            :actions="studentActions" />
        </div>
      </div>
    </div>

    <!-- Modals -->
    <Modal v-model="showEditModal" title="Edit Group">
      <StudyGroupForm v-if="showEditModal" :group="group" :subjects="subjects" @submit="handleUpdateGroup"
        @cancel="showEditModal = false" />
    </Modal>

    <Modal v-model="showEnrollModal" title="Enroll Students">
      <EnrollStudentsForm v-if="showEnrollModal" :studyGroupId="Number(route.params.id)" @submit="handleEnrollStudents"
        @cancel="showEnrollModal = false" />
    </Modal>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { storeToRefs } from 'pinia'
import { useGroupStore } from '@/stores/studyGroup.store'
import { useSubjectStore } from '@/stores/subject.store'
import { useAuthStore } from '@/stores/auth.store'
import { useReportStore } from '@/stores/report.store'
import type { StudyGroupDto, GroupStudentDto } from '@/api/generated/data-contracts'
import type { TableItem } from '@/types/tableItem.types'
import type { Action } from '@/types/tableItem.types'

// UI Components
import Button from '@/shared/components/ui/Button.vue'
import Badge from '@/shared/components/ui/Badge.vue'
import Spinner from '@/shared/components/ui/Spinner.vue'
import StatCard from '@/shared/components/ui/StatCard.vue'
import DataTable from '@/shared/components/ui/DataTable.vue'
import Modal from '@/shared/components/ui/Modal.vue'
import StudyGroupForm from '../components/StudyGroupForm.vue'
import EnrollStudentsForm from '../components/EnrollStudentsForm.vue'

interface ExtendedGroupStats {
  averageAttendance: number
  totalClasses: number
  totalStudents: number
}

export interface StudyGroup extends StudyGroupDto, ExtendedGroupStats { }
export interface GroupStudent extends GroupStudentDto, TableItem {
  id: number
  fullName: string
  studentNumber: string 
  attendedClasses: number
  attendanceRate: number 
}

// Setup stores and route
const route = useRoute()
const groupStore = useGroupStore()
const subjectStore = useSubjectStore()
const authStore = useAuthStore()
const reportStore = useReportStore()

// Store refs
const { currentGroup: baseGroup, isLoading, error } = storeToRefs(groupStore)
const { subjects } = storeToRefs(subjectStore)

const group = computed<StudyGroup | null>(() => {
  if (!baseGroup.value) return null

  return {
    ...baseGroup.value,
    averageAttendance: 0,
    totalClasses: 0,
    totalStudents: students.value.length
  }
})

// Component state
const showEditModal = ref(false)
const showEnrollModal = ref(false)
const isLoadingStudents = ref(false)
const students = ref<GroupStudent[]>([])

// Computed properties
const isAdminOrSecretary = computed(() =>
  ['admin', 'secretary'].includes(authStore.userRole || '')
)
const studentColumns = [
  { key: 'studentNumber', label: 'Student ID' },
  { key: 'fullName', label: 'Full Name' },
  {
    key: 'attendanceRate',
    label: 'Attendance Rate',
    render: (value: number) => `${value}%`
  },
  {
    key: 'attendedClasses',
    label: 'Classes Attended'
  }
]

const studentActions = computed<Action<TableItem>[]>(() =>
  isAdminOrSecretary.value ? [
    {
      label: 'Remove',
      icon: 'remove_circle',
      action: (item: TableItem) => handleRemoveStudent(item)
    }
  ] : []
)

// Methods
async function loadGroupData() {
  const studyGroupId = Number(route.params.id)
  if (studyGroupId) {
    try {
      await Promise.all([
        groupStore.fetchGroupById(studyGroupId),
        loadStudents(studyGroupId),
        loadGroupStats(studyGroupId)
      ])
    } catch (err) {
      console.error('Failed to load group data:', err)
    }
  }
}

async function loadStudents(studyGroupId: number) {
  isLoadingStudents.value = true
  try {
    const result = await groupStore.fetchGroupStudents(studyGroupId)
    if (Array.isArray(result)) {
      students.value = result.map(student => ({
        ...student,
        id: student.studentId,
        fullName: student.studentName || '',
        studentNumber: student.studentNumber || '',
        attendedClasses: 0,
        attendanceRate: 0
      })) as GroupStudent[]
    }
  } catch (err) {
    console.error('Failed to load students:', err)
  } finally {
    isLoadingStudents.value = false
  }
}

async function loadGroupStats(studyGroupId: number) {
  try {
    const report = await reportStore.getGroupReport(studyGroupId)
    if (baseGroup.value && report) {
      const updatedGroup: StudyGroup = {
        ...baseGroup.value,
        averageAttendance: report.averageAttendance ?? 0,
        totalClasses: report.totalClasses ?? 0,
        totalStudents: students.value.length
      }
      baseGroup.value = updatedGroup

      // Fix nullable report.students check
      if (report.students && Array.isArray(report.students)) {
        students.value = students.value.map(student => {
          const studentStats = report.students?.find(s => s.studentId === student.studentId)
          return {
            ...student,
            attendedClasses: studentStats?.attendedClasses ?? 0,
            attendanceRate: studentStats?.attendanceRate ?? 0
          }
        })
      }
    }
  } catch (err) {
    console.error('Failed to load group stats:', err)
  }
}

async function handleUpdateGroup(groupData: Partial<StudyGroup>) {
  try {
    if (group.value?.id) {
      await groupStore.updateGroup(group.value.id, groupData)
      await loadGroupData()
      showEditModal.value = false
    }
  } catch (err) {
    console.error('Failed to update group:', err)
  }
}

async function handleEnrollStudents(studentIds: number[]) {
  try {
    if (!route.params.id || !studentIds.length) {
      throw new Error('Invalid parameters for enrollment')
    }
    
    await groupStore.enrollStudents(Number(route.params.id), studentIds)
    await loadStudents(Number(route.params.id))
    showEnrollModal.value = false
  } catch (err) {
    console.error('Failed to enroll students:', err)
    // Add user feedback here (e.g., toast notification)
  }
}

async function handleRemoveStudent(student: TableItem) {
  const groupStudent = student as GroupStudent
  const studyGroupId = Number(route.params.id)
  
  // Add checks for both studyGroupId and studentId
  if (!studyGroupId) {
    console.error('Invalid group ID')
    return
  }

  if (typeof groupStudent.studentId === 'undefined') {
    console.error('Invalid student ID')
    return
  }

  if (confirm('Are you sure you want to remove this student from the group?')) {
    try {
      await groupStore.removeStudent(studyGroupId, groupStudent.studentId)
      await loadStudents(studyGroupId)
    } catch (err) {
      console.error('Failed to remove student:', err)
    }
  }
}

function openEditModal() {
  showEditModal.value = true
}

function openEnrollModal() {
  showEnrollModal.value = true
}

// Lifecycle hooks
onMounted(() => {
  loadGroupData()
})
</script>