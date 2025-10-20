import { createRouter, createWebHistory } from 'vue-router'
import TaskList from '@/views/TaskList.vue'
import TaskHistory from '@/views/TaskHistory.vue'
import TaskUpcoming from '@/views/TaskUpcoming.vue'
//import TaskProject from '@/views/TaskProject.vue'
//import TaskStats from '@/views/TaskStats.vue'

const routes = [
  { path: '/', component: TaskList },
  { path: '/history', component: TaskHistory },
  { path: '/upcoming', component: TaskUpcoming },
  //{ path: '/projects', component: TaskProject },
  //{ path: '/stats', component: TaskStats }
]

export default createRouter({
  history: createWebHistory(),
  routes
})