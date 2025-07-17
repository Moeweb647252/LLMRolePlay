<script setup lang="ts">
import { MdContact, MdCreate, MdSync, MdClipboard } from '@vicons/ionicons4'
import { DeleteFilled } from '@vicons/material'
import { onMounted, ref, watch } from 'vue'
import { NAvatar, NButton, NIcon, NSpace, NInput, NAlert } from 'naive-ui'

const parts = ref<
  {
    type: 'text' | 'action'
    content: string
  }[]
>([])
const text = ref('')
const editing = ref(false)
const editingValue = ref('')

const props = defineProps<{
  content: string
  reasoningContent: string | null
  direction: 'left' | 'right' | null
  name: string
  avatar: string | null
  reloadable: boolean
}>()

const emit = defineEmits(['delete', 'regenerate', 'edit'])

const entities: Record<string, string> = {
  '&lt;': '<',
  '&gt;': '>',
  '&amp;': '&',
  '&quot;': '"',
  '&#39;': "'",
  '&#47;': '/', // 常见的斜杠实体
}

const startEdit = () => {
  editingValue.value = props.content
  editing.value = true
}

const onConfirmEdit = () => {
  emit('edit', editingValue.value)
  editing.value = false
}

onMounted(() => {
  watch(
    () => props.content,
    (newContent) => {
      // 初始化
      let processedContent = newContent || ''
      processedContent = processedContent.replace(/&[^;]+;/g, (match) => {
        return entities[match] || match // 如果没有匹配到实体，保留原样
      })
      parts.value = []

      // 解析内容为 parts
      let currentContent = processedContent
      let currentIndex = 0
      const tempParts: { type: 'text' | 'action'; content: string }[] = []

      while (currentIndex < currentContent.length) {
        const actionStart = currentContent.indexOf('<action>', currentIndex)

        if (actionStart === -1) {
          // 没有更多 action 标签，剩余内容都是文本
          const textContent = currentContent.substring(currentIndex)
          if (textContent.trim()) {
            tempParts.push({ type: 'text', content: textContent })
          }
          break
        }

        // 添加 action 前的文本部分
        if (actionStart > currentIndex) {
          const textContent = currentContent.substring(
            currentIndex,
            actionStart,
          )
          if (textContent.trim()) {
            tempParts.push({ type: 'text', content: textContent })
          }
        }

        // 处理 action 标签
        const actionEnd = currentContent.indexOf('</action>', actionStart)
        if (actionEnd !== -1) {
          // 完整的 action 标签
          const actionContent = currentContent.substring(
            actionStart + '<action>'.length,
            actionEnd,
          )
          tempParts.push({ type: 'action', content: actionContent })
          currentIndex = actionEnd + '</action>'.length
        } else {
          // 没有结束标签，剩余内容都是 action
          const actionContent = currentContent.substring(
            actionStart + '<action>'.length,
          )
          tempParts.push({ type: 'action', content: actionContent })
          break
        }
      }

      parts.value = tempParts
      text.value = currentContent
        .replace(/<\/?action[^>]*>/g, '')
        .replace(/<\/?think[^>]*>/g, '')
      console.log('parts', parts.value)
    },
    { immediate: true },
  )
})
</script>
<template>
  <div
    v-if="props.reasoningContent?.length && !editing"
    style="margin: 4px 8px 4px 8px"
  >
    <NAlert :show-icon="false">
      {{ props.reasoningContent }}
    </NAlert>
  </div>
  <div class="message">
    <div class="avatar">
      <NAvatar>
        <NIcon>
          <MdContact />
        </NIcon>
      </NAvatar>
    </div>
    <div class="main">
      <div class="header">
        {{ props.name }}
      </div>
      <div v-if="!editing">
        <div class="text">
          <div v-for="(part, index) in parts" :key="index">
            <span v-if="part.type === 'text'">{{ part.content }}</span>
            <div v-else-if="part.type === 'action'" class="action">
              {{ part.content }}
            </div>
          </div>
        </div>
        <div class="actions">
          <NSpace size="small">
            <NButton size="small" strong secondary circle @click="startEdit">
              <template #icon>
                <NIcon>
                  <MdCreate />
                </NIcon>
              </template>
            </NButton>
            <NButton size="small" strong secondary circle>
              <template #icon>
                <NIcon>
                  <MdClipboard />
                </NIcon>
              </template>
            </NButton>
            <NButton
              size="small"
              strong
              secondary
              circle
              @click="emit('delete')"
            >
              <template #icon>
                <NIcon>
                  <DeleteFilled />
                </NIcon>
              </template>
            </NButton>
            <NButton
              v-if="reloadable"
              size="small"
              strong
              secondary
              circle
              @click="emit('regenerate')"
            >
              <template #icon>
                <NIcon>
                  <MdSync />
                </NIcon>
              </template>
            </NButton>
          </NSpace>
        </div>
      </div>
      <div v-else>
        <NInput v-model:value="editingValue" type="textarea" autosize></NInput>
        <NSpace
          ><NButton @click="editing = false">取消</NButton
          ><NButton @click="onConfirmEdit">确定</NButton></NSpace
        >
      </div>
    </div>
  </div>
</template>

<style scoped>
.action {
  background-color: #f0a020;
  border-radius: 4px;
  padding: 4px 8px;
  width: fit-content;
}

.text {
  display: flex;
  flex-direction: column;
  gap: 4px;
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
