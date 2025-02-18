import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import LoginView from '../views/LoginView.vue'
import { useSettingsStore } from '@/stores/settings'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/login',
      name: 'login',
      component: LoginView
    }
  ],
})

router.beforeEach((to, from, next) => {
  next()
  return
  const settingsStore = useSettingsStore()
  if (to.name !== 'login' && !settingsStore.user) {
    next({ name: 'login' })
  } else {
    next()
  }
})

export default router
