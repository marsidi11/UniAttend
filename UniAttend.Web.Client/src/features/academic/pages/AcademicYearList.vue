<template>
  <div class="space-y-6">
    <!-- Header -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-semibold">Academic Years</h1>
      <Button @click="showCreateModal = true" variant="primary">
        Create Academic Year
      </Button>
    </div>

    <!-- Academic Years List -->
    <div class="bg-white shadow rounded-lg">
      <div class="overflow-x-auto">
        <table class="min-w-full divide-y divide-gray-200">
          <thead class="bg-gray-50">
            <tr>
              <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Name
              </th>
              <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Period
              </th>
              <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Status
              </th>
              <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Groups
              </th>
              <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Students
              </th>
              <th class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">
                Actions
              </th>
            </tr>
          </thead>
          <tbody class="bg-white divide-y divide-gray-200">
            <tr v-for="year in sortedYears" :key="year.id">
              <td class="px-6 py-4 whitespace-nowrap">{{ year.name }}</td>
              <td class="px-6 py-4 whitespace-nowrap">
                {{ formatDateString(year.startDate) }} - {{ formatDateString(year.endDate) }}
              </td>
              <td class="px-6 py-4 whitespace-nowrap">
                <Badge :status="year.isActive ? 'success' : 'error'">
                  {{ year.isActive ? 'Active' : 'Closed' }}
                </Badge>
              </td>
              <td class="px-6 py-4 whitespace-nowrap">
                {{ year.totalGroups || 0 }} groups
              </td>
              <td class="px-6 py-4 whitespace-nowrap">
                {{ year.totalStudents || 0 }} students
              </td>
              <td class="px-6 py-4 whitespace-nowrap text-right">
                <Button 
                  v-if="year.isActive"
                  @click="handleCloseYear(year)" 
                  variant="danger" 
                  size="sm"
                >
                  Close Year
                </Button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- Create Modal -->
    <Modal v-model="showCreateModal" title="Create Academic Year">
      <AcademicYearForm 
        @submit="handleCreateYear"
        @cancel="showCreateModal = false"
      />
    </Modal>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { storeToRefs } from 'pinia'
import { useAcademicYearStore } from '@/stores/academicYear.store'
import { formatDate } from '@/utils/dateUtils'
import type { AcademicYearDto, CreateAcademicYearCommand } from '@/api/generated/data-contracts'
import Modal from '@/shared/components/ui/Modal.vue'
import Button from '@/shared/components/ui/Button.vue'
import Badge from '@/shared/components/ui/Badge.vue'
import AcademicYearForm from '../components/AcademicYearForm.vue'

const academicYearStore = useAcademicYearStore()
const { academicYears } = storeToRefs(academicYearStore)
const showCreateModal = ref(false)

// Helper function to safely format dates
function formatDateString(date: string | undefined): string {
  if (!date) return ''
  return formatDate(new Date(date))
}

// Sort years by start date (newest first)
const sortedYears = computed(() => 
  [...academicYears.value].sort((a, b) => {
    const dateA = a.startDate ? new Date(a.startDate).getTime() : 0
    const dateB = b.startDate ? new Date(b.startDate).getTime() : 0
    return dateB - dateA
  })
)

async function handleCreateYear(data: CreateAcademicYearCommand) {
  try {
    await academicYearStore.createAcademicYear(data)
    showCreateModal.value = false
  } catch (error) {
    console.error('Failed to create academic year:', error)
  }
}

async function handleCloseYear(year: AcademicYearDto) {
  if (!year.id) return
  
  try {
    await academicYearStore.closeAcademicYear(year.id)
  } catch (error) {
    console.error('Failed to close academic year:', error)
  }
}

// Load academic years on component mount
onMounted(() => {
  academicYearStore.fetchAcademicYears()
})
</script>