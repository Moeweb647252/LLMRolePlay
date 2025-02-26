import { createRouter, createWebHistory } from 'vue-router'
import Main from '@/Main.vue'
import HomeView from '../views/HomeView.vue'
import LoginView from '../views/LoginView.vue'
import SettingsView from '@/views/SettingsView.vue'
import UserSettings from '@/components/settings/UserSettings.vue'
import PresetSettings from '@/components/settings/PresetSettings.vue'
import CharacterSettings from '@/components/settings/CharacterSettings.vue'
import GeneralSettings from '@/components/settings/GeneralSettings.vue'
import ProviderSettings from '@/components/settings/ProviderSettings.vue'
import TemplateSettings from '@/components/settings/TemplateSettings.vue'
import { useSettingsStore } from '@/stores/settings'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      redirect: '/main',
    },
    {
      path: '/main',
      component: Main,
      children: [
        {
          path: '',
          name: 'home',
          component: HomeView,
        },
        {
          path: 'settings',
          name: 'settings',
          component: SettingsView,
          children: [
            {
              path: '',
              redirect: '/main/settings/user',
            },
            {
              path: 'user',
              component: UserSettings,
            },
            {
              path: 'preset',
              component: PresetSettings,
            },
            {
              path: 'character',
              component: CharacterSettings,
            },
            {
              path: 'general',
              component: GeneralSettings,
            },
            {
              path: 'provider',
              component: ProviderSettings,
            },
            {
              path: 'template',
              component: TemplateSettings,
            },
          ],
        },
      ],
    },
    {
      path: '/login',
      name: 'login',
      component: LoginView,
    },
  ],
})

router.beforeEach((to, from, next) => {
  next()
  const settingsStore = useSettingsStore()
  if (to.name !== 'login' && !settingsStore.user) {
    next({ name: 'login' })
  } else {
    next()
  }
})

export default router
