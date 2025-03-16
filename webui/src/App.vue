<script setup lang="ts">
import { RouterView } from 'vue-router'
import { NMessageProvider, NModalProvider, NSpin } from 'naive-ui'
</script>

<template>
  <NMessageProvider>
    <NModalProvider>
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
            <NSpin tip="Loading..." />
          </div>
        </template>
      </Suspense>
    </NModalProvider>
  </NMessageProvider>
</template>

<style scoped></style>
