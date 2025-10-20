<script setup>
import { onMounted, reactive, ref } from 'vue';
import api from '@/api/axios';
import TaskElement from '../components/TaskElement.vue';
import DateRangeInput from '../components/DateRangeInput.vue';

const state = reactive({
  tasks: [],
  dateFrom: null,
  dateTo: null,
});

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
      completed: true,
    },
  }).then(response => {
    state.tasks = response.data.items;
    console.log('Pobrane zadania:', state.tasks);
    console.log('Response:', response);
  }).catch(error => {
    console.error('Błąd podczas pobierania zadań:', error);
  }); 
}
</script>

<template>
  <div class="container">
    <div class="d-flex justify-content-between align-items-center mb-0">
      <div class="mb-0">
        <h1 class="mb-0 text-start">Historia zadań</h1>
        <p class="mb-0 text-muted">Ta strona wyświetla wszystkie ukończone zadania.</p>
      </div>
      <div class="d-flex flex-column align-items-end mb-4">
        <DateRangeInput 
          v-model:from="state.dateFrom" 
          v-model:to="state.dateTo" 
          @change="onDateRangeChange" 
        />
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
      :history="true"
    />
  </div>
</template>

<style scoped>

</style>