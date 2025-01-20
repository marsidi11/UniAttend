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
        <option v-for="classroom in availableReaders" 
                :key="classroom.id" 
                :value="classroom.readerDeviceId">
          {{ classroom.name }} - {{ classroom.readerDeviceId }}
        </option>
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
import { ref, watch, computed } from 'vue'
import { storeToRefs } from 'pinia'
import { useClassroomStore } from '@/stores/classroom.store'
import Button from '@/shared/components/ui/Button.vue'
import type { 
  ClassroomDto, 
  CreateClassroomCommand, 
  UpdateClassroomCommand 
} from '@/api/generated/data-contracts'

interface Props {
  classroom?: ClassroomDto | null
}

const props = defineProps<Props>()
const emit = defineEmits<{
  (e: 'submit', data: CreateClassroomCommand | UpdateClassroomCommand): void
  (e: 'cancel'): void
}>()

const classroomStore = useClassroomStore()
const { isLoading, classroomsWithReaders } = storeToRefs(classroomStore)

const form = ref<CreateClassroomCommand>({
  name: '',
  readerDeviceId: null
})

const availableReaders = computed(() => 
  classroomsWithReaders.value.filter(c => 
    !c.readerDeviceId || c.id === props.classroom?.id
  )
)

watch(() => props.classroom, (newClassroom) => {
  if (newClassroom) {
    form.value = {
      name: newClassroom.name ?? '',
      readerDeviceId: newClassroom.readerDeviceId
    }
  } else {
    form.value = {
      name: '',
      readerDeviceId: null
    }
  }
}, { immediate: true })

async function handleSubmit() {
  try {
    isLoading.value = true
    if (props.classroom?.id) {
      emit('submit', {
        id: props.classroom.id,
        ...form.value
      } as UpdateClassroomCommand)
    } else {
      emit('submit', form.value as CreateClassroomCommand)
    }
  } finally {
    isLoading.value = false
  }
}
</script>