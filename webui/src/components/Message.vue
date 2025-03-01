<script setup lang="ts">
import { MdContact, MdCreate, MdSync, MdClipboard } from '@vicons/ionicons4'
import { DeleteFilled } from '@vicons/material'
import { onMounted, ref, watch } from 'vue'

const action = ref('')
const content = ref('')
const think = ref('')

const props = defineProps<{
  content: string
  direction: 'left' | 'right'
  name: string
  avatar: string
  reloadable: boolean
}>()

const emit = defineEmits(['delete'])

const entities = {
  '&lt;': '<',
  '&gt;': '>',
  '&amp;': '&',
  '&quot;': '"',
  '&#39;': "'",
  '&#47;': '/', // 常见的斜杠实体
}

onMounted(() => {
  watch(
    () => props.content,
    (newContent) => {
      // 初始化
      let processedContent = newContent || ''
      processedContent = processedContent.replace(/&[^;]+;/g, (match) => {
        return (entities as any)[match] || match // 如果没有匹配到实体，保留原样
      })
      think.value = ''
      action.value = ''

      // 先处理 think 标签
      let openTagStart = processedContent.indexOf('<think>')
      if (openTagStart !== -1) {
        const openTagEnd = openTagStart + '<think>'.length
        const closeTagStart = processedContent.indexOf('</think>')

        if (closeTagStart !== -1) {
          // 完整的 think 标签
          think.value = processedContent.substring(openTagEnd, closeTagStart)
          processedContent =
            processedContent.substring(0, openTagStart) +
            processedContent.substring(closeTagStart + '</think>'.length)
        } else {
          // 只有开始标签，没有结束标签
          think.value = processedContent.substring(openTagEnd)
          processedContent = processedContent.substring(0, openTagStart)
        }

        // 清理可能残留的标签部分
        think.value = think.value.replace(/<\/?think[^>]*>/g, '')
      }

      // 接着处理 action 标签
      openTagStart = processedContent.indexOf('<action>')
      if (openTagStart === -1) {
        // 没有 action 标签
        content.value = processedContent
      } else {
        const openTagEnd = openTagStart + '<action>'.length
        const closeTagStart = processedContent.indexOf('</action>')

        if (closeTagStart !== -1) {
          // 完整的 action 标签
          action.value = processedContent.substring(openTagEnd, closeTagStart)
          content.value =
            processedContent.substring(0, openTagStart) +
            processedContent.substring(closeTagStart + '</action>'.length)
        } else {
          // 只有开始标签，没有结束标签
          action.value = processedContent.substring(openTagEnd)
          content.value = processedContent.substring(0, openTagStart)
        }

        // 清理可能残留的标签部分
        action.value = action.value.replace(/<\/?action[^>]*>/g, '')
        content.value = content.value.replace(/<\/?action[^>]*>/g, '')
      }

      // 确保 content 中不包含任何 think 标签残留
      content.value = content.value.replace(/<\/?think[^>]*>/g, '')
    },
    { immediate: true },
  )
})
</script>
<template>
  <div class="action" v-if="action.length">
    <n-alert :show-icon="false">
      {{ action }}
    </n-alert>
  </div>
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
        {{ content }}
      </div>
      <div class="actions">
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
.action {
  margin: 1em 3em 1em 3em;
}

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
