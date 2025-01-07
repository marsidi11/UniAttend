<template>
  <form @submit.prevent="handleSubmit" class="space-y-6">
    <!-- Name -->
    <div>
      <label for="name" class="block text-sm font-medium text-gray-700">Name</label>
      <input
        id="name"
        v-model="form.name"
        type="text"
        required
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
      />
    </div>

    <!-- Reader Device -->
    <div>
      <label for="readerDeviceId" class="block text-sm font-medium text-gray-700">Reader Device</label>
      <select
        id="readerDeviceId"
        v-model="form.readerDeviceId"
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
      >
        <option value="">No Reader</option>
        <option v-for="reader in availableReaders" :key="reader.id" :value="reader.id">
          {{ reader.name }}
        </option>
      </select>
    </div>

    <!-- Status -->
    <div>
      <label for="status" class="block text-sm font-medium text-gray-700">Status</label>
      <select
        id="status"
        v-model="form.status"
        required
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
      >
        <option value="available">Available</option>
        <option value="inUse">In Use</option>
        <option value="maintenance">Maintenance</option>
      </select>
    </div>

    <div class="flex justify-end space-x-3">
      <Button type="button" variant="secondary" @click="$emit('cancel')">
        Cancel
      </Button>
      <Button type="submit" :loading="isLoading">
        Save
      </Button>
    </div>
  </form>
</template>

<script setup lang="ts">
import { ref, watch, onMounted, computed } from 'vue'
import { storeToRefs } from 'pinia'
import { useClassroomStore } from '@/stores/classroom.store'
import Button from '@/shared/components/ui/Button.vue'
import type { Classroom } from '@/types/classroom.types'

interface Props {
  classroom?: Classroom | null
}

type ClassroomStatus = 'available' | 'inUse' | 'maintenance'

interface ClassroomFormData {
  name: string
  readerDeviceId: string
  status: ClassroomStatus
}

const props = defineProps<Props>()
const emit = defineEmits<{
  (e: 'submit', data: Partial<Classroom>): void
  (e: 'cancel'): void
}>()

const classroomStore = useClassroomStore()
const { readers } = storeToRefs(classroomStore)

const isLoading = ref(false)
const form = ref<ClassroomFormData>({
  name: '',
  readerDeviceId: '',
  status: 'available'
})

const availableReaders = computed(() => 
  readers.value.filter(r => !r.classroomId || r.classroomId === props.classroom?.id)
)

watch(() => props.classroom, (newClassroom) => {
  if (newClassroom) {
    form.value = {
      name: newClassroom.name,
      readerDeviceId: newClassroom.readerDeviceId || '',
      status: newClassroom.status || 'available'
    }
  } else {
    form.value = {
      name: '',
      readerDeviceId: '',
      status: 'available'
    }
  }
}, { immediate: true })

onMounted(async () => {
  if (!readers.value.length) {
    await classroomStore.fetchReaders()
  }
})

async function handleSubmit() {
  try {
    isLoading.value = true
    emit('submit', {
      name: form.value.name,
      readerDeviceId: form.value.readerDeviceId || null,
      status: form.value.status
    })
  } finally {
    isLoading.value = false
  }
}
</script>