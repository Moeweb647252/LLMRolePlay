<script setup lang="ts">
import Main from './Main.vue'
</script>

<template>
  <n-message-provider>
    <Suspense>
      <RouterView />
      <template #fallback>
        <div
          style="
            width: 100%;
            height: 100%;
            display: flex;
            align-items: center;
            justify-content: center;
          "
        >
          <n-spin tip="Loading..." />
        </div>
      </template>
    </Suspense>
  </n-message-provider>
</template>

<style scoped></style>
