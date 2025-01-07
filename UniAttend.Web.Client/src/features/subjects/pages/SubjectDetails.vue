<template>
  <div class="space-y-6">
    <!-- Header Section -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Subject Details</h1>
      <div class="flex space-x-3">
        <Button v-if="isAdmin" @click="openEditModal">Edit Subject</Button>
        <Button variant="secondary" @click="$router.push('/dashboard/subjects')">
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
      <!-- Subject Info Card -->
      <div class="col-span-1 bg-white shadow rounded-lg p-6">
        <h2 class="text-lg font-medium mb-4">Subject Information</h2>
        <dl class="space-y-4">
          <div>
            <dt class="text-sm font-medium text-gray-500">Name</dt>
            <dd class="mt-1 text-sm text-gray-900">{{ subject?.name }}</dd>
          </div>
          <div>
            <dt class="text-sm font-medium text-gray-500">Department</dt>
            <dd class="mt-1 text-sm text-gray-900">{{ subject?.departmentName }}</dd>
          </div>
          <div>
            <dt class="text-sm font-medium text-gray-500">Status</dt>
            <dd class="mt-1">
              <Badge :status="subject?.isActive ? 'success' : 'error'">
                {{ subject?.isActive ? 'Active' : 'Inactive' }}
              </Badge>
            </dd>
          </div>
        </dl>
      </div>

      <!-- Stats Cards -->
      <div class="col-span-2 space-y-6">
        <div class="grid grid-cols-3 gap-4">
          <StatCard 
            title="Total Groups" 
            :value="subject?.groupsCount || 0"
          />
          <StatCard 
            title="Active Students" 
            :value="subject?.studentsCount || 0"
          />
          <StatCard 
            title="Average Attendance" 
            :value="`${subject?.averageAttendance || 0}%`"
          />
        </div>

        <!-- Groups Table -->
        <div class="bg-white shadow rounded-lg p-6">
          <div class="flex justify-between items-center mb-4">
            <h2 class="text-lg font-medium">Groups</h2>
            <Button v-if="isAdmin" variant="secondary" @click="openAddGroupModal">
              Add Group
            </Button>
          </div>
          <DataTable 
            :data="groups"
            :columns="groupColumns"
            :loading="isLoadingGroups"
          />
        </div>
      </div>
    </div>

    <!-- Edit Subject Modal -->
    <Modal v-model="showEditModal" title="Edit Subject">
      <SubjectForm
        v-if="showEditModal"
        :subject="subject"
        :departments="departments"
        @submit="handleUpdateSubject"
        @cancel="showEditModal = false"
      />
    </Modal>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { storeToRefs } from 'pinia'
import { useSubjectStore } from '@/stores/subject.store'
import { useDepartmentStore } from '@/stores/department.store'
import { useAuthStore } from '@/stores/auth.store'
import type { Subject } from '@/types/subject.types'
import type { StudyGroup } from '@/types/group.types'
import Button from '@/shared/components/ui/Button.vue'
import Badge from '@/shared/components/ui/Badge.vue'
import Spinner from '@/shared/components/ui/Spinner.vue'
import StatCard from '@/shared/components/ui/StatCard.vue'
import DataTable from '@/shared/components/ui/DataTable.vue'
import Modal from '@/shared/components/ui/Modal.vue'
import SubjectForm from '../components/SubjectForm.vue'

const route = useRoute()
const router = useRouter()
const subjectStore = useSubjectStore()
const departmentStore = useDepartmentStore()
const authStore = useAuthStore()

// Store refs
const { currentSubject: subject, isLoading, error } = storeToRefs(subjectStore)
const { departments } = storeToRefs(departmentStore)

// Component state
const showEditModal = ref(false)
const isLoadingGroups = ref(false)
const groups = ref<StudyGroup[]>([])

// Computed
const isAdmin = computed(() => authStore.userRole === 'admin')

const groupColumns = [
  { key: 'name', label: 'Name' },
  { key: 'professorName', label: 'Professor' },
  { key: 'studentsCount', label: 'Students' },
  { key: 'averageAttendance', label: 'Attendance Rate',
    render: (value: number) => `${value}%`
  }
]

// Methods
async function loadSubjectData(subjectId: number) {
  try {
    await Promise.all([
      subjectStore.fetchSubjectById(subjectId),
      loadGroups(subjectId)
    ])
  } catch (err) {
    console.error('Failed to load subject data:', err)
  }
}

async function loadGroups(subjectId: number) {
  isLoadingGroups.value = true
  try {
    groups.value = await subjectStore.fetchSubjectGroups(subjectId)
  } catch (err) {
    console.error('Failed to load groups:', err)
  } finally {
    isLoadingGroups.value = false
  }
}

async function handleUpdateSubject(updatedSubject: Partial<Subject>) {
  try {
    if (subject.value?.id) {
      await subjectStore.updateSubject(subject.value.id, updatedSubject)
      showEditModal.value = false
    }
  } catch (err) {
    console.error('Failed to update subject:', err)
  }
}

function openEditModal() {
  showEditModal.value = true
}

function openAddGroupModal() {
  router.push(`/dashboard/subjects/${route.params.id}/groups/new`)
}

onMounted(async () => {
  const subjectId = Number(route.params.id)
  if (subjectId) {
    await loadSubjectData(subjectId)
  }
})
</script>