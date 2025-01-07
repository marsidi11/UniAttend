<template>
  <div class="space-y-6">
    <!-- Header -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Group Details</h1>
      <div class="flex space-x-3">
        <Button v-if="isAdmin" @click="openEditModal">Edit Group</Button>
        <Button variant="secondary" @click="$router.push('/dashboard/groups')">
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
      <!-- Group Info -->
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

      <!-- Stats and Lists -->
      <div class="col-span-2 space-y-6">
        <!-- Stats -->
        <div class="grid grid-cols-3 gap-4">
          <StatCard title="Total Students" :value="group?.studentsCount || 0" />
          <StatCard title="Average Attendance" :value="`${group?.averageAttendance || 0}%`" />
          <StatCard title="Classes Held" :value="group?.classesCount || 0" />
        </div>

        <!-- Students List -->
        <div class="bg-white shadow rounded-lg p-6">
          <div class="flex justify-between items-center mb-4">
            <h2 class="text-lg font-medium">Enrolled Students</h2>
            <Button
              v-if="isAdmin"
              variant="secondary"
              @click="openEnrollModal"
            >
              Enroll Students
            </Button>
          </div>
          <DataTable
            :data="students"
            :columns="studentColumns"
            :loading="isLoadingStudents"
            :actions="studentActions"
          />
        </div>
      </div>
    </div>

    <!-- Edit Group Modal -->
    <Modal v-model="showEditModal" title="Edit Group">
      <StudyGroupForm
        v-if="showEditModal"
        :group="group"
        :subjects="subjects"
        @submit="handleUpdateGroup"
        @cancel="showEditModal = false"
      />
    </Modal>

    <!-- Enroll Students Modal -->
    <Modal v-model="showEnrollModal" title="Enroll Students">
      <EnrollStudentsForm
        v-if="showEnrollModal"
        :groupId="Number(route.params.id)"
        @submit="handleEnrollStudents"
        @cancel="showEnrollModal = false"
      />
    </Modal>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRoute } from 'vue-router' 
import { storeToRefs } from 'pinia'
import { useGroupStore } from '@/stores/group.store'
import { useSubjectStore } from '@/stores/subject.store'
import { useAuthStore } from '@/stores/auth.store'
import type { StudyGroup, GroupStudent } from '@/types/group.types'
import type { TableItem } from '@/types/tableItem.types' 
import type { Action } from '@/types/tableItem.types'
import Button from '@/shared/components/ui/Button.vue'
import Badge from '@/shared/components/ui/Badge.vue'
import Spinner from '@/shared/components/ui/Spinner.vue'
import StatCard from '@/shared/components/ui/StatCard.vue'
import DataTable from '@/shared/components/ui/DataTable.vue'
import Modal from '@/shared/components/ui/Modal.vue'
import StudyGroupForm from '../components/StudyGroupForm.vue'
import EnrollStudentsForm from '../components/EnrollStudentsForm.vue'

const route = useRoute()
const groupStore = useGroupStore()
const subjectStore = useSubjectStore()
const authStore = useAuthStore()

const { currentGroup: group, isLoading, error } = storeToRefs(groupStore)
const { subjects } = storeToRefs(subjectStore)

const showEditModal = ref(false)
const showEnrollModal = ref(false)
const isLoadingStudents = ref(false)
const students = ref<GroupStudent[]>([])

const isAdmin = computed(() => authStore.userRole === 'admin')

const studentColumns = [
  { key: 'studentId', label: 'Student ID' },
  { key: 'fullName', label: 'Full Name' },
  { key: 'attendanceRate', label: 'Attendance Rate',
    render: (value: number) => `${value}%`
  }
]

const studentActions = computed<Action<TableItem>[]>(() => isAdmin.value ? [
  {
    label: 'Remove',
    icon: 'remove_circle',
    action: (item: TableItem) => handleRemoveStudent(item as GroupStudent)
  }
] : [])

async function loadGroupData() {
  const groupId = Number(route.params.id)
  if (groupId) {
    const [_, loadedStudents] = await Promise.all([
      groupStore.fetchGroupById(groupId),
      loadStudents(groupId)
    ])
    students.value = loadedStudents
  }
}

async function loadStudents(groupId: number): Promise<GroupStudent[]> {
  isLoadingStudents.value = true
  try {
    const result = await groupStore.fetchGroupStudents(groupId)
    if (Array.isArray(result)) {
      students.value = result
      return result
    }
    return []
  } catch (err) {
    console.error('Failed to load students:', err)
    return []
  } finally {
    isLoadingStudents.value = false
  }
}

async function handleUpdateGroup(groupData: Partial<StudyGroup>) {
  try {
    if (group.value?.id) {
      await groupStore.updateGroup(group.value.id, groupData)
      showEditModal.value = false
    }
  } catch (err) {
    console.error('Failed to update group:', err)
  }
}

async function handleEnrollStudents(studentIds: number[]) {
  try {
    await groupStore.enrollStudents(Number(route.params.id), studentIds)
    showEnrollModal.value = false
    await loadStudents(Number(route.params.id))
  } catch (err) {
    console.error('Failed to enroll students:', err)
  }
}

async function handleRemoveStudent(student: GroupStudent) {
  if (confirm('Are you sure you want to remove this student from the group?')) {
    try {
      await groupStore.removeStudentFromGroup(Number(route.params.id), student.id)
      await loadStudents(Number(route.params.id))
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

onMounted(() => {
  loadGroupData()
})
</script>