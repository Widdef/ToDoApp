<script setup lang="ts">
import { reactive, ref, watch, nextTick, onMounted, onBeforeUnmount } from 'vue'
import api from '../api/axios';

type Task = {
  id: number; title: string;
  description?: string|null;
  dueDate?: string|null;           
  estimatedTimeMinutes: number;
  priority: number;                
  category?: string|null;
  isCompleted: boolean;
}

const props = defineProps<{
  task?: Task;     
  show?: boolean;  
}>()

const emit = defineEmits<{
  (e: 'save', payload: any): void
  (e: 'cancel'): void
}>()

const form = reactive({
  id: null as number|null,
  title: '',
  description: '',
  dueDate: '',                     // YYYY-MM-DD
  estimatedTimeMinutes: 0,
  priority: 1,
  category: '',
})
const saving = ref(false)
const error = ref<string | null>(null)

function loadFromTask(t?: Task) {
  if (!t) return
  form.id = t.id ?? null
  form.title = t.title ?? ''
  form.description = t.description ?? ''
  form.dueDate = t.dueDate ? t.dueDate.slice(0,10) : ''
  form.estimatedTimeMinutes = t.estimatedTimeMinutes ?? 0
  form.priority = t.priority ?? 1
  form.category = t.category ?? ''
}
loadFromTask(props.task)
watch(() => props.task, (t) => loadFromTask(t), { immediate: true })

const titleRef = ref<HTMLInputElement|null>(null)
watch(() => props.show, async v => {
  if (v) { await nextTick(); titleRef.value?.focus() }
})

function escToClose(e: KeyboardEvent){
  if (e.key === 'Escape') emit('cancel')
}

onMounted(() => {
  document.body.style.overflow = 'hidden' // blok scrolla pod spodem
  window.addEventListener('keydown', escToClose)
})
onBeforeUnmount(() => {
  document.body.style.overflow = ''
  window.removeEventListener('keydown', escToClose)
})


const onSubmit = async () => {
  if (!form.id) return
  if (!form.title.trim()) {
    error.value = 'Tytuł jest wymagany'
    return
  }
  try {
    const payload = {
      title: form.title.trim(),
      description: form.description || null,
      dueDate: form.dueDate || null,
      estimatedTimeMinutes: Number(form.estimatedTimeMinutes) || 0,
      priority: Number(form.priority),
      category: form.category || null,
    }
    const res = await api.patch(`/tasks/${form.id}`, payload)
    emit('save', res.data)
  } catch (e:any) {
    error.value = e?.message ?? 'Nie udało się dodać zadania'
  } finally {
    saving.value = false
  }
  
}
function onCancel(){ emit('cancel') }
</script>

<template>
  <!-- tło -->
  <div class="modal-backdrop" @click.self="emit('cancel')">
    <!-- okno -->
    <div class="modal-window" role="dialog" aria-modal="true">
      <header class="modal-header">
        <h2>Edytuj zadanie</h2>
        <button class="icon" @click="emit('cancel')" aria-label="Zamknij">×</button>
      </header>
      <form @submit.prevent="onSubmit" class="modal-body">
          <label>
          Tytuł
          <input ref="titleRef" type="text" v-model="form.title" required />
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
              <input type="text" v-model="form.category" />
          </label>
          </div>

          <footer class="modal-footer">
          <button type="button" @click="onCancel">Anuluj</button>
          <button type="submit">Zapisz</button>
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
.icon{ background: transparent; border:none; font-size:1.4rem; line-height:1; cursor:pointer; }
.error{ color:#c0392b; }
input, select, textarea{ width:100%; padding:.55rem .65rem; border:1px solid #ddd; border-radius:8px; }
button{ padding:.55rem .9rem; border-radius:10px; border:1px solid #ddd; background:#f8f8f8; cursor:pointer; }
button[type="submit"]{ background:#0ea5e9; color:#fff; border-color:#0ea5e9; }
button[disabled]{ opacity:.7; cursor:not-allowed; }
</style>