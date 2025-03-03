<script setup lang="ts">
import { useSettingsStore } from '@/stores/settings'
import { ref, onMounted, onUnmounted } from 'vue'
import { MenuOutlined, CloseOutlined } from '@vicons/material'

const settings = useSettingsStore()
const collapsed = ref(false)
const isMobile = ref(false)

const links = [
  ...[
    { name: '返回', path: '/' },
    { name: '通用', path: '/main/settings/general' },
    { name: '角色', path: '/main/settings/character' },
    { name: '预设', path: '/main/settings/preset' },
    { name: '模板', path: '/main/settings/template' },
    { name: 'Provider', path: '/main/settings/provider' },
  ],
  ...(settings.user?.group == '2' ? [{ name: '用户', path: '/main/settings/user' }] : []),
]

const checkScreenSize = () => {
  isMobile.value = window.innerWidth <= 768
  collapsed.value = isMobile.value
}

onMounted(() => {
  checkScreenSize()
  window.addEventListener('resize', checkScreenSize)
})

onUnmounted(() => {
  window.removeEventListener('resize', checkScreenSize)
})

const toggleSider = () => {
  collapsed.value = !collapsed.value
}
</script>

<template>
  <div class="menu-toggle" v-if="isMobile" @click="toggleSider">
    <n-icon size="24">
      <component :is="collapsed ? MenuOutlined : CloseOutlined" />
    </n-icon>
  </div>

  <n-layout has-sider style="height: 100%">
    <n-layout-sider
      :collapsed="collapsed"
      :collapsed-width="0"
      :width="200"
      show-trigger="arrow-circle"
      collapse-mode="transform"
      @collapse="collapsed = true"
      @expand="collapsed = false"
      :native-scrollbar="false"
      class="layout-sider"
    >
      <div class="sider">
        <RouterLink
          v-for="link in links"
          :to="link.path"
          active-class="link-active"
          class="link"
          @click="isMobile && (collapsed = true)"
        >
          {{ link.name }}
        </RouterLink>
      </div>
    </n-layout-sider>
    <n-layout-content>
      <RouterView style="padding: 2em" />
    </n-layout-content>
  </n-layout>
</template>

<style scoped>
.layout-sider {
  position: relative;
  height: 100%;
  z-index: 100;
  border-right: 1px solid #e9e9e9;
  transition: all 0.3s ease;
}

.sider {
  display: flex;
  flex-direction: column;
  gap: 1rem;
  padding: 1rem;
  height: 100%;
}

.link {
  color: #000;
  text-decoration: none;
  padding: 0.8rem 0.5rem;
  border-radius: 4px;
  font-size: 1rem;
  display: flex;
  align-items: center;
}

.link:hover {
  background-color: #cccccc;
}

.link-active {
  background-color: #cccccc;
}

.menu-toggle {
  position: fixed;
  top: 10px;
  right: 10px;
  z-index: 1000;
  background-color: rgba(255, 255, 255, 0.8);
  border-radius: 50%;
  width: 42px;
  height: 42px;
  display: flex;
  align-items: center;
  justify-content: center;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);
  cursor: pointer;
}

@media (max-width: 768px) {
  .link {
    padding: 1rem 0.8rem;
    font-size: 1.1rem;
  }

  .sider {
    padding-top: 3rem;
  }
}
</style>
