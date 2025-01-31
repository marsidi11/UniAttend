<template>
  <div class="space-y-6">
    <!-- Welcome Section -->
    <div class="bg-white p-6 rounded-lg shadow">
      <div class="flex justify-between items-center mb-6">
        <div>
          <h1 class="text-2xl font-bold text-gray-900">Welcome back!</h1>
          <p class="text-gray-600">Here's what's happening across departments</p>
        </div>
        <Button @click="loadDashboardData" variant="secondary" class="flex items-center gap-2">
          <span class="material-icons text-sm">refresh</span>
          Refresh
        </Button>
      </div>

      <!-- Stats Overview -->
      <div class="grid grid-cols-4 gap-4">
        <StatCard title="Total Students" :value="stats.totalStudents" icon="school"
          :trend="`${stats.studentGrowth}% from last month`" />
        <StatCard title="Departments" :value="stats.totalDepartments" icon="account_balance"
          trend="Active departments" />
        <StatCard title="Study Groups" :value="stats.activeStudyGroups" icon="groups"
          :trend="`${stats.activeGroupsPercentage}% active`" />
        <StatCard title="Average Attendance" :value="`${stats.averageAttendance}%`" icon="fact_check"
          :status="stats.averageAttendance > 75 ? 'success' : 'warning'" trend="Last 30 days" />
      </div>
    </div>

    <!-- Main Content Grid -->
    <div class="grid grid-cols-3 gap-6">
      <!-- Department Stats -->
      <div class="bg-white p-6 rounded-lg shadow">
        <h2 class="text-lg font-medium mb-4 flex items-center gap-2">
          <span class="material-icons">analytics</span>
          Students by Department
        </h2>
        <div class="relative h-[300px]">
          <canvas ref="deptChartRef"></canvas>
          <div v-if="isLoading" class="absolute inset-0 flex items-center justify-center bg-white/50">
            <Spinner :size="8" />
          </div>
        </div>
      </div>

      <!-- Attendance Trends -->
      <div class="col-span-2">
        <div class="bg-white p-6 rounded-lg shadow h-full">
          <h2 class="text-lg font-medium mb-4 flex items-center gap-2">
            <span class="material-icons">trending_up</span>
            Attendance by Department
          </h2>
          <div class="relative h-[300px]">
            <canvas ref="attendanceChartRef"></canvas>
            <div v-if="isLoading" class="absolute inset-0 flex items-center justify-center bg-white/50">
              <Spinner :size="8" />
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Recent Activity -->
    <div class="space-y-4">
      <div v-for="dept in departmentReports" :key="dept.departmentId"
        class="flex items-center gap-3 p-3 bg-gray-50 rounded-lg">
        <span class="material-icons text-gray-600">business</span>
        <div class="flex-1">
          <p class="text-gray-900">{{ dept.departmentName }}</p>
          <p class="text-sm text-gray-500">
            {{ dept.totalStudents ?? 0 }} students | {{ dept.totalGroups ?? 0 }} groups |
            {{ Math.round(dept.averageAttendance ?? 0) }}% attendance
          </p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue'
import { storeToRefs } from 'pinia'
import { useStudentStore } from '@/stores/student.store'
import { useGroupStore } from '@/stores/studyGroup.store'
import { useDepartmentStore } from '@/stores/department.store'
import { useReportStore } from '@/stores/report.store'
import Button from '@/shared/components/ui/Button.vue'
import StatCard from '@/shared/components/ui/StatCard.vue'
import Spinner from '@/shared/components/ui/Spinner.vue'
import Chart from 'chart.js/auto'
import type { DepartmentReportDto } from '@/api/generated/data-contracts'

interface DashboardStats {
  totalStudents: number
  totalDepartments: number
  activeStudyGroups: number
  averageAttendance: number
  studentGrowth: number
  activeGroupsPercentage: number
}

const studentStore = useStudentStore()
const groupStore = useGroupStore()
const departmentStore = useDepartmentStore()
const reportStore = useReportStore()

const { students } = storeToRefs(studentStore)
const { studyGroups } = storeToRefs(groupStore)
const { departments } = storeToRefs(departmentStore)

const isLoading = ref(false)
const stats = ref<DashboardStats>({
  totalStudents: 0,
  totalDepartments: 0,
  activeStudyGroups: 0,
  averageAttendance: 0,
  studentGrowth: 0,
  activeGroupsPercentage: 0
})

const departmentReports = ref<DepartmentReportDto[]>([])
const deptChartRef = ref<HTMLCanvasElement | null>(null)
const attendanceChartRef = ref<HTMLCanvasElement | null>(null)
let deptChart: Chart | null = null
let attendanceChart: Chart | null = null

function initDepartmentChart(departments: { name: string; count: number }[]) {
  if (!deptChartRef.value) return

  deptChart?.destroy()
  deptChart = new Chart(deptChartRef.value, {
    type: 'pie',
    data: {
      labels: departments.map(d => d.name),
      datasets: [{
        data: departments.map(d => d.count),
        backgroundColor: [
          '#4F46E5', '#7C3AED', '#2563EB', '#DB2777',
          '#DC2626', '#EA580C', '#65A30D', '#0891B2'
        ]
      }]
    },
    options: {
      responsive: true,
      maintainAspectRatio: false,
      plugins: {
        legend: {
          position: 'right'
        }
      }
    }
  })
}

function initAttendanceChart(departments: { name: string; value: number }[]) {
  if (!attendanceChartRef.value) return

  attendanceChart?.destroy()
  attendanceChart = new Chart(attendanceChartRef.value, {
    type: 'bar',
    data: {
      labels: departments.map(d => d.name),
      datasets: [{
        label: 'Attendance Rate',
        data: departments.map(d => d.value),
        backgroundColor: '#4F46E5',
      }]
    },
    options: {
      responsive: true,
      maintainAspectRatio: false,
      plugins: {
        legend: {
          display: false
        }
      },
      scales: {
        y: {
          beginAtZero: true,
          max: 100,
          title: {
            display: true,
            text: 'Attendance Rate (%)'
          }
        }
      }
    }
  })
}

async function loadDashboardData() {
  isLoading.value = true
  try {
    await Promise.all([
      studentStore.fetchStudentsList(),
      groupStore.fetchStudyGroups(),
      departmentStore.fetchDepartments()
    ])

    const reports = await Promise.all(
      departments.value
        .filter((dept): dept is typeof dept & { id: number } => 
          typeof dept.id === 'number'
        )
        .map(dept => reportStore.getDepartmentReport(dept.id))
    )

    departmentReports.value = reports as DepartmentReportDto[]

    const totalGroups = studyGroups.value.length
    const activeGroups = studyGroups.value.filter(g => g.isActive).length

    // Update stats with safe access
    const totalAttendance = departmentReports.value
      .reduce((acc, dept) => acc + (dept?.averageAttendance ?? 0), 0)
    
    const validReportsCount = departmentReports.value.length || 1 // Prevent division by zero

    stats.value = {
      totalStudents: students.value.length,
      totalDepartments: departments.value.length,
      activeStudyGroups: activeGroups,
      averageAttendance: Math.round(totalAttendance / validReportsCount),
      studentGrowth: 5,
      activeGroupsPercentage: Math.round((activeGroups / (totalGroups || 1)) * 100)
    }

    // Initialize charts with proper type assertions
    initDepartmentChart(
      departments.value
        .filter((dept): dept is typeof dept & { name: string } =>
          typeof dept.name === 'string'
        )
        .map(dept => ({
          name: dept.name,
          count: departmentReports.value.find(r => r.departmentId === dept.id)?.totalStudents ?? 0
        }))
    )

    initAttendanceChart(
      departments.value
        .filter((dept): dept is typeof dept & { name: string } =>
          typeof dept.name === 'string'
        )
        .map(dept => ({
          name: dept.name,
          value: Math.round(
            departmentReports.value.find(r => r.departmentId === dept.id)?.averageAttendance ?? 0
          )
        }))
    )

  } catch (err) {
    console.error('Failed to load dashboard data:', err)
  } finally {
    isLoading.value = false
  }
}

onMounted(() => {
  loadDashboardData()
})

onUnmounted(() => {
  deptChart?.destroy()
  attendanceChart?.destroy()
})
</script>