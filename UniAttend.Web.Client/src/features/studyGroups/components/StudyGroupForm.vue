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
          {{ professor.fullName }}
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
import { useProfessorStore } from '@/stores/professor.store'
import { useAcademicYearStore } from '@/stores/academicYear.store'
import Button from '@/shared/components/ui/Button.vue'
import type { 
  StudyGroupDto,
  CreateStudyGroupCommand,
  UpdateStudyGroupCommand,
  SubjectDto
} from '@/api/generated/data-contracts'

interface Props {
  studyGroup?: StudyGroupDto | null
  subjects: SubjectDto[]
}

interface FormData {
  name: string
  subjectId: number | undefined
  professorId: number | undefined
  academicYearId: number | undefined
  isActive: boolean
}

const props = defineProps<Props>()
const emit = defineEmits<{
  (e: 'submit', data: CreateStudyGroupCommand | UpdateStudyGroupCommand): void
  (e: 'cancel'): void
}>()

// Store setup
const professorStore = useProfessorStore()
const academicYearStore = useAcademicYearStore()

const { professors } = storeToRefs(professorStore)
const { academicYears } = storeToRefs(academicYearStore)

const isLoading = ref(false)
const form = ref<FormData>({
  name: '',
  subjectId: undefined,
  professorId: undefined,
  academicYearId: undefined,
  isActive: true
})

// Show only active professors
const availableProfessors = computed(() => 
  professors.value.filter(p => p.isActive)
)

// Watch for changes in props.group and update form
watch(() => props.studyGroup, (newGroup) => {
  if (newGroup) {
    form.value = {
      name: newGroup.name ?? '', // Use nullish coalescing
      subjectId: newGroup.subjectId,
      professorId: newGroup.professorId,
      academicYearId: newGroup.academicYearId,
      isActive: newGroup.isActive ?? true // Use nullish coalescing
    }
  } else {
    form.value = {
      name: '',
      subjectId: undefined,
      professorId: undefined,
      academicYearId: undefined,
      isActive: true
    }
  }
}, { immediate: true })

onMounted(async () => {
  // Load required data if not already loaded
  if (!professors.value.length) {
    await professorStore.fetchProfessors()
  }
  if (!academicYears.value.length) {
    await academicYearStore.fetchAcademicYears()
  }
})

async function handleSubmit() {
  try {
    if (!form.value.subjectId || !form.value.professorId || !form.value.academicYearId) {
      return // Early return if required fields are missing
    }

    isLoading.value = true
    const formData = {
      name: form.value.name,
      subjectId: form.value.subjectId,
      professorId: form.value.professorId,
      academicYearId: form.value.academicYearId,
      isActive: form.value.isActive
    }

    if (props.studyGroup?.id) {
      emit('submit', { 
        id: props.studyGroup.id,
        ...formData 
      } as UpdateStudyGroupCommand)
    } else {
      emit('submit', formData as CreateStudyGroupCommand)
    }
  } finally {
    isLoading.value = false
  }
}
</script>