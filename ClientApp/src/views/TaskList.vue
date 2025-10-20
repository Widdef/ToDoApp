<script setup>
import { onMounted, reactive, ref } from 'vue';
import api from '@/api/axios';
import TaskElement from '../components/TaskElement.vue';
import AddTaskButton from '../components/AddTaskButton.vue';
import DateRangeInput from '../components/DateRangeInput.vue';
import AddTaskForm from '../components/AddTaskForm.vue';
import EditTaskForm from '../components/EditTaskForm.vue';

const state = reactive({
  tasks: [],
  dateFrom: null,
  dateTo: null,
});
const showAdd = ref(false)
const showEdit = ref(false) 
const editTask = ref(null)

onMounted(() => {
  fetchTasks();
});

function onDateRangeChange({ from, to }) {
  state.dateFrom = from;
  state.dateTo = to;
  fetchTasks();
  console.log('Zakres dat zmieniony:', from, to);
}

function fetchTasks() {
  api.get('/tasks', {
    params: {
      from: state.dateFrom || undefined,
      to: state.dateTo || undefined,
      completed: false,
    },
  }).then(response => {
    state.tasks = response.data.items;
    console.log('Pobrane zadania:', state.tasks);
    console.log('Response:', response);
  }).catch(error => {
    console.error('Błąd podczas pobierania zadań:', error);
  }); 
}

const onComplete = async (task) => {
  if (!task.id) return
  try {
    const payload = {
      isCompleted: true
    }
    await api.patch(`/tasks/${task.id}`, payload)
  } catch (e) {
    error.value = e?.message ?? 'Nie udało się ukończyć zadania'
  } finally {
    console.log('Zadanie ukończone')
    fetchTasks()
  }
}

function onTaskAdded(task) {
  state.tasks.unshift(task)   // dodaj na górę listy
  showAdd.value = false
  console.log('Nowe zadanie:', task)
}
function onTaskEdited(updated) {
  const i = state.tasks.findIndex(t => t.id === updated.id)
  if (i !== -1) state.tasks[i] = { ...state.tasks[i], ...updated }
  showEdit.value = false
  editTask.value = null
}

async function handleDelete(id) {
  try {
    await api.delete(`/tasks/${id}`)
    // usuń z lokalnego stanu, żeby UI się odświeżył
    state.tasks = state.tasks.filter(t => t.id !== id)
  } catch (err) {
    console.log('Usuwanie zadania nie powiodło się', id)
    console.error('Błąd przy usuwaniu zadania:', err)
  }
}

</script>

<template>
  <div class="container">
    <div class="d-flex justify-content-between align-items-center mb-0">
      <div class="mb-0">
        <h1 class="mb-0 text-start">Lista zadań</h1>
        <p class="mb-0 text-muted">Ta strona wyświetla wszystkie zadania.</p>
      </div>
      <div class="d-flex flex-column align-items-end mb-4">
        <DateRangeInput 
          v-model:from="state.dateFrom" 
          v-model:to="state.dateTo" 
          @change="onDateRangeChange" 
        />
        <div class="mt-3">
          <AddTaskButton  
            @show-form="showAdd = true" 
          />
        </div>
      </div>
    </div>
    <TaskElement
      v-for="task in state.tasks"
      :id="task.id"
      :name="task.title"
      :dueDate="task.dueDate"
      :estimatedTime="task.estimatedTimeMinutes"
      :description="task.description"
      :priority="task.priority"
      @done="onComplete(task)"
      @delete="handleDelete"
      @edit="{
        showEdit = true;
        editTask = task;
        console.log('Edycja zadania:', task, showEdit, editTask);
      }"
    />
    <AddTaskForm 
        v-if="showAdd" 
        @created="onTaskAdded" 
        @close="showAdd = false"
    />
    <EditTaskForm 
      v-if="showEdit && editTask"
      :task="editTask"
      :show="showEdit"
      @save="onTaskEdited"
      @cancel="{ showEdit = false; editTask = null }"
    />
  </div>
</template>

<style scoped>

</style>