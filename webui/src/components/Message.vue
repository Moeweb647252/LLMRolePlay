<script setup lang="ts">
import { MdContact, MdCreate, MdSync, MdClipboard } from '@vicons/ionicons4'
import { DeleteFilled } from '@vicons/material'

const props = defineProps<{
  content: string
  direction: 'left' | 'right'
  name: string
  avatar: string
  reloadable: boolean
}>()

const emit = defineEmits(['delete'])
</script>
<template>
  <div class="message">
    <div class="avatar">
      <n-avatar>
        <n-icon>
          <MdContact />
        </n-icon>
      </n-avatar>
    </div>
    <div class="main">
      <div class="header">{{ props.name }}</div>
      <div class="content">
        {{ props.content }}
      </div>
      <div class="action">
        <n-space size="small">
          <n-button size="small" strong secondary circle>
            <template #icon>
              <n-icon>
                <MdCreate />
              </n-icon>
            </template>
          </n-button>
          <n-button size="small" strong secondary circle>
            <template #icon>
              <n-icon>
                <MdClipboard />
              </n-icon>
            </template>
          </n-button>
          <n-button size="small" strong secondary circle @click="emit('delete')">
            <template #icon>
              <n-icon>
                <DeleteFilled />
              </n-icon>
            </template>
          </n-button>
          <n-button size="small" strong secondary circle v-if="reloadable">
            <template #icon>
              <n-icon>
                <MdSync />
              </n-icon>
            </template>
          </n-button>
        </n-space>
      </div>
    </div>
  </div>
</template>

<style scoped>
.message {
  width: 100%;
  display: flex;
  flex-direction: row;
}

.avatar {
  margin-right: 12px;
  margin-top: 2px;
}

.header {
  display: flex;
  margin-bottom: 4px;
  justify-content: space-between;
  align-items: center;
  font-size: 16px;
}
</style>
