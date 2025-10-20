<script setup lang="ts">
import { ref, onMounted, onBeforeUnmount } from 'vue'
import api from '@/api/axios'

const emit = defineEmits<{
  (e: 'close'): void
  (e: 'created', task: any): void
}>()

// model formularza
const form = ref({
  title: '',
  description: '',
  dueDate: '',                 // ISO 'YYYY-MM-DD' albo ''
  estimatedTimeMinutes: 0,
  priority: 1,                 // 0=Low,1=Medium,2=High
  category: ''
})
const saving = ref(false)
const error = ref<string | null>(null)

function escToClose(e: KeyboardEvent){
  if (e.key === 'Escape') emit('close')
}

onMounted(() => {
  document.body.style.overflow = 'hidden' // blok scrolla pod spodem
  window.addEventListener('keydown', escToClose)
})
onBeforeUnmount(() => {
  document.body.style.overflow = ''
  window.removeEventListener('keydown', escToClose)
})

async function submit() {
  if (!form.value.title.trim()) {
    error.value = 'Tytuł jest wymagany'
    return
  }
  saving.value = true
  error.value = null
  try {
    const payload = {
      title: form.value.title.trim(),
      description: form.value.description || null,
      dueDate: form.value.dueDate || null,
      estimatedTimeMinutes: Number(form.value.estimatedTimeMinutes) || 0,
      priority: Number(form.value.priority),
      category: form.value.category || null,
      isCompleted: false
    }
    const res = await api.post('/tasks', payload)
    emit('created', res.data)
  } catch (e:any) {
    error.value = e?.message ?? 'Nie udało się dodać zadania'
  } finally {
    saving.value = false
  }
}
</script>

<template>
  <!-- tło -->
  <div class="modal-backdrop" @click.self="emit('close')">
    <!-- okno -->
    <div class="modal-window" role="dialog" aria-modal="true">
      <header class="modal-header">
        <h2>Dodaj zadanie</h2>
        <button class="icon" @click="emit('close')" aria-label="Zamknij">×</button>
      </header>

      <form @submit.prevent="submit" class="modal-body">
        <label>
          Tytuł* 
          <input v-model="form.title" required placeholder="np. Zadzwonić do klienta" />
        </label>

        <label>
          Opis
          <textarea v-model="form.description" rows="3" />
        </label>

        <div class="grid">
          <label>
            Termin
            <input type="date" v-model="form.dueDate" />
          </label>
          <label>
            Szac. czas (min)
            <input type="number" min="0" v-model.number="form.estimatedTimeMinutes" />
          </label>
        </div>

        <div class="grid">
          <label>
            Priorytet
            <select v-model.number="form.priority">
              <option :value="0">Niski</option>
              <option :value="1">Średni</option>
              <option :value="2">Wysoki</option>
            </select>
          </label>
          <label>
            Kategoria
            <input v-model="form.category" placeholder="np. Praca" />
          </label>
        </div>

        <p v-if="error" class="error">{{ error }}</p>

        <footer class="modal-footer">
          <button class="btn btn-warning" type="button" @click="emit('close')">Anuluj</button>
          <button class="btn btn-primary" type="submit" :disabled="saving">{{ saving ? 'Zapisywanie…' : 'Zapisz' }}</button>
        </footer>
      </form>
    </div>
  </div>
</template>

<style scoped>
.modal-backdrop{
  position: fixed; inset: 0;
  background: rgba(0,0,0,.5);
  display:flex; align-items:center; justify-content:center;
  padding: 1rem; z-index: 9999;
}
.modal-window{
  width: min(720px, 100%); background: #fff; border-radius: 14px;
  box-shadow: 0 10px 30px rgba(0,0,0,.2); overflow: hidden;
}
.modal-header{
  display:flex; align-items:center; justify-content:space-between;
  padding: .9rem 1.1rem; border-bottom: 1px solid #eee;
}
.modal-body{ padding: 1rem 1.1rem; display:grid; gap:.85rem; }
.modal-footer{ display:flex; justify-content:flex-end; gap:.5rem; margin-top:.5rem; }
.grid{ display:grid; grid-template-columns: 1fr 1fr; gap:.75rem; }
.icon{ background: transparent; border:none; font-size:1.4rem; line-height:1; cursor:pointer; color: black; }
.error{ color:#c0392b; }
input, select, textarea{ width:100%; padding:.55rem .65rem; border:1px solid #ddd; border-radius:8px; }
button{ padding:.55rem .9rem; border-radius:10px; border:1px solid #ddd; background:#f8f8f8; cursor:pointer; }
button[type="submit"]{ background:#0ea5e9; color:#fff; border-color:#0ea5e9; }
button[disabled]{ opacity:.7; cursor:not-allowed; }
</style>