<!-- <template>
  <div class="space-y-4">
    <div class="flex items-center justify-between">
      <h3 class="text-lg font-medium">Card Reader</h3>
      <span :class="statusClass">{{ status }}</span>
    </div>
    <input
      type="text"
      v-model="cardId"
      @keyup.enter="handleCardSwipe"
      ref="cardInput"
      class="sr-only"
      autofocus
    />
    <div class="text-center p-8 border-2 border-dashed rounded-lg">
      <p>Please swipe your card</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useAttendanceStore } from '@/stores/attendance.store'

const attendanceStore = useAttendanceStore()
const cardId = ref('')
const status = ref('Ready')

const statusClass = computed(() => ({
  'text-green-500': status.value === 'Ready',
  'text-yellow-500': status.value === 'Processing',
  'text-red-500': status.value === 'Error'
}))

async function handleCardSwipe() {
  if (!cardId.value) return
  
  status.value = 'Processing'
  try {
    await attendanceStore.recordCardAttendance(cardId.value)
    status.value = 'Success'
  } catch (error) {
    status.value = 'Error'
  }
  cardId.value = ''
  setTimeout(() => {
    status.value = 'Ready'
  }, 2000)
}

onMounted(() => {
  const cardInput = document.querySelector('input')
  if (cardInput) cardInput.focus()
})
</script> -->