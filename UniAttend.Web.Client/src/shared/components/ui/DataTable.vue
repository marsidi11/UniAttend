<template>
    <div class="overflow-x-auto">
      <table class="min-w-full divide-y divide-gray-200">
        <!-- Table Header -->
        <thead class="bg-gray-50">
          <tr>
            <th
              v-for="column in columns"
              :key="column.key"
              scope="col"
              class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider"
            >
              {{ column.label }}
            </th>
            <th v-if="actions && actions.length" scope="col" class="relative px-6 py-3">
              <span class="sr-only">Actions</span>
            </th>
          </tr>
        </thead>
  
        <!-- Table Body -->
        <tbody class="bg-white divide-y divide-gray-200">
          <tr v-if="loading" class="animate-pulse">
            <td :colspan="columns.length + (actions?.length || 0)" class="px-6 py-4">
              <div class="flex justify-center">
                <Spinner :size="6" />
              </div>
            </td>
          </tr>
  
          <tr
            v-else-if="data.length === 0"
            class="text-center text-gray-500"
          >
            <td :colspan="columns.length + (actions?.length || 0)" class="px-6 py-4">
              No data available
            </td>
          </tr>
  
          <tr
            v-else
            v-for="item in data"
            :key="item.id"
            @click="handleRowClick(item)"
            class="hover:bg-gray-50 cursor-pointer"
          >
            <td
              v-for="column in columns"
              :key="column.key"
              class="px-6 py-4 whitespace-nowrap text-sm text-gray-900"
            >
              {{ column.render ? column.render(item[column.key]) : item[column.key] }}
            </td>
            
            <!-- Action Buttons -->
            <td v-if="actions && actions.length" class="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
              <Button
                v-for="action in actions"
                :key="action.label"
                variant="secondary"
                class="ml-2"
                @click.stop="action.action(item)"
              >
                {{ action.label }}
              </Button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </template>
  
  <script setup lang="ts">
  import Button from './Button.vue'
  import Spinner from './Spinner.vue'
  import type { TableItem, Column, Action } from '@/types/tableItem.types'
  
  defineProps<{
  data: TableItem[]
  columns: Column<TableItem>[]
  loading?: boolean
  actions?: Action<TableItem>[]
}>()
  
  const emit = defineEmits<{
    (e: 'row-click', item: TableItem): void
  }>()
  
  function handleRowClick(item: TableItem) {
    emit('row-click', item)
  }
  </script>