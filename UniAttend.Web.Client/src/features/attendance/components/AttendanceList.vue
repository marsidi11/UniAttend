<template>
  <div class="space-y-4">
    <div class="flex justify-between items-center">
      <h2 class="text-xl font-semibold">Attendance History</h2>
      <div class="flex gap-4">
        <DatePicker v-model="startDate" placeholder="Start Date" />
        <DatePicker v-model="endDate" placeholder="End Date" />
        <Button @click="loadAttendance">Filter</Button>
      </div>
    </div>

    <div v-if="isLoading" class="flex justify-center">
      <Spinner />
    </div>

    <div v-else-if="records.length === 0" class="text-center py-8">
      No attendance records found
    </div>

    <table v-else class="min-w-full divide-y divide-gray-200">
      <thead class="bg-gray-50">
        <tr>
          <th>Date</th>
          <th>Course</th>
          <th>Professor</th>
          <th>Check-in Method</th>
          <th>Status</th>
        </tr>
      </thead>
      <tbody class="bg-white divide-y divide-gray-200">
        <tr v-for="record in records" :key="record.id">
          <td>{{ formatDate(record.checkInTime) }}</td>
          <td>{{ record.courseName }}</td>
          <td>{{ record.professorName }}</td>
          <td>{{ record.checkInMethod }}</td>
          <td>
            <Badge :status="record.isConfirmed ? 'success' : 'warning'">
              {{ record.isConfirmed ? 'Confirmed' : 'Pending' }}
            </Badge>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { storeToRefs } from 'pinia';
import { useAttendanceStore } from '@/stores/attendance.store';
import { formatDate } from '@/utils/dateUtils';
import DatePicker from '@/shared/components/ui/DatePicker.vue';
import Button from '@/shared/components/ui/Button.vue';
import Spinner from '@/shared/components/ui/Spinner.vue';
import Badge from '@/shared/components/ui/Badge.vue';

const attendanceStore = useAttendanceStore();
const startDate = ref<string>();
const endDate = ref<string>();

const { records, isLoading } = storeToRefs(attendanceStore);

async function loadAttendance() {
  const start = startDate.value ? new Date(startDate.value) : undefined;
  const end = endDate.value ? new Date(endDate.value) : undefined;
  await attendanceStore.fetchAttendance(start, end);
}

onMounted(() => {
  loadAttendance();
});
</script>