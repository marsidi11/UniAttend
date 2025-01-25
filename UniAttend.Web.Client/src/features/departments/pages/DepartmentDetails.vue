<template>
  <div class="space-y-6">
    <!-- Header Section -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Department Details</h1>
      <div class="flex space-x-3">
        <Button v-if="isAdmin" @click="openEditModal">Edit Department</Button>
        <Button variant="secondary" @click="$router.push('/dashboard/departments')">
          Back to List
        </Button>
      </div>
    </div>
  </div>

  <div v-if="isLoading">
    <Spinner :size="6" />
  </div>

  <div v-else-if="error" class="text-red-600">
    {{ error }}
  </div>

  <div v-else class="grid grid-cols-1 md:grid-cols-3 gap-6">
    <!-- Department Info Card -->
    <div class="col-span-1 bg-white shadow rounded-lg p-6">
      <h2 class="text-lg font-medium mb-4">Department Information</h2>
      <dl class="space-y-4">
        <div>
          <dt class="text-sm font-medium text-gray-500">Name</dt>
          <dd class="mt-1 text-sm text-gray-900">{{ department?.name }}</dd>
        </div>
        <div>
          <dt class="text-sm font-medium text-gray-500">Status</dt>
          <dd class="mt-1">
            <Badge :status="department?.isActive ? 'success' : 'error'">
              {{ department?.isActive ? 'Active' : 'Inactive' }}
            </Badge>
          </dd>
        </div>
      </dl>
    </div>

    <!-- Stats Cards -->
    <div class="col-span-2 grid grid-cols-3 gap-4">
      <StatCard title="Total Subjects" :value="department?.subjectsCount || 0" />
      <StatCard title="Total Students" :value="department?.studentsCount || 0" />
      <StatCard title="Total Professors" :value="department?.professorsCount || 0" />
    </div>

    <!-- Subjects Table -->
    <div class="col-span-3">
      <div class="bg-white shadow rounded-lg p-6">
        <div class="flex justify-between items-center mb-4">
          <h2 class="text-lg font-medium">Subjects</h2>
          <Button v-if="isAdmin" variant="secondary" @click="openAddSubjectModal">
            Add Subject
          </Button>
        </div>
        <DataTable :data="subjects" :columns="subjectColumns" :loading="isLoadingSubjects" />
      </div>
    </div>

    <!-- Edit Department Modal -->
    <Modal v-model="showEditModal" title="Edit Department">
      <DepartmentForm v-if="showEditModal" :department="department" @submit="handleUpdateDepartment"
        @cancel="showEditModal = false" />
    </Modal>

    <!-- Add Subject Modal -->
    <Modal v-model="showModal" title="Add Subject">
      <SubjectForm v-if="showModal" :departments="department ? [department] : []"
        :default-department-id="Number(route.params.id)" @submit="handleCreateSubject" @cancel="showModal = false" />
    </Modal>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { storeToRefs } from 'pinia'
import { useDepartmentStore } from '@/stores/department.store'
import { useAuthStore } from '@/stores/auth.store'
import { useSubjectStore } from '@/stores/subject.store'
import type { DepartmentDto, UpdateDepartmentCommand, SubjectDto, CreateSubjectCommand } from '@/api/generated/data-contracts'
import { TableItem } from '@/types/tableItem.types'
import Button from '@/shared/components/ui/Button.vue'
import Badge from '@/shared/components/ui/Badge.vue'
import Spinner from '@/shared/components/ui/Spinner.vue'
import StatCard from '@/shared/components/ui/StatCard.vue'
import DataTable from '@/shared/components/ui/DataTable.vue'
import Modal from '@/shared/components/ui/Modal.vue'
import DepartmentForm from '../components/DepartmentForm.vue'
import SubjectForm from '@/features/subjects/components/SubjectForm.vue'

const route = useRoute()
const departmentStore = useDepartmentStore()
const authStore = useAuthStore()
const subjectStore = useSubjectStore()
const showModal = ref(false)

const { currentDepartment: department, isLoading, error } = storeToRefs(departmentStore)
const showEditModal = ref(false)
const subjects = ref<(SubjectDto & TableItem)[]>([])
const isLoadingSubjects = ref(false)

const isAdmin = computed(() => authStore.userRole === 'admin')

const subjectColumns = [
  { key: 'name', label: 'Name' },
  { key: 'credits', label: 'Credits' },
  {
    key: 'isActive', label: 'Status',
    render: (value: boolean) => value ? 'Active' : 'Inactive'
  }
]

async function loadSubjects(departmentId: number) {
  isLoadingSubjects.value = true
  try {
    const data = await subjectStore.fetchSubjects({ departmentId })
    subjects.value = data.map(subject => ({
      ...subject,
      id: subject.id || 0
    })) as (SubjectDto & TableItem)[]
  } catch (err) {
    console.error('Failed to load subjects:', err)
  } finally {
    isLoadingSubjects.value = false
  }
}

onMounted(async () => {
  const departmentId = Number(route.params.id)
  if (departmentId) {
    await Promise.all([
      departmentStore.fetchDepartmentById(departmentId),
      loadSubjects(departmentId)
    ])
  }
})

async function handleUpdateDepartment(updatedDepartment: Partial<DepartmentDto>) {
  try {
    if (!department.value?.id) return

    const updateRequest: UpdateDepartmentCommand = {
      id: department.value.id,
      name: updatedDepartment.name || '',
      description: '',
      isActive: updatedDepartment.isActive ?? true
    }
    await departmentStore.updateDepartment(department.value.id, updateRequest)
    showEditModal.value = false
  } catch (err) {
    console.error('Failed to update department:', err)
  }
}

async function handleCreateSubject(data: CreateSubjectCommand) {
  try {
    await subjectStore.createSubject({
      ...data,
      departmentId: Number(route.params.id)
    })
    showModal.value = false
    await loadSubjects(Number(route.params.id))
  } catch (err) {
    console.error('Failed to create subject:', err)
  }
}

function openEditModal() {
  showEditModal.value = true
}

function openAddSubjectModal() {
  showModal.value = true
}
</script>