<script setup lang="ts">
import { IosSend, MdContact, MdCreate, MdSync, MdClipboard } from '@vicons/ionicons4'
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
      <div class="header">Character Name</div>
      <div class="content">
        "1"是阿拉伯数字中最小的正整数，常用于计数、编号等场景。请问有具体的问题或需要进一步解释吗？
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
                <MdSync />
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
