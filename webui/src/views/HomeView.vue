<script setup lang="ts"></script>
<template>
  <n-layout has-sider class="full bfc">
    <n-layout-sider width="200" theme="dark" class="bfc">
      <div class="sider bfc">
        <div style="margin: 2px 4px; height: 2.5em">
          <n-button type="default" @click="() => {}" style="width: 100%; height: 100%"
            >Button 1</n-button
          >
        </div>
        <n-scrollbar style="height: 100%" content-style="margin: 0 4px">
          <n-list :show-divider="false">
            <n-list-item v-for="item in 100" :key="item" style="height: 2.5em"> 1 </n-list-item>
          </n-list>
        </n-scrollbar>
      </div>
    </n-layout-sider>
    <n-layout>
      <n-layout-header style="height: 2.5em">
        <n-flex justify="space-between" align="center" style="height: 100%">
          <h4>Model Name</h4>
          <n-button type="default" @click="() => {}">Button 2</n-button>
        </n-flex>
      </n-layout-header>
      <n-layout-content class="full bfc">
        
      </n-layout-content>
    </n-layout>
  </n-layout>
</template>

<style scoped>
.full {
  height: 100%;
  width: 100%;
}

.sider {
  height: 100%;
  width: 100%;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.bfc {
  overflow: hidden;
}
</style>
