<template>
  <form @submit.prevent="handleSubmit" class="space-y-6">
    <!-- Name -->
    <div>
      <label for="name" class="block text-sm font-medium text-gray-700">Group Name</label>
      <input
        id="name"
        v-model="form.name"
        type="text"
        required
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
      />
    </div>

    <!-- Subject -->
    <div>
      <label for="subjectId" class="block text-sm font-medium text-gray-700">Subject</label>
      <select
        id="subjectId"
        v-model="form.subjectId"
        required
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
      >
        <option value="">Select Subject</option>
        <option v-for="subject in subjects" :key="subject.id" :value="subject.id">
          {{ subject.name }}
        </option>
      </select>
    </div>

    <!-- Professor -->
    <div>
      <label for="professorId" class="block text-sm font-medium text-gray-700">Professor</label>
      <select
        id="professorId"
        v-model="form.professorId"
        required
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
      >
        <option value="">Select Professor</option>
        <option v-for="professor in availableProfessors" :key="professor.id" :value="professor.id">
          {{ professor.firstName }} {{ professor.lastName }}
        </option>
      </select>
    </div>

    <!-- Academic Year -->
    <div>
      <label for="academicYearId" class="block text-sm font-medium text-gray-700">Academic Year</label>
      <select
        id="academicYearId"
        v-model="form.academicYearId"
        required
        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
      >
        <option value="">Select Academic Year</option>
        <option v-for="year in academicYears" :key="year.id" :value="year.id">
          {{ year.name }}
        </option>
      </select>
    </div>

    <!-- Active Status -->
    <div>
      <label class="flex items-center">
        <input
          type="checkbox"
          v-model="form.isActive"
          class="rounded border-gray-300 text-indigo-600 focus:ring-indigo-500"
        />
        <span class="ml-2 text-sm text-gray-600">Active</span>
      </label>
    </div>

    <div class="flex justify-end space-x-3">
      <Button type="button" variant="secondary" @click="$emit('cancel')">Cancel</Button>
      <Button type="submit" :loading="isLoading">Save</Button>
    </div>
  </form>
</template>

<script setup lang="ts">
import { ref, computed, watch, onMounted } from 'vue'
import { storeToRefs } from 'pinia'
import { useUserStore } from '@/stores/user.store'
import { useAcademicYearStore } from '@/stores/academicYear.store'
import Button from '@/shared/components/ui/Button.vue'
import type { 
  StudyGroupDto,
  CreateGroupCommand,
  UpdateGroupCommand,
  SubjectDto
} from '@/api/generated/data-contracts'

interface Props {
  group?: StudyGroupDto | null
  subjects: Array<SubjectDto>
}

const props = defineProps<Props>()
const emit = defineEmits<{
  (e: 'submit', data: Partial<CreateGroupCommand | UpdateGroupCommand>): void
  (e: 'cancel'): void
}>()

const userStore = useUserStore()
const academicYearStore = useAcademicYearStore()

const { users } = storeToRefs(userStore)
const { academicYears } = storeToRefs(academicYearStore)

const isLoading = ref(false)
const form = ref({
  name: '',
  subjectId: 0,
  professorId: 0,
  academicYearId: 0,
  isActive: true
})

// Filter professors from users - using role 2 for Professor
const professors = computed(() => 
  users.value.filter(user => user.role === 2 && user.isActive)
)

// Since professors aren't pre-assigned to subjects in DB, show all professors
const availableProfessors = computed(() => professors.value)

watch(() => props.group, (newGroup) => {
  if (newGroup) {
    form.value = {
      name: newGroup.name ?? '',
      subjectId: newGroup.subjectId ?? 0,
      professorId: newGroup.professorId ?? 0,
      academicYearId: newGroup.academicYearId ?? 0,
      isActive: newGroup.isActive ?? true
    }
  } else {
    form.value = {
      name: '',
      subjectId: 0,
      professorId: 0,
      academicYearId: 0,
      isActive: true
    }
  }
}, { immediate: true })

onMounted(async () => {
  if (!users.value.length) {
    await userStore.fetchUsers({ role: 2, isActive: true })
  }
  if (!academicYears.value.length) {
    await academicYearStore.fetchAcademicYears()
  }
})

async function handleSubmit() {
  try {
    isLoading.value = true
    emit('submit', {
      name: form.value.name,
      subjectId: form.value.subjectId,
      professorId: form.value.professorId,
      academicYearId: form.value.academicYearId,
      isActive: form.value.isActive
    })
  } finally {
    isLoading.value = false
  }
}
</script>