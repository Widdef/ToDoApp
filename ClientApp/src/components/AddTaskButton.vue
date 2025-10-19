<template>
  <div>
    <button
      class="btn btn-success d-flex align-items-center"
      :disabled="loading"
      @click="addTask"
    >
      <span v-if="!loading">➕ Dodaj zadanie</span>
      <span v-else class="spinner-border spinner-border-sm me-2"></span>
      <span v-if="loading">Wysyłanie...</span>
    </button>

    <!-- komunikaty -->
    <div v-if="message" class="mt-2 small" :class="messageClass">
      {{ message }}
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'

const props = defineProps({
  apiUrl: {
    type: String,
    default: '/api/tasks'
  },
  defaultTask: {
    type: Object,
    default: () => ({
      name: 'Nowe zadanie',
      description: '',
      dueDate: null,
      priority: 'Normalny'
    })
  }
})

const emit = defineEmits(['task-added'])

const loading = ref(false)
const message = ref('')
const messageType = ref('') // 'success' / 'error'

const messageClass = computed(() => ({
  'text-success': messageType.value === 'success',
  'text-danger': messageType.value === 'error'
}))

async function addTask() {
  loading.value = true
  message.value = ''

  try {
    const response = await fetch(props.apiUrl, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(props.defaultTask)
    })

    if (!response.ok) throw new Error(`Błąd: ${response.status}`)

    const created = await response.json()

    message.value = 'Zadanie zostało dodane!'
    messageType.value = 'success'
    emit('task-added', created) // informacja zwrotna
  } catch (err) {
    console.error(err)
    message.value = 'Nie udało się dodać zadania.'
    messageType.value = 'error'
  } finally {
    loading.value = false
    setTimeout(() => (message.value = ''), 3000)
  }
}
</script>

<style scoped>
button {
  min-width: 160px;
}
</style>