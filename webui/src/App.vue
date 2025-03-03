<script setup lang="ts"></script>

<template>
  <n-message-provider>
    <n-modal-provider>
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
    </n-modal-provider>
  </n-message-provider>
</template>

<style scoped></style>
