<template>
  <div class="space-y-6">
    <!-- Header Section -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Subjects</h1>
      <Button v-if="isAdmin" @click="openCreateModal">Add Subject</Button>
    </div>

    <!-- Filters -->
    <div class="flex gap-4 bg-white p-4 rounded-lg shadow">
      <div class="w-64">
        <label class="block text-sm font-medium text-gray-700">Department</label>
        <select
          v-model="selectedDepartment"
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        >
          <option value="">All Departments</option>
          <option v-for="dept in departments" :key="dept.id" :value="dept.id">
            {{ dept.name }}
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

    <!-- Subjects Table -->
    <div class="bg-white shadow rounded-lg">
      <DataTable
        :data="filteredSubjects"
        :columns="columns"
        :loading="isLoading"
        :actions="isAdmin ? tableActions : undefined"
        @row-click="handleRowClick"
      />
    </div>

    <!-- Create/Edit Subject Modal -->
    <Modal v-model="showModal" :title="modalTitle">
      <SubjectForm
        v-if="showModal"
        :subject="selectedSubject"
        :departments="departments"
        @submit="handleSubmit"
        @cancel="showModal = false"
      />
    </Modal>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { storeToRefs } from 'pinia'
import { useSubjectStore } from '@/stores/subject.store'
import { useDepartmentStore } from '@/stores/department.store'
import { useAuthStore } from '@/stores/auth.store'
import type { 
  SubjectDto,
  CreateSubjectCommand,
  UpdateSubjectCommand 
} from '@/api/generated/data-contracts'
import type { TableItem } from '@/types/tableItem.types'
import Button from '@/shared/components/ui/Button.vue'
import DataTable from '@/shared/components/ui/DataTable.vue'
import Modal from '@/shared/components/ui/Modal.vue'
import SubjectForm from '../components/SubjectForm.vue'

// Store initialization
const subjectStore = useSubjectStore()
const departmentStore = useDepartmentStore()
const authStore = useAuthStore()

// Store refs
const { subjects, isLoading } = storeToRefs(subjectStore)
const { departments } = storeToRefs(departmentStore)

// Component state
const showModal = ref(false)
const selectedSubject = ref<SubjectDto | null>(null)
const selectedDepartment = ref('')
const selectedStatus = ref('')

// Computed properties
const isAdmin = computed(() => authStore.userRole === 'admin')

const columns = [
  { key: 'name', label: 'Name' },
  { key: 'departmentName', label: 'Department' },
  { key: 'isActive', label: 'Status',
    render: (value: boolean) => value ? 'Active' : 'Inactive'
  }
]

const tableActions = computed(() => isAdmin.value ? [
  { 
    label: 'Edit', 
    icon: 'edit', 
    action: (item: TableItem) => handleEdit(item as SubjectDto)
  },
  { 
    label: 'Delete', 
    icon: 'delete', 
    action: (item: TableItem) => handleDelete(item as SubjectDto)
  }
] : undefined)

const modalTitle = computed(() => 
  selectedSubject.value ? 'Edit Subject' : 'Add Subject'
)

const filteredSubjects = computed(() => {
  let filtered = [...subjects.value].map(subject => ({
    ...subject,
    id: subject.id || 0
  })) as (SubjectDto & TableItem)[]
  
  if (selectedDepartment.value) {
    filtered = filtered.filter(s => s.departmentId === Number(selectedDepartment.value))
  }
  
  if (selectedStatus.value !== '') {
    filtered = filtered.filter(s => s.isActive === (selectedStatus.value === 'true'))
  }
  
  return filtered
})

// Methods
function openCreateModal() {
  selectedSubject.value = null
  showModal.value = true
}

function handleEdit(subject: SubjectDto) {
  selectedSubject.value = subject
  showModal.value = true
}

async function handleDelete(subject: SubjectDto) {
  if (confirm('Are you sure you want to delete this subject?')) {
    try {
      if (subject.id) {
        await subjectStore.updateSubject(subject.id, { 
          id: subject.id,
          isActive: false 
        } as UpdateSubjectCommand)
      }
    } catch (err) {
      console.error('Failed to delete subject:', err)
    }
  }
}

async function handleSubmit(data: CreateSubjectCommand | UpdateSubjectCommand) {
  try {
    if (selectedSubject.value?.id) {
      await subjectStore.updateSubject(selectedSubject.value.id, data as UpdateSubjectCommand)
    } else {
      await subjectStore.createSubject(data as CreateSubjectCommand)
    }
    showModal.value = false
  } catch (err) {
    console.error('Failed to save subject:', err)
  }
}

function handleRowClick(subject: TableItem) {
  console.log('Subject clicked:', subject)
}

// Lifecycle hooks
onMounted(async () => {
  await Promise.all([
    subjectStore.fetchSubjects(),
    departmentStore.fetchDepartments()
  ])
})
</script>