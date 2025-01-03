<template>
  <div class="min-h-screen bg-gray-50 flex flex-col justify-center py-12 sm:px-6 lg:px-8">
    <div class="sm:mx-auto sm:w-full sm:max-w-md">
      <!-- Logo -->
      <router-link to="/" class="flex justify-center">
        <h2 class="text-center text-3xl font-extrabold text-indigo-600">
          UniAttend
        </h2>
      </router-link>
      
      <!-- Page Title -->
      <h2 class="mt-6 text-center text-3xl font-extrabold text-gray-900">
        {{ pageTitle }}
      </h2>
      
      <!-- Subtitle -->
      <p class="mt-2 text-center text-sm text-gray-600">
        {{ pageSubtitle }}
        <router-link
          :to="alternateAuthPath"
          class="font-medium text-indigo-600 hover:text-indigo-500"
        >
          {{ alternateAuthText }}
        </router-link>
      </p>
    </div>

    <div class="mt-8 sm:mx-auto sm:w-full sm:max-w-md">
      <div class="bg-white py-8 px-4 shadow sm:rounded-lg sm:px-10">
        <slot />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRoute } from 'vue-router'

const route = useRoute()

const pageTitle = computed(() => {
  if (route.path === '/login') return 'Sign in to your account'
  if (route.path === '/register') return 'Create your account'
  return 'Authentication'
})

const pageSubtitle = computed(() => {
  if (route.path === '/login') return 'Or '
  if (route.path === '/register') return 'Already have an account? '
  return ''
})

const alternateAuthPath = computed(() => 
  route.path === '/login' ? '/register' : '/login'
)

const alternateAuthText = computed(() => 
  route.path === '/login' ? 'create a new account' : 'sign in to your account'
)
</script>