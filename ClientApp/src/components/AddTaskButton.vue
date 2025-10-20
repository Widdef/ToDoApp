<template>
  <div>
    <button
      class="btn btn-success d-flex align-items-center"
      :disabled="loading"
      @click="showForm"
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

const emit = defineEmits(['show-form'])

const loading = ref(false)
const message = ref('')
const messageType = ref('') // 'success' / 'error'

const messageClass = computed(() => ({
  'text-success': messageType.value === 'success',
  'text-danger': messageType.value === 'error'
}))

function showForm() {
  // emit do rodzica – informacja, że ma pokazać formularz
  console.log('Emituję show-form')
  emit('show-form')
}

</script>

<style scoped>
button {
  min-width: 160px;
}
</style>