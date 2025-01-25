<template>
  <div class="space-y-6">
    <!-- Header -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Study Groups</h1>
      <Button v-if="isAdmin" @click="openCreateModal">Add Group</Button>
    </div>

    <!-- Filters -->
    <div class="flex gap-4 bg-white p-4 rounded-lg shadow">
      <div class="w-64">
        <label class="block text-sm font-medium text-gray-700">Subject</label>
        <select
          v-model="selectedSubject"
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        >
          <option value="">All Subjects</option>
          <option v-for="subject in subjects" :key="subject.id" :value="subject.id">
            {{ subject.name }}
          </option>
        </select>
      </div>
      <div class="w-64">
        <label class="block text-sm font-medium text-gray-700">Status</label>
        <select
          v-model="selectedStatus"
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        >
          <option value="">All</option>
          <option value="true">Active</option>
          <option value="false">Inactive</option>
        </select>
      </div>
    </div>

    <!-- Groups Table -->
    <div class="bg-white shadow rounded-lg">
      <DataTable
        :data="filteredGroups"
        :columns="columns"
        :loading="isLoading"
        :actions="tableActions"
        @row-click="handleRowClick"
      />
    </div>

    <!-- Create/Edit Modal -->
    <Modal v-model="showModal" :title="modalTitle">
      <StudyGroupForm
        v-if="showModal"
        :group="selectedGroup"
        :subjects="subjects"
        @submit="handleSubmit"
        @cancel="showModal = false"
      />
    </Modal>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { storeToRefs } from 'pinia'
import { useGroupStore } from '@/stores/studyGroup.store'
import { useSubjectStore } from '@/stores/subject.store'
import { useAuthStore } from '@/stores/auth.store'
import type { StudyGroupDto } from '@/api/generated/data-contracts'
import type { TableItem } from '@/types/tableItem.types'
import Button from '@/shared/components/ui/Button.vue'
import DataTable from '@/shared/components/ui/DataTable.vue'
import Modal from '@/shared/components/ui/Modal.vue'
import StudyGroupForm from '../components/StudyGroupForm.vue'

interface ExtendedStudyGroup extends Omit<StudyGroupDto, 'id'>, TableItem {
  id: number; 
}
const router = useRouter()
const groupStore = useGroupStore()
const subjectStore = useSubjectStore()
const authStore = useAuthStore()

// Store refs
const { groups, isLoading } = storeToRefs(groupStore)
const { subjects } = storeToRefs(subjectStore)

// Component state
const showModal = ref(false)
const selectedGroup = ref<StudyGroupDto | null>(null)
const selectedSubject = ref('')
const selectedStatus = ref('')

// Computed properties
const isAdmin = computed(() => 
  authStore.userRole === 'admin' || authStore.userRole === 'secretary'
)

const columns = [
  { key: 'name', label: 'Group Name' },
  { key: 'subjectName', label: 'Subject' },
  { key: 'professorName', label: 'Professor' },
  { key: 'studentsCount', label: 'Students' },
  { 
    key: 'isActive', 
    label: 'Status',
    render: (value: boolean) => value ? 'Active' : 'Inactive'
  }
]

const tableActions = computed(() => [
  {
    label: 'View Details',
    icon: 'visibility',
    action: (item: TableItem) => handleViewDetails(item as StudyGroupDto)
  },
  ...(isAdmin.value ? [
    {
      label: 'Edit',
      icon: 'edit',
      action: (item: TableItem) => handleEdit(item as StudyGroupDto)
    }
  ] : [])
])

const modalTitle = computed(() => 
  selectedGroup.value ? 'Edit Group' : 'Add Group'
)

const filteredGroups = computed(() => {
  let filtered = groups.value.map(group => ({
    ...studyGroup,
    id: studyGroup.id ?? 0 // Use nullish coalescing
  })) as ExtendedStudyGroup[]
  
  if (selectedSubject.value) {
    filtered = filtered.filter(g => g.subjectId === Number(selectedSubject.value))
  }
  
  if (selectedStatus.value !== '') {
    filtered = filtered.filter(g => g.isActive === (selectedStatus.value === 'true'))
  }
  
  return filtered
})

// Methods
function openCreateModal() {
  selectedGroup.value = null
  showModal.value = true
}

function handleViewDetails(group: StudyGroupDto) {
  router.push(`/dashboard/groups/${studyGroup.id}`)
}

function handleEdit(group: StudyGroupDto) {
  selectedGroup.value = group
  showModal.value = true
}

function handleRowClick(group: TableItem) {
  handleViewDetails(group as StudyGroupDto)
}

async function handleSubmit(groupData: Partial<StudyGroupDto>) {
  try {
    if (selectedGroup.value?.id) {
      await groupStore.updateGroup(selectedGroup.value.id, groupData)
    } else {
      await groupStore.createGroup(groupData)
    }
    showModal.value = false
  } catch (err) {
    console.error('Failed to save group:', err)
  }
}

// Lifecycle hooks
onMounted(async () => {
  await Promise.all([
    groupStore.fetchGroups(),
    subjectStore.fetchSubjects()
  ])
})
</script>