<template>
  <div class="space-y-6">
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Attendance Reports</h1>
    </div>

    <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
      <!-- Student Report -->
      <div class="bg-white rounded-lg shadow p-6">
        <h2 class="text-lg font-medium mb-4">Student Report</h2>
        <div class="space-y-4">
          <div class="flex flex-col gap-4">
            <div class="flex gap-2">
              <select
                v-model="filters.studentId"
                class="w-full rounded-md border-gray-300"
              >
                <option value="">Select Student</option>
                <option 
                  v-for="student in students" 
                  :key="student.id" 
                  :value="student.id"
                >
                  {{ student.firstName }} {{ student.lastName }} ({{ student.studentId }})
                </option>
              </select>
              <Button variant="secondary" @click="showSearchStudentModal = true">
                <span class="material-icons">search</span>
              </Button>
            </div>
            <div class="grid grid-cols-2 gap-4">
              <DatePicker v-model="filters.startDate" placeholder="Start Date" />
              <DatePicker v-model="filters.endDate" placeholder="End Date" />
            </div>
            <Button
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
            <div class="flex gap-2">
              <select
                v-model="filters.studyGroupId"
                class="w-full rounded-md border-gray-300"
              >
                <option value="">Select Study Group</option>
                <option 
                  v-for="group in studyGroups" 
                  :key="group.id" 
                  :value="group.id"
                >
                  {{ group.name }} ({{ group.subjectName }})
                </option>
              </select>
              <Button variant="secondary" @click="showSearchGroupModal = true">
                <span class="material-icons">search</span>
              </Button>
            </div>
            <div class="grid grid-cols-2 gap-4">
              <DatePicker v-model="filters.startDate" placeholder="Start Date" />
              <DatePicker v-model="filters.endDate" placeholder="End Date" />
            </div>
            <Button
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
            <select
              v-model="filters.departmentId"
              class="rounded-md border-gray-300"
            >
              <option value="">Select Department</option>
              <option 
                v-for="dept in departments" 
                :key="dept.id" 
                :value="dept.id"
              >
                {{ dept.name }}
              </option>
            </select>
            <select
              v-model="filters.academicYearId"
              class="rounded-md border-gray-300"
            >
              <option value="">Select Academic Year</option>
              <option 
                v-for="year in academicYears" 
                :key="year.id" 
                :value="year.id"
              >
                {{ year.name }}
              </option>
            </select>
            <Button
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

    <!-- Search Modals -->
    <Modal v-model="showSearchStudentModal" title="Search Student">
      <div class="space-y-4">
        <input
          v-model="studentSearch"
          type="text"
          placeholder="Search by name or ID"
          class="w-full rounded-md border-gray-300"
        />
        <div class="max-h-96 overflow-y-auto">
          <div 
            v-for="student in filteredStudents"
            :key="student.id"
            class="p-3 hover:bg-gray-50 cursor-pointer"
            @click="selectStudent(student)"
          >
            {{ student.firstName }} {{ student.lastName }} ({{ student.studentId }})
          </div>
        </div>
      </div>
    </Modal>

    <Modal v-model="showSearchGroupModal" title="Search Study Group">
      <div class="space-y-4">
        <input
          v-model="groupSearch"
          type="text"
          placeholder="Search by name or subject"
          class="w-full rounded-md border-gray-300"
        />
        <div class="max-h-96 overflow-y-auto">
          <div 
            v-for="group in filteredGroups"
            :key="group.id"
            class="p-3 hover:bg-gray-50 cursor-pointer"
            @click="selectGroup(group)"
          >
            {{ group.name }} ({{ group.subjectName }})
          </div>
        </div>
      </div>
    </Modal>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, type Ref } from 'vue'
import { storeToRefs } from 'pinia'
import { useReportStore } from '@/stores/report.store'
import { useStudentStore } from '@/stores/student.store'
import { useGroupStore } from '@/stores/studyGroup.store'
import { useDepartmentStore } from '@/stores/department.store'
import { useAcademicYearStore } from '@/stores/academicYear.store'
import type { UserDto } from '@/api/generated/data-contracts'
import Button from '@/shared/components/ui/Button.vue'
import DatePicker from '@/shared/components/ui/DatePicker.vue'
import Modal from '@/shared/components/ui/Modal.vue'

// Extended interface to include studentId
interface ExtendedStudent extends UserDto {
  studentId?: string
}

const reportStore = useReportStore()
const studentStore = useStudentStore()
const groupStore = useGroupStore()
const departmentStore = useDepartmentStore()
const academicYearStore = useAcademicYearStore()

// Get store refs with proper typing
const { students } = storeToRefs(studentStore) as { students: Ref<ExtendedStudent[]> }
const { studyGroups } = storeToRefs(groupStore)
const { departments } = storeToRefs(departmentStore)
const { academicYears } = storeToRefs(academicYearStore)

// Search state
const showSearchStudentModal = ref(false)
const showSearchGroupModal = ref(false)
const studentSearch = ref('')
const groupSearch = ref('')

const filters = ref({
  studentId: null as number | null,
  studyGroupId: null as number | null,
  departmentId: null as number | null,
  academicYearId: null as number | null,
  startDate: '' as string,
  endDate: '' as string,
})

// Filtered search results
const filteredStudents = computed(() => {
  const search = studentSearch.value.toLowerCase()
  return students.value.filter((s: ExtendedStudent) => 
    s.firstName?.toLowerCase().includes(search) ||
    s.lastName?.toLowerCase().includes(search) ||
    s.studentId?.toLowerCase().includes(search)
  )
})

const filteredGroups = computed(() => {
  const search = groupSearch.value.toLowerCase()
  return studyGroups.value.filter(g =>
    g.name?.toLowerCase().includes(search) ||
    g.subjectName?.toLowerCase().includes(search)
  )
})

// Selection handlers
function selectStudent(student: ExtendedStudent) {
  filters.value.studentId = student.id || null
  showSearchStudentModal.value = false
}

function selectGroup(group: any) {
  filters.value.studyGroupId = group.id
  showSearchGroupModal.value = false
}

// Export handlers
async function handleExportStudentReport() {
  if(reportStore.isLoading || !filters.value.studentId) return
  await reportStore.exportStudentReport(
    filters.value.studentId,
    filters.value.startDate,
    filters.value.endDate
  )
}

async function handleExportGroupReport() {
  if(reportStore.isLoading || !filters.value.studyGroupId) return
  await reportStore.exportGroupReport(
    filters.value.studyGroupId,
    new Date(filters.value.startDate),
    new Date(filters.value.endDate)
  )
}

async function handleExportDepartmentReport() {
  if(reportStore.isLoading || !filters.value.departmentId) return
  await reportStore.exportDepartmentReport(
    filters.value.departmentId,
    filters.value.academicYearId || undefined
  )
}

// Load initial data
onMounted(async () => {
  await Promise.all([
    studentStore.fetchStudentsList(),
    groupStore.fetchStudyGroups(),
    departmentStore.fetchDepartments(),
    academicYearStore.fetchAcademicYears()
  ])
})
</script>