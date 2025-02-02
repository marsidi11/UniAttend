<template>
  <div class="space-y-6">
    <!-- Header Section -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Attendance Reports</h1>
    </div>

    <!-- Report Types -->
    <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
      <!-- Student Report -->
      <div class="bg-white rounded-lg shadow p-6">
        <h2 class="text-lg font-medium mb-4">Student Report</h2>
        <div class="space-y-4">
          <div class="flex flex-col gap-4">
            <input 
              type="number" 
              v-model="filters.studentId" 
              placeholder="Student ID"
              class="rounded-md border-gray-300" 
            />
            <div class="grid grid-cols-2 gap-4">
              <DatePicker 
                v-model="filters.startDate" 
                placeholder="Start Date" 
              />
              <DatePicker 
                v-model="filters.endDate" 
                placeholder="End Date" 
              />
            </div>
            <Button 
              type="button"
              @click="handleExportStudentReport" 
              :loading="reportStore.isLoading"
              :disabled="reportStore.isLoading || !filters.studentId"
            >
              <span class="material-icons mr-2">download</span>
              Export Student Report
            </Button>
          </div>
        </div>
      </div>

      <!-- Group Report -->
      <div class="bg-white rounded-lg shadow p-6">
        <h2 class="text-lg font-medium mb-4">Study Group Report</h2>
        <div class="space-y-4">
          <div class="flex flex-col gap-4">
            <input 
              type="number" 
              v-model="filters.studyGroupId" 
              placeholder="Group ID" 
              class="rounded-md border-gray-300" 
            />
            <div class="grid grid-cols-2 gap-4">
              <DatePicker 
                v-model="filters.startDate" 
                placeholder="Start Date" 
              />
              <DatePicker 
                v-model="filters.endDate" 
                placeholder="End Date" 
              />
            </div>
            <Button 
              type="button"
              @click="handleExportGroupReport" 
              :loading="reportStore.isLoading"
              :disabled="reportStore.isLoading || !filters.studyGroupId"
            >
              <span class="material-icons mr-2">download</span>
              Export Group Report
            </Button>
          </div>
        </div>
      </div>

      <!-- Department Report -->
      <div class="bg-white rounded-lg shadow p-6">
        <h2 class="text-lg font-medium mb-4">Department Report</h2>
        <div class="space-y-4">
          <div class="flex flex-col gap-4">
            <input 
              type="number" 
              v-model="filters.departmentId" 
              placeholder="Department ID"
              class="rounded-md border-gray-300" 
            />
            <input 
              type="number" 
              v-model="filters.academicYearId" 
              placeholder="Academic Year ID"
              class="rounded-md border-gray-300" 
            />
            <Button 
              type="button"
              @click="handleExportDepartmentReport" 
              :loading="reportStore.isLoading"
              :disabled="reportStore.isLoading || !filters.departmentId"
            >
              <span class="material-icons mr-2">download</span>
              Export Department Report
            </Button>
          </div>
        </div>
      </div>
    </div>
    <!-- Error Display -->
    <div v-if="reportStore.error" class="bg-red-50 text-red-600 p-4 rounded-lg">
      {{ reportStore.error }}
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useReportStore } from '@/stores/report.store'
import Button from '@/shared/components/ui/Button.vue'
import DatePicker from '@/shared/components/ui/DatePicker.vue'

// Ensure stringToDate is defined (or import it).
function stringToDate(dateStr: string): Date {
  return new Date(dateStr)
}

const reportStore = useReportStore()

const filters = ref({
  studentId: null as number | null,
  studyGroupId: null as number | null,
  departmentId: null as number | null,
  academicYearId: null as number | null,
  startDate: '' as string,
  endDate: '' as string,
})

async function handleExportStudentReport() {
  if(reportStore.isLoading) return
  if (!filters.value.studentId) return
  await reportStore.exportStudentReport(
    filters.value.studentId,
    filters.value.startDate,  
    filters.value.endDate   
  )
}

async function handleExportGroupReport() {
  if(reportStore.isLoading) return
  if (!filters.value.studyGroupId) return
  await reportStore.exportGroupReport(
    filters.value.studyGroupId,
    stringToDate(filters.value.startDate),
    stringToDate(filters.value.endDate)
  )
}

async function handleExportDepartmentReport() {
  if(reportStore.isLoading) return
  if (!filters.value.departmentId) return
  await reportStore.exportDepartmentReport(
    filters.value.departmentId,
    filters.value.academicYearId || undefined
  )
}
</script>