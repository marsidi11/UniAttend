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
import { useUserStore } from '@/stores/user.store'
import { useDepartmentStore } from '@/stores/department.store'
import type { TableItem, StaffTableItem, Action } from '@/types/tableItem.types'
import type { StringRole } from '@/types/base.types'
import type { 
  UserDto, 
  CreateUserCommand, 
  UpdateUserCommand
} from '@/api/generated/data-contracts'
import Button from '@/shared/components/ui/Button.vue'
import DataTable from '@/shared/components/ui/DataTable.vue'
import Modal from '@/shared/components/ui/Modal.vue'
import UserForm from '../components/UserForm.vue'

// Initialize stores
const userStore = useUserStore()
const departmentStore = useDepartmentStore()

// Get store refs
const { users, isLoading } = storeToRefs(userStore)
const { departments } = storeToRefs(departmentStore)

// Helper function to map numeric roles to StringRole
function mapRole(roleId: number): StringRole {
  switch(roleId) {
    case 1: return 'admin'
    case 2: return 'secretary'
    case 3: return 'professor'
    case 4: return 'student'
    default: return 'student'
  }
}

// Staff computed property
const staff = computed(() => 
  users.value.map(user => ({
    ...user,
    id: user.id || 0,
    role: mapRole(user.role || 0)
  })) as StaffTableItem[]
)

// Component state
const showModal = ref(false)
const selectedStaff = ref<UserDto | null>(null)
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

const tableActions: Action<TableItem>[] = [
  { 
    label: 'Edit', 
    icon: 'edit', 
    action: (item: TableItem, event?: Event) => {
      event?.stopPropagation();
      handleEdit(item as UserDto);
    }
  },
  { 
    label: 'Toggle Status', 
    icon: 'toggle_on', 
    action: (item: TableItem, event?: Event) => {
      event?.stopPropagation();
      handleToggleStatus(item as UserDto);
    }
  }
]

// Computed properties
const modalTitle = computed(() => 
  selectedStaff.value ? 'Edit Staff Member' : 'Add Staff Member'
)

const filteredStaff = computed(() => {
  let filtered = [...staff.value]
  
  if (selectedRole.value) {
    filtered = filtered.filter(s => {
      const roleNumber = {
        'admin': 1,
        'secretary': 2,
        'professor': 3,
        'student': 4
      }[selectedRole.value]
      
      return s.role === mapRole(roleNumber || 0)
    })
  }
  
  if (selectedDepartment.value) {
    const deptId = Number(selectedDepartment.value)
    filtered = filtered.filter(s => s.departmentId === deptId)
  }
  
  if (selectedStatus.value !== '') {
    const isActive = selectedStatus.value === 'true'
    filtered = filtered.filter(s => s.isActive === isActive)
  }
  
  return filtered.filter(s => s.role !== 'student')
})

// Methods
function openCreateModal() {
  selectedStaff.value = null
  showModal.value = true
}

function handleEdit(staff: UserDto) {
  selectedStaff.value = staff
  showModal.value = true
}

async function handleToggleStatus(staff: UserDto) {
  if (!staff.id) return;
  
  try {
    if (staff.isActive) {
      // Deactivate user
      if (confirm('Are you sure you want to deactivate this staff member?')) {
        await userStore.deactivateUser(staff.id);
      }
    } else {
      // Activate user
      if (confirm('Are you sure you want to activate this staff member?')) {
        await userStore.updateUser(staff.id, { 
          id: staff.id,
          isActive: true 
        });
      }
    }
  } catch (err) {
    console.error('Failed to update staff status:', err);
  }
}

async function handleSubmit(data: CreateUserCommand | UpdateUserCommand) {
  try {
    if ('id' in data && data.id) {
      await userStore.updateUser(data.id, data)
    } else {
      await userStore.createUser(data as CreateUserCommand)
    }
    showModal.value = false
  } catch (err) {
    console.error('Failed to save staff member:', err)
  }
}

// Lifecycle hooks
onMounted(async () => {
  await Promise.all([
    userStore.fetchUsers(),
    departmentStore.fetchDepartments()
  ])
})
</script>