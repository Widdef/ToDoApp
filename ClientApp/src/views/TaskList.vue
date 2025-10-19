<script setup>
import TaskElement from '../components/TaskElement.vue';
import AddTaskButton from '../components/AddTaskButton.vue';
import DateRangeInput from '../components/DateRangeInput.vue';

function onTaskAdded(task) {
  console.log('Nowe zadanie:', task)
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
          v-model:from="dateFrom" 
          v-model:to="dateTo" 
          @change="onDateRangeChange" 
        />
        <div class="mt-3">
          <AddTaskButton  
            api-url=""
            @task-added="onTaskAdded" 
          />
        </div>
      </div>
    </div>
    <TaskElement
      name="task.name"
      due-date="2024-12-31"
      estimated-time="task.eta"
      description="task.descriptiondsbauydia yudhbasyudhaios duhjasiudnaiusondiuoashbdiuoashdiuasndiuoasndiuoansiudobna siudhgasiyudba
      dasdja ioiaodjio asjidoasjpdjp ioasjiopdajiopsdiojp asjiopdjiopasjio dpaj jiosdaouidaiu ashduiuisahd AGHY8QUWGRHASDPH YHE78912HDSAIUHBDIASB sodjio asjiopsiudhaidasdasudasiopdj ajsdioajdigher sdhiousadiuhbfai DSAFSD EWTWEF  fgsdfgsdf ######## u"
    />
  </div>
</template>

<style scoped>

</style>
