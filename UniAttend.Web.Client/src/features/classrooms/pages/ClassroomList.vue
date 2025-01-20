<template>
  <div class="space-y-6">
    <!-- Header -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Classrooms</h1>
      <Button v-if="canManageClassrooms" @click="openCreateModal">Add Classroom</Button>
    </div>

    <!-- Classrooms Table -->
    <div class="bg-white shadow rounded-lg">
      <DataTable 
        :data="filteredClassrooms" 
        :columns="tableColumns" 
        :loading="isLoading" 
        :actions="tableActions"
        @row-click="handleRowClick" 
      />
    </div>

    <!-- Create/Edit Modal -->
    <Modal v-model="showModal" :title="modalTitle">
      <ClassroomForm 
        v-if="showModal" 
        :classroom="selectedClassroom" 
        @submit="handleSubmit"
        @cancel="showModal = false" 
      />
    </Modal>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { storeToRefs } from 'pinia'
import { useClassroomStore } from '@/stores/classroom.store'
import { useAuthStore } from '@/stores/auth.store'
import type { ClassroomDto, CreateClassroomCommand, UpdateClassroomCommand } from '@/api/generated/data-contracts'
import type { TableItem, Column } from '@/types/tableItem.types'
import Button from '@/shared/components/ui/Button.vue'
import DataTable from '@/shared/components/ui/DataTable.vue'
import Modal from '@/shared/components/ui/Modal.vue'
import ClassroomForm from '../components/ClassroomForm.vue'

const classroomStore = useClassroomStore()
const authStore = useAuthStore()

const { classrooms, isLoading } = storeToRefs(classroomStore)
const showModal = ref(false)
const selectedClassroom = ref<ClassroomDto | null>(null)

const canManageClassrooms = computed(() => 
  ['admin', 'secretary'].includes(authStore.userRole?.toLowerCase() || '')
)

const columns = [
  { 
    key: 'name', 
    label: 'Name',
    render: (value: string) => value || 'N/A' // Add render function
  },
  { 
    key: 'readerDeviceId', 
    label: 'Reader Device',
    render: (value: string | null) => value || 'Not Assigned'
  }
] as const

const tableColumns: Column<TableItem>[] = columns.map(col => ({
  ...col,
  render: col.render as ((value: any) => string)
}))

const tableActions = computed(() => canManageClassrooms.value ? [
  {
    label: 'Edit',
    icon: 'edit',
    action: (item: TableItem) => handleEdit(item as ClassroomDto)
  },
  {
    label: 'Configure Reader',
    icon: 'settings',
    action: (item: TableItem) => handleConfigureReader(item as ClassroomDto)
  }
] : [])

const modalTitle = computed(() =>
  selectedClassroom.value ? 'Edit Classroom' : 'Add Classroom'
)

const filteredClassrooms = computed((): TableItem[] => {
  let filtered = classrooms.value.map(classroom => ({
    ...classroom,
    id: classroom.id ?? 0
  }))
  return filtered
})

onMounted(async () => {
  await classroomStore.fetchClassrooms()
})

function openCreateModal() {
  selectedClassroom.value = null
  showModal.value = true
}

function handleEdit(classroom: ClassroomDto) {
  selectedClassroom.value = classroom
  showModal.value = true
}

async function handleConfigureReader(classroom: ClassroomDto) {
  if (!classroom.id) return
  
  try {
    if (classroom.readerDeviceId) {
      await classroomStore.removeReader(classroom.id)
    } else {
      const deviceId = window.prompt('Enter reader device ID:')
      if (deviceId) {
        await classroomStore.assignReader(classroom.id, deviceId)
      }
    }
  } catch (err) {
    console.error('Failed to configure reader:', err)
  }
}

async function handleSubmit(formData: CreateClassroomCommand | UpdateClassroomCommand) {
  try {
    if ('id' in formData && formData.id) {
      await classroomStore.updateClassroom(formData.id, formData)
    } else {
      await classroomStore.createClassroom(formData as CreateClassroomCommand)
    }
    showModal.value = false
  } catch (err) {
    console.error('Failed to save classroom:', err)
  }
}

function handleRowClick(classroom: TableItem) {
  if (canManageClassrooms.value) {
    handleEdit(classroom as ClassroomDto)
  }
}
</script>