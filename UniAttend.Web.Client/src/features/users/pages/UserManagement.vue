<template>
  <div class="space-y-6">
    <!-- Header Section -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Staff Management</h1>
      <Button @click="openCreateModal">Add Staff Member</Button>
    </div>

    <!-- Filters -->
    <div class="flex gap-4 bg-white p-4 rounded-lg shadow">
      <div class="w-64">
        <label class="block text-sm font-medium text-gray-700">Role</label>
        <select
          v-model="selectedRole"
          class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
        >
          <option value="">All Roles</option>
          <option value="admin">Admin</option>
          <option value="secretary">Secretary</option>
          <option value="professor">Professor</option>
        </select>
      </div>
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

    <!-- Staff Table -->
    <div class="bg-white shadow rounded-lg">
      <DataTable
        :data="filteredStaff"
        :columns="columns"
        :loading="isLoading"
        :actions="tableActions"
      />
    </div>

    <!-- Create/Edit Staff Modal -->
    <Modal v-model="showModal" :title="modalTitle">
      <UserForm
        v-if="showModal"
        :staff="selectedStaff"
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
import { useAdminStore } from '@/stores/admin.store'
import { useDepartmentStore } from '@/stores/department.store'
import type { User } from '@/types/user.types'
import type { CreateStaffCommand, UpdateStaffRequest } from '@/types/admin.types'
import type { TableItem } from '@/types/tableItem.types'
import Button from '@/shared/components/ui/Button.vue'
import DataTable from '@/shared/components/ui/DataTable.vue'
import Modal from '@/shared/components/ui/Modal.vue'
import UserForm from '../components/UserForm.vue'

// Initialize stores
const adminStore = useAdminStore()
const departmentStore = useDepartmentStore()

// Get store refs
const { staff, isLoading } = storeToRefs(adminStore)
const { departments } = storeToRefs(departmentStore)

// Component state
const showModal = ref(false)
const selectedStaff = ref<User | null>(null)
const selectedRole = ref('')
const selectedDepartment = ref('')
const selectedStatus = ref('')

// Table configuration
const columns = [
  { key: 'username', label: 'Username' },
  { key: 'fullName', label: 'Full Name' },
  { key: 'role', label: 'Role' },
  { key: 'email', label: 'Email' },
  { key: 'departmentName', label: 'Department' },
  { key: 'isActive', label: 'Status',
    render: (value: boolean) => value ? 'Active' : 'Inactive'
  }
]

const tableActions = [
  { 
    label: 'Edit', 
    icon: 'edit', 
    action: (item: TableItem) => handleEdit(item as User)
  },
  { 
    label: 'Toggle Status', 
    icon: 'toggle_on', 
    action: (item: TableItem) => handleToggleStatus(item as User)
  }
]

// Computed properties
const modalTitle = computed(() => 
  selectedStaff.value ? 'Edit Staff Member' : 'Add Staff Member'
)

const filteredStaff = computed(() => {
  let filtered = [...staff.value]
  
  if (selectedRole.value) {
    filtered = filtered.filter(s => s.role.toLowerCase() === selectedRole.value)
  }
  
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
  selectedStaff.value = null
  showModal.value = true
}

function handleEdit(staff: User) {
  selectedStaff.value = staff
  showModal.value = true
}

async function handleToggleStatus(staff: User) {
  if (confirm(`Are you sure you want to ${staff.isActive ? 'deactivate' : 'activate'} this staff member?`)) {
    try {
      await adminStore.updateStaff(staff.id, { isActive: !staff.isActive })
    } catch (err) {
      console.error('Failed to update staff status:', err)
    }
  }
}

async function handleSubmit(data: CreateStaffCommand | UpdateStaffRequest) {
  try {
    if ('id' in data) {
      // It's an update
      await adminStore.updateStaff(data.id, data)
    } else {
      // It's a create
      await adminStore.createStaff(data)
    }
    showModal.value = false
  } catch (err) {
    console.error('Failed to save staff member:', err)
  }
}

// Lifecycle hooks
onMounted(async () => {
  await Promise.all([
    adminStore.getStaffList(),
    departmentStore.fetchDepartments()
  ])
})
</script>