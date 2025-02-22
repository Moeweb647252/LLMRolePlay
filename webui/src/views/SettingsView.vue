<script setup lang="ts">
const links = [
  { name: '返回', path: '/' },
  { name: '通用', path: '/main/settings/general' },
  { name: '用户', path: '/main/settings/user' },
  { name: '角色', path: '/main/settings/character' },
  { name: '预设', path: '/main/settings/preset' },
  { name: 'Provider', path: '/main/settings/provider' },
]
</script>

<template>
  <n-layout has-sider style="height: 100%">
    <n-layout-sider>
      <div class="sider">
        <RouterLink v-for="link in links" :to="link.path" active-class="link-active" class="link">{{
          link.name
        }}</RouterLink>
      </div>
    </n-layout-sider>
    <n-layout-content>
      <RouterView style="padding: 2em" />
    </n-layout-content>
  </n-layout>
</template>

<style scoped>
.sider {
  display: flex;
  flex-direction: column;
  gap: 1rem;
  padding: 1rem;
  border-right: 1px solid #e9e9e9;
}

.link {
  color: #000;
  text-decoration: none;
  padding: 0.5rem;
  border-radius: 4%;
}

.link:hover {
  background-color: #cccccc;
}

.link-active {
  background-color: #cccccc;
}
</style>
