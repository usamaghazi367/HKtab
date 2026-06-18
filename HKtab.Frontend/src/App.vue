<script setup lang="ts">
import { ref } from 'vue'
import DashboardView from './components/DashboardView.vue'
import { useAuthStore } from './stores/authStore'
import apiClient from './services/apiClient'

const authStore = useAuthStore()
const email = ref('')
const password = ref('')
const firstName = ref('')
const lastName = ref('')
const isRegister = ref(false)
const error = ref('')
const loading = ref(false)

const handleSubmit = async () => {
  error.value = ''
  loading.value = true
  try {
    const response = isRegister.value
      ? await apiClient.register({
          email: email.value,
          password: password.value,
          firstName: firstName.value,
          lastName: lastName.value,
        })
      : await apiClient.login({ email: email.value, password: password.value })

    authStore.setAuth(response.data.token, response.data.user)
  } catch (err: any) {
    error.value = err.response?.data?.message || 'Authentication failed'
  } finally {
    loading.value = false
  }
}

const handleLogout = () => {
  authStore.clearAuth()
}
</script>

<template>
  <div class="min-h-screen bg-gray-100 p-4">
    <div class="max-w-md mx-auto">
      <h1 class="text-2xl font-bold text-center text-gray-800 mb-6">HKtab Finance</h1>

      <div v-if="!authStore.isAuthenticated" class="bg-white rounded-lg shadow-md p-6 space-y-4">
        <div class="flex gap-2">
          <button
            type="button"
            :class="['flex-1 py-2 rounded', !isRegister ? 'bg-blue-600 text-white' : 'bg-gray-200']"
            @click="isRegister = false"
          >
            Login
          </button>
          <button
            type="button"
            :class="['flex-1 py-2 rounded', isRegister ? 'bg-blue-600 text-white' : 'bg-gray-200']"
            @click="isRegister = true"
          >
            Register
          </button>
        </div>

        <form class="space-y-3" @submit.prevent="handleSubmit">
          <template v-if="isRegister">
            <input v-model="firstName" type="text" placeholder="First name" required class="w-full border rounded px-3 py-2" />
            <input v-model="lastName" type="text" placeholder="Last name" required class="w-full border rounded px-3 py-2" />
          </template>
          <input v-model="email" type="email" placeholder="Email" required class="w-full border rounded px-3 py-2" />
          <input v-model="password" type="password" placeholder="Password" required class="w-full border rounded px-3 py-2" />
          <p v-if="error" class="text-red-600 text-sm">{{ error }}</p>
          <button type="submit" :disabled="loading" class="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700 disabled:opacity-50">
            {{ loading ? 'Please wait...' : isRegister ? 'Register' : 'Login' }}
          </button>
        </form>
      </div>

      <div v-else class="space-y-4">
        <div class="flex justify-between items-center">
          <p class="text-gray-700">Welcome, {{ authStore.user?.firstName }}</p>
          <button type="button" class="text-sm text-red-600" @click="handleLogout">Logout</button>
        </div>
        <DashboardView />
      </div>
    </div>
  </div>
</template>
