<script setup lang="ts">
import Main from './Main.vue'
</script>

<template>
  <Suspense>
    <Main></Main>
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
</template>

<style scoped></style>
