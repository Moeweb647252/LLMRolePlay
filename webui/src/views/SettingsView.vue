<script setup lang="ts"></script>

<template>
  <n-layout has-sider>
    <n-layout-sider>
      <div class="sider">
        <RouterLink to="/settings/general">通用</RouterLink>
        <RouterLink to="/settings/user">用户</RouterLink>
        <RouterLink to="/settings/character">角色</RouterLink>
        <RouterLink to="/settings/preset">预设</RouterLink>
        <RouterLink to="/settings/provider">Provider</RouterLink>
      </div>
    </n-layout-sider>
    <n-layout-content>
      <RouterView />
    </n-layout-content>
  </n-layout>
</template>

<style scoped>
.sider {
  display: flex;
  flex-direction: column;
  gap: 1rem;
  padding: 1rem;
  background-color: #f0f2f5;
  height: 100%;
  overflow-y: auto;
  border-right: 1px solid #e9e9e9;
}
</style>
