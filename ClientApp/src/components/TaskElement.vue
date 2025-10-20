<template>
  <div class="card shadow-sm mb-3 task-item">
    <div class="card-body">
      <div class="d-flex justify-content-between align-items-center flex-wrap mb-2">
        <!-- Nazwa zadania -->
          <h3 class="mb-0 text-start flex-grow-1">{{ name }}</h3>
        <!-- Przyciski -->
        <button
          v-if="!history"
          type="button"
          class="btn btn-outline-primary btn-square me-2"
          @click="$emit('done')"
          aria-label="Oznacz jako wykonane"
          title="Oznacz jako wykonane"
        >
          ✓
        </button>

        <button
          v-if="!history"
          type="button"
          class="btn btn-primary me-2"
          @click="$emit('edit')"
        >
          Edytuj
        </button>

        <button
          v-if="!history"
          type="button"
          class="btn btn-outline-danger"
          @click="onDelete"
        >
          Usuń
        </button>
      </div>
    </div>
    <div class="card-body pt-0">
      <div class="row g-3 align-items-left">
        <!-- Data -->
        <div class="col-3 col-md-3 col-lg-2">
          <small class="text-secondary d-block">Data</small>
          <span>{{ formatDate(dueDate) }}</span>
        </div>
        <!-- Szacowany czas -->
        <div class="col-3 col-md-3 col-lg-2">
          <small class="text-secondary d-block">Przewidywany czas (min)</small>
          <span>{{ estimatedTime }}</span>
        </div>
        <!-- Priorytet -->
        <div class="col-3 col-md-3 col-lg-1">
          <small class="text-secondary d-block">Priorytet</small>
          <span class="badge" :class="priorityClass">{{ priorityName }}</span>
        </div>
      </div>

      <!-- Opis -->
      <div class="mt-3 text-start">
        <p v-if="!isLong || expanded" class="mb-1 preserve-newlines">
          {{ description }}
        </p>
        <p v-else class="mb-1 clamp-2 preserve-newlines">
          {{ description }}
        </p>

        <button
          v-if="isLong"
          type="button"
          class="btn btn-link p-0"
          @click="expanded = !expanded"
        >
          {{ expanded ? 'Zwiń' : 'Rozwiń' }}
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'

const props = defineProps({
  id: { type: Number, required: true },
  name: { type: String, required: true },
  dueDate: { type: [String, Date], default: null },
  estimatedTime: { type: [String, Number], default: '' },
  priority: { type: Number, default: 1, validator: v => [null,-1, 0, 1, 2].includes(Number(v)) },
  description: { type: String, default: '' },
  history: { type: Boolean, default: false },
})
const longThreshold = 120
const emit = defineEmits(['done', 'edit', 'delete'])

const expanded = ref(false)
const isLong = computed(() => (props.description?.length || 0) > longThreshold)

const priorityName = ['Niski', 'Średni', 'Wysoki'][props.priority] || 'Nieznany';

const priorityClass = computed(() => {
  const p = priorityName.toLowerCase()
  if (['wysoki', 'high'].includes(p)) return 'bg-danger'
  if (['średni', 'sredni', 'medium'].includes(p)) return 'bg-warning text-dark'
  return 'bg-secondary'
})

// Formatowanie daty z formatu bazowego do formatu PL
function formatDate(d) {
  if (!d) return '—'
  const dt = new Date(d)
  // krótki format PL
  return dt.toLocaleDateString('pl-PL', { year: 'numeric', month: '2-digit', day: '2-digit' })
}

function onDelete() {
  if (window.confirm('Na pewno usunąć to zadanie?')) {
    emit('delete', props.id)
  }
}
</script>

<style scoped>
/* kwadratowy przycisk "done" */
.btn-square {
  width: 2.25rem;
  height: 2.25rem;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  border-radius: .5rem;
}

/* skracanie opisu do 2 linii */
.clamp-2 {
  display: -webkit-box;
  -webkit-line-clamp: 2;
  line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

/* zachowanie nowych linii w opisie */
.preserve-newlines {
  white-space: pre-line;
}

/* lekkie doszlifowanie karty */
.task-item .card-body {
  padding: 1rem 1.25rem;
}
</style>