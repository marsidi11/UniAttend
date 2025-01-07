<template>
  <div class="space-y-6">
    <!-- Header -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Classrooms</h1>
      <Button v-if="isAdmin" @click="openCreateModal">Add Classroom</Button>
    </div>

    <!-- Filters -->
    <div class="flex gap-4 bg-white p-4 rounded-lg shadow">
      <div class="w-64">
        <label class="block text-sm font-medium text-gray-700">Status</label>
        <select v-model="selectedStatus"
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm">
          <option value="">All</option>
          <option value="available">Available</option>
          <option value="inUse">In Use</option>
          <option value="maintenance">Maintenance</option>
        </select>
      </div>
    </div>

    <!-- Classrooms Table -->
    <div class="bg-white shadow rounded-lg">
      <DataTable :data="filteredClassrooms" :columns="columns" :loading="isLoading" :actions="tableActions"
        @row-click="handleRowClick" />
    </div>

    <!-- Create/Edit Modal -->
    <Modal v-model="showModal" :title="modalTitle">
      <ClassroomForm v-if="showModal" :classroom="selectedClassroom" @submit="handleSubmit"
        @cancel="showModal = false" />
    </Modal>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { storeToRefs } from 'pinia'
import { useClassroomStore } from '@/stores/classroom.store'
import { useAuthStore } from '@/stores/auth.store'
import type { Classroom, CreateClassroomRequest, ClassroomStatus } from '@/types/classroom.types'
import type { TableItem } from '@/types/tableItem.types'
import Button from '@/shared/components/ui/Button.vue'
import DataTable from '@/shared/components/ui/DataTable.vue'
import Modal from '@/shared/components/ui/Modal.vue'
import ClassroomForm from '../components/ClassroomForm.vue'

const classroomStore = useClassroomStore()
const authStore = useAuthStore()

const { classrooms, isLoading } = storeToRefs(classroomStore)
const showModal = ref(false)
const selectedClassroom = ref<Classroom | null>(null)
const selectedStatus = ref('')

const isAdmin = computed(() => authStore.userRole === 'admin')

const columns = [
  { key: 'name', label: 'Name' },
  { key: 'building', label: 'Building' },
  { key: 'capacity', label: 'Capacity' },
  {
    key: 'readerDeviceId', label: 'Reader Device',
    render: (value: string) => value || 'Not Assigned'
  },
  {
    key: 'status', label: 'Status',
    render: (value: string) => value.charAt(0).toUpperCase() + value.slice(1)
  }
]

const tableActions = computed(() => isAdmin.value ? [
  {
    label: 'Edit',
    icon: 'edit',
    action: (item: TableItem) => handleEdit(item as Classroom)
  },
  {
    label: 'Configure Reader',
    icon: 'settings',
    action: (item: TableItem) => handleConfigureReader(item as Classroom)
  }
] : [])

const modalTitle = computed(() =>
  selectedClassroom.value ? 'Edit Classroom' : 'Add Classroom'
)

const filteredClassrooms = computed(() => {
  let filtered = [...classrooms.value]

  if (selectedStatus.value) {
    filtered = filtered.filter(c => c.status === selectedStatus.value)
  }

  return filtered
})

onMounted(async () => {
  await classroomStore.fetchClassrooms()
})

function openCreateModal() {
  selectedClassroom.value = null
  showModal.value = true
}

function handleEdit(classroom: Classroom) {
  selectedClassroom.value = classroom
  showModal.value = true
}

async function handleConfigureReader(classroom: Classroom) {
  // Implementation for configuring reader device
  console.log('Configure reader for classroom:', classroom.id)
}

async function handleSubmit(formData: Partial<Classroom>) {
  try {
    if (selectedClassroom.value?.id) {
      // For updates, we can use Partial<Classroom>
      await classroomStore.updateClassroom(selectedClassroom.value.id, formData)
    } else {
      // For creation, we need to construct a proper CreateClassroomRequest
      const createRequest: CreateClassroomRequest = {
        name: formData.name || '',
        status: (formData.status as ClassroomStatus) || 'available',
        readerDeviceId: formData.readerDeviceId,
        createdAt: new Date()
      }
      await classroomStore.createClassroom(createRequest)
    }
    showModal.value = false
  } catch (err) {
    console.error('Failed to save classroom:', err)
  }
}

function handleRowClick(classroom: TableItem) {
  handleEdit(classroom as Classroom)
}
</script>