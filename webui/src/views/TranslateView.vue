<script setup lang="ts">
import { NLayout, NLayoutHeader, NLayoutContent, NInput, NCard } from 'naive-ui'
</script>

<template>
  <div></div>
  <NLayout>
    <NLayoutHeader></NLayoutHeader>
    <NLayoutContent>
      <div class="layout">
        <div>
          <NCard>
            <NInput type="textarea"></NInput>
          </NCard>
        </div>
        <div>
          <NCard>
            <NInput type="textarea"></NInput>
          </NCard>
        </div>
      </div>
    </NLayoutContent>
  </NLayout>
</template>

<style scoped>
.layout {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  row-gap: 1em;
  width: 100%;
}
</style>
