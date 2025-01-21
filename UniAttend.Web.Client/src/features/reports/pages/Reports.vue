<template>
  <div class="space-y-6">
    <!-- Header Section -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Reports & Analytics</h1>
      <div class="flex gap-2">
        <Button v-if="canExport" @click="handleExportAttendance" variant="secondary" :loading="reportStore.isLoading">
          <span class="material-icons mr-2">download</span>
          Export Attendance Data
        </Button>
      </div>
    </div>

    <!-- Report Types -->
    <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
      <!-- Student Reports -->
      <div class="bg-white rounded-lg shadow p-6">
        <h2 class="text-lg font-medium mb-4">Student Reports</h2>
        <div class="space-y-4">
          <div class="flex flex-col gap-4">
            <input type="number" v-model="filters.studentId" placeholder="Student ID"
              class="rounded-md border-gray-300" />
            <DatePicker v-model="filters.startDate" placeholder="Start Date" />
            <DatePicker v-model="filters.endDate" placeholder="End Date" />
            <Button @click="handleStudentReport" :loading="reportStore.isLoading">
              Generate Student Report
            </Button>
          </div>
        </div>
      </div>

      <!-- Group Reports -->
      <div class="bg-white rounded-lg shadow p-6">
        <h2 class="text-lg font-medium mb-4">Group Reports</h2>
        <div class="space-y-4">
          <div class="flex flex-col gap-4">
            <input type="number" v-model="filters.studyGroupId" placeholder="Group ID" class="rounded-md border-gray-300" />
            <DatePicker v-model="filters.startDate" placeholder="Start Date" />
            <DatePicker v-model="filters.endDate" placeholder="End Date" />
            <Button @click="handleGroupReport" :loading="reportStore.isLoading">
              Generate Group Report
            </Button>
          </div>
        </div>
      </div>

      <!-- Department Reports -->
      <div class="bg-white rounded-lg shadow p-6">
        <h2 class="text-lg font-medium mb-4">Department Reports</h2>
        <div class="space-y-4">
          <div class="flex flex-col gap-4">
            <input type="number" v-model="filters.departmentId" placeholder="Department ID"
              class="rounded-md border-gray-300" />
            <input type="number" v-model="filters.academicYearId" placeholder="Academic Year ID"
              class="rounded-md border-gray-300" />
            <Button @click="handleDepartmentReport" :loading="reportStore.isLoading">
              Generate Department Report
            </Button>
          </div>
        </div>
      </div>
    </div>

    <!-- Report Results -->
    <div v-if="reportStore.error" class="bg-red-50 text-red-600 p-4 rounded-lg">
      {{ reportStore.error }}
    </div>

    <div v-if="activeReport" class="bg-white rounded-lg shadow p-6">
      <h2 class="text-lg font-medium mb-4">Report Results</h2>
      <pre class="bg-gray-50 p-4 rounded-lg overflow-auto">
        {{ JSON.stringify(activeReport, null, 2) }}
      </pre>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useReportStore } from '@/stores/report.store'
import { useAuthStore } from '@/stores/auth.store'
import Button from '@/shared/components/ui/Button.vue'
import DatePicker from '@/shared/components/ui/DatePicker.vue'

const reportStore = useReportStore()
const authStore = useAuthStore()

const filters = ref({
  studentId: null as number | null,
  studyGroupId: null as number | null,
  departmentId: null as number | null,
  academicYearId: null as number | null,
  startDate: '' as string,
  endDate: '' as string,
})

const canExport = computed(() => 
  ['admin', 'secretary'].includes(authStore.userRole)
)

const activeReport = computed(() => {
  return reportStore.studentReport || 
         reportStore.groupReport || 
         reportStore.departmentReport || 
         reportStore.attendanceReport || 
         reportStore.academicYearReport
})

// Helper function to convert string to Date
function stringToDate(dateStr: string): Date | undefined {
  return dateStr ? new Date(dateStr) : undefined
}

async function handleStudentReport() {
  if (!filters.value.studentId) return
  await reportStore.getStudentReport(
    filters.value.studentId,
    stringToDate(filters.value.startDate),
    stringToDate(filters.value.endDate)
  )
}

async function handleGroupReport() {
  if (!filters.value.studyGroupId) return
  await reportStore.getGroupReport(
    filters.value.studyGroupId,
    stringToDate(filters.value.startDate),
    stringToDate(filters.value.endDate)
  )
}

async function handleDepartmentReport() {
  if (!filters.value.departmentId) return
  await reportStore.getDepartmentReport(
    filters.value.departmentId,
    filters.value.academicYearId || undefined
  )
}

async function handleExportAttendance() {
  if (!filters.value.studyGroupId || !filters.value.startDate || !filters.value.endDate) {
    return
  }
  
  await reportStore.exportAttendanceReport(
    filters.value.studyGroupId,
    new Date(filters.value.startDate),
    new Date(filters.value.endDate)
  )
}
</script>