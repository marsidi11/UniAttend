<!-- <template>
  <form @submit.prevent="handleSubmit" class="space-y-6">
    <!-- Card ID -->
    <div>
      <label for="cardId" class="block text-sm font-medium text-gray-700">Card ID</label>
      <input
        id="cardId"
        v-model="form.cardId"
        type="text"
        required
        :readonly="!!card"
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
      />
    </div>

    <!-- Department Filter -->
    <div>
      <label for="departmentId" class="block text-sm font-medium text-gray-700">Department</label>
      <select
        id="departmentId"
        v-model="selectedDepartment"
        required
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
      >
        <option value="">Select Department</option>
        <option v-for="dept in departments" :key="dept.id" :value="dept.id">
          {{ dept.name }}
        </option>
      </select>
    </div>

    <!-- Student Selection -->
    <div>
      <label for="studentId" class="block text-sm font-medium text-gray-700">Student</label>
      <select
        id="studentId"
        v-model="form.studentId"
        required
        :disabled="!selectedDepartment"
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
      >
        <option value="">Select Student</option>
        <option v-for="student in filteredStudents" :key="student.id" :value="student.id">
          {{ student.firstName }} {{ student.lastName }} ({{ student.studentId }})
        </option>
      </select>
    </div>

    <div class="flex justify-end space-x-3">
      <Button type="button" variant="secondary" @click="$emit('cancel')">Cancel</Button>
      <Button type="submit" :loading="isLoading">Assign Card</Button>
    </div>
  </form>
</template> -->

<!-- <script setup lang="ts">
import { ref, computed, watch, onMounted } from 'vue'
import { storeToRefs } from 'pinia'
import { useStudentStore } from '@/stores/student.store'
import Button from '@/shared/components/ui/Button.vue'
import type { Card } from '@/types/card.types'
import type { Department } from '@/types/department.types'
import type { Student } from '@/types/student.types'


interface Props {
  card?: Card | null
  departments: Department[]
}

const props = defineProps<Props>()
const emit = defineEmits<{
  (e: 'submit', data: { cardId: string; studentId: number }): void
  (e: 'cancel'): void
}>()

const studentStore = useStudentStore()
const { students } = storeToRefs(studentStore)

const isLoading = ref(false)
const selectedDepartment = ref('')
const form = ref({
  cardId: '',
  studentId: 0
})

const filteredStudents = computed(() => 
  students.value.filter((student: Student) => 
    student.departmentId === Number(selectedDepartment.value) &&
    student.isActive &&
    !student.cardId
  )
)

watch(() => props.card, (newCard) => {
  if (newCard) {
    form.value.cardId = newCard.cardId
    if (newCard.departmentId) {
      selectedDepartment.value = String(newCard.departmentId)
    }
  }
}, { immediate: true })

onMounted(async () => {
  if (!students.value.length) {
    await studentStore.fetchStudents()
  }
})

async function handleSubmit() {
  try {
    isLoading.value = true
    emit('submit', {
      cardId: form.value.cardId,
      studentId: form.value.studentId
    })
  } finally {
    isLoading.value = false
  }
}
</script> -->