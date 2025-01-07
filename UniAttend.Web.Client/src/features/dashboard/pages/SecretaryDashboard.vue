<template>
  <div class="space-y-6">
    <!-- Header -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Secretary Dashboard</h1>
    </div>

    <!-- Stats Overview -->
    <div class="grid grid-cols-4 gap-4">
      <StatCard
        title="Total Students"
        :value="stats.studentCount"
      />
      <StatCard
        title="Active Groups"
        :value="stats.activeGroups"
      />
      <StatCard
        title="Unassigned Cards"
        :value="stats.unassignedCards"
      />
      <StatCard
        title="Schedule Conflicts"
        :value="stats.scheduleConflicts"
        :status="stats.scheduleConflicts > 0 ? 'error' : 'success'"
      />
    </div>

    <!-- Quick Actions Grid -->
    <div class="grid grid-cols-2 gap-4">
      <!-- Primary Tasks -->
      <div class="bg-white p-6 rounded-lg shadow">
        <h2 class="text-lg font-medium mb-4">Quick Actions</h2>
        <div class="space-y-4">
          <Button 
            @click="router.push('/dashboard/students/register')" 
            class="w-full"
            variant="secondary"
          >
            Register New Student
          </Button>
          <Button 
            @click="router.push('/dashboard/students')" 
            class="w-full"
            variant="secondary"
          >
            Manage Student Cards
          </Button>
          <Button 
            @click="router.push('/dashboard/schedule')" 
            class="w-full"
            variant="secondary"
          >
            Manage Class Schedule
          </Button>
          <Button 
            @click="router.push('/dashboard/groups')" 
            class="w-full"
            variant="secondary"
          >
            Manage Study Groups
          </Button>
        </div>
      </div>

      <!-- Recent Activities -->
      <div class="bg-white p-6 rounded-lg shadow">
        <h2 class="text-lg font-medium mb-4">Recent Activities</h2>
        <div class="space-y-4">
          <div v-if="isLoading" class="flex justify-center">
            <Spinner :size="6" />
          </div>
          <div 
            v-else
            v-for="activity in recentActivities" 
            :key="activity.id"
            class="flex items-start space-x-3 p-2"
          >
            <span class="material-icons text-gray-400">{{ activity.icon }}</span>
            <div>
              <p class="text-sm">{{ activity.description }}</p>
              <p class="text-xs text-gray-500">{{ formatDate(activity.timestamp) }}</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { storeToRefs } from 'pinia'
import { useStudentStore } from '@/stores/student.store'
import { useGroupStore } from '@/stores/group.store'
import { formatDate } from '@/utils/dateUtils'
import Button from '@/shared/components/ui/Button.vue'
import StatCard from '@/shared/components/ui/StatCard.vue'
import Spinner from '@/shared/components/ui/Spinner.vue'

// Define activity interface
interface Activity {
  id: number
  icon: string
  description: string
  timestamp: Date
}

const router = useRouter()
const studentStore = useStudentStore()
const groupStore = useGroupStore()

// Get store refs
const { students } = storeToRefs(studentStore)
const { groups } = storeToRefs(groupStore)

const isLoading = ref(false)
const stats = ref({
  studentCount: 0,
  activeGroups: 0,
  unassignedCards: 0,
  scheduleConflicts: 0
})
const recentActivities = ref<Activity[]>([])

// Computed properties for stats
const unassignedCardsCount = computed(() => 
  students.value.filter(student => !student.cardId).length
)

async function loadDashboardData() {
  isLoading.value = true
  try {
    await Promise.all([
      studentStore.fetchStudentsList(),
      groupStore.fetchGroups()
    ])
    
    stats.value = {
      studentCount: students.value.length,
      activeGroups: groups.value.filter(g => g.isActive).length,
      unassignedCards: unassignedCardsCount.value,
      scheduleConflicts: 0 // This would come from a schedule validation service
    }
  } catch (err) {
    console.error('Failed to load dashboard data:', err)
  } finally {
    isLoading.value = false
  }
}

onMounted(() => {
  loadDashboardData()
})
</script>