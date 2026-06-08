<template>
  <div class="bg-white rounded-lg shadow-md p-4 space-y-4">
    <!-- Summary Cards -->
    <div class="grid grid-cols-3 gap-2">
      <div class="bg-green-100 rounded-lg p-3 text-center">
        <p class="text-xs text-gray-600">Income</p>
        <p class="text-lg font-bold text-green-600">{{ formatCurrency(dashboard.totalIncome) }}</p>
      </div>
      <div class="bg-red-100 rounded-lg p-3 text-center">
        <p class="text-xs text-gray-600">Expense</p>
        <p class="text-lg font-bold text-red-600">{{ formatCurrency(dashboard.totalExpense) }}</p>
      </div>
      <div :class="['rounded-lg p-3 text-center', dashboard.balance >= 0 ? 'bg-blue-100' : 'bg-yellow-100']">
        <p class="text-xs text-gray-600">Balance</p>
        <p :class="['text-lg font-bold', dashboard.balance >= 0 ? 'text-blue-600' : 'text-yellow-600']">
          {{ formatCurrency(dashboard.balance) }}
        </p>
      </div>
    </div>

    <!-- Top Categories -->
    <div v-if="dashboard.topCategories.length > 0" class="border-t pt-4">
      <h3 class="font-bold text-gray-800 mb-2">Top Categories</h3>
      <div class="space-y-2">
        <div v-for="cat in dashboard.topCategories" :key="cat.category" class="flex justify-between text-sm">
          <span class="text-gray-700">{{ cat.category }}</span>
          <span class="font-semibold text-gray-900">{{ formatCurrency(cat.amount) }}</span>
        </div>
      </div>
    </div>

    <!-- Recent Transactions -->
    <div v-if="dashboard.recentTransactions.length > 0" class="border-t pt-4">
      <h3 class="font-bold text-gray-800 mb-2">Recent Transactions</h3>
      <div class="space-y-2 max-h-48 overflow-y-auto">
        <div v-for="txn in dashboard.recentTransactions" :key="`${txn.type}-${txn.id}`" class="flex justify-between text-sm">
          <div class="flex-1">
            <p class="text-gray-800">{{ txn.description }}</p>
            <p class="text-xs text-gray-500">{{ new Date(txn.date).toLocaleDateString() }}</p>
          </div>
          <p :class="['font-semibold', txn.type === 'Income' ? 'text-green-600' : 'text-red-600']">
            {{ txn.type === 'Income' ? '+' : '-' }}{{ formatCurrency(txn.amount) }}
          </p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import apiClient from '@/services/apiClient'

interface Dashboard {
  totalIncome: number
  totalExpense: number
  balance: number
  transactionCount: number
  topCategories: any[]
  recentTransactions: any[]
}

const dashboard = ref<Dashboard>({
  totalIncome: 0,
  totalExpense: 0,
  balance: 0,
  transactionCount: 0,
  topCategories: [],
  recentTransactions: []
})

const formatCurrency = (amount: number) => {
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD'
  }).format(amount)
}

onMounted(async () => {
  try {
    const response = await apiClient.getDashboardSummary()
    dashboard.value = response.data
  } catch (error) {
    console.error('Failed to load dashboard:', error)
  }
})
</script>
