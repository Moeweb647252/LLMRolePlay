import { createRouter, createWebHashHistory } from 'vue-router'
import Main from '@/MainView.vue'
import HomeView from '../views/HomeView.vue'
import LoginView from '../views/LoginView.vue'
import SettingsView from '@/views/SettingsView.vue'
import UserSettings from '@/components/settings/UserSettings.vue'
import PresetSettings from '@/components/settings/PresetSettings.vue'
import CharacterSettings from '@/components/settings/CharacterSettings.vue'
import GeneralSettings from '@/components/settings/GeneralSettings.vue'
import ProviderSettings from '@/components/settings/ProviderSettings.vue'
import TemplateSettings from '@/components/settings/TemplateSettings.vue'
import TranslateView from '@/views/TranslateView.vue'
import { useSettingsStore } from '@/stores/settings'

const router = createRouter({
  history: createWebHashHistory(),
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
          redirect: '/main/settings/user',
          children: [
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
        {
          path: 'translate',
          component: TranslateView,
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
  console.log(to.path)
  console.log(from.path)
  const settingsStore = useSettingsStore()
  if (to.name !== 'login' && !settingsStore.user) {
    next({ name: 'login' })
  } else {
    next()
  }
})

export default router
