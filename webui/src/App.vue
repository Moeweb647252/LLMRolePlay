<script setup lang="ts">
import Main from './Main.vue'
</script>

<template>
  <Suspense>
    <Main></Main>
    <template #fallback>
      <n-spin tip="Loading..." />
    </template>
  </Suspense>
</template>

<style scoped></style>
