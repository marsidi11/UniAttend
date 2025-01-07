<template>
  <div class="space-y-6">
    <!-- Header Section -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">User Management</h1>
      <Button v-if="isAdmin" @click="openCreateModal">Add User</Button>
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
          <option value="professor">Professor</option>
          <option value="secretary">Secretary</option>
          <option value="student">Student</option>
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

    <!-- Users Table -->
    <div class="bg-white shadow rounded-lg">
      <DataTable
        :data="filteredUsers"
        :columns="columns"
        :loading="isLoading"
        :actions="tableActions"
        @row-click="handleRowClick"
      />
    </div>

    <!-- Create/Edit User Modal -->
    <Modal v-model="showModal" :title="modalTitle">
      <UserForm
        v-if="showModal"
        :user="selectedUser"
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
import { useAuthStore } from '@/stores/auth.store'
import type { User } from '@/types/user.types'
import Button from '@/shared/components/ui/Button.vue'
import DataTable from '@/shared/components/ui/DataTable.vue'
import Modal from '@/shared/components/ui/Modal.vue'
import UserForm from '../components/UserForm.vue'
import type { TableItem } from '@/types/tableItem.types'

const userStore = useUserStore()
const authStore = useAuthStore()

const { users, isLoading } = storeToRefs(userStore)
const showModal = ref(false)
const selectedUser = ref<User | null>(null)
const selectedRole = ref('')
const selectedStatus = ref('')

const isAdmin = computed(() => authStore.userRole === 'admin')

const columns = [
  { key: 'username', label: 'Username' },
  { key: 'fullName', label: 'Full Name' },
  { key: 'role', label: 'Role' },
  { key: 'email', label: 'Email' },
  { key: 'isActive', label: 'Status',
    render: (value: boolean) => value ? 'Active' : 'Inactive'
  }
]

const tableActions = computed(() => [
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
])

const modalTitle = computed(() => 
  selectedUser.value ? 'Edit User' : 'Add User'
)

const filteredUsers = computed(() => {
  let filtered = [...users.value]
  
  if (selectedRole.value) {
    filtered = filtered.filter(u => u.role.toLowerCase() === selectedRole.value)
  }
  
  if (selectedStatus.value !== '') {
    filtered = filtered.filter(u => u.isActive === (selectedStatus.value === 'true'))
  }
  
  return filtered
})

onMounted(async () => {
  await userStore.fetchUsers()
})

function openCreateModal() {
  selectedUser.value = null
  showModal.value = true
}

function handleEdit(user: User) {
  selectedUser.value = user
  showModal.value = true
}

async function handleToggleStatus(user: User) {
  if (confirm(`Are you sure you want to ${user.isActive ? 'deactivate' : 'activate'} this user?`)) {
    try {
      await userStore.updateUser(user.id, { isActive: !user.isActive })
    } catch (err) {
      console.error('Failed to update user status:', err)
    }
  }
}

async function handleSubmit(userData: Partial<User>) {
  try {
    if (selectedUser.value?.id) {
      await userStore.updateUser(selectedUser.value.id, userData)
    } else {
      await userStore.createUser(userData)
    }
    showModal.value = false
  } catch (err) {
    console.error('Failed to save user:', err)
  }
}

function handleRowClick(user: TableItem) {
  console.log('User clicked:', user)
}
</script>