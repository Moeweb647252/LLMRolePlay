<script setup lang="ts">
import { MdContact, MdCreate, MdSync, MdClipboard } from '@vicons/ionicons4'
import { DeleteFilled } from '@vicons/material'
import { onMounted, ref, watch } from 'vue'
import { NAvatar, NButton, NIcon, NSpace, NAlert, NInput } from 'naive-ui'

const action = ref('')
const text = ref('')
const think = ref('')
const editing = ref(false)
const editingValue = ref('')

const props = defineProps<{
  content: string
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
        text.value = processedContent
      } else {
        const openTagEnd = openTagStart + '<action>'.length
        const closeTagStart = processedContent.indexOf('</action>')

        if (closeTagStart !== -1) {
          // 完整的 action 标签
          action.value = processedContent.substring(openTagEnd, closeTagStart)
          text.value =
            processedContent.substring(0, openTagStart) +
            processedContent.substring(closeTagStart + '</action>'.length)
        } else {
          // 只有开始标签，没有结束标签
          action.value = processedContent.substring(openTagEnd)
          text.value = processedContent.substring(0, openTagStart)
        }

        // 清理可能残留的标签部分
        action.value = action.value.replace(/<\/?action[^>]*>/g, '')
        text.value = text.value.replace(/<\/?action[^>]*>/g, '')
      }

      // 确保 text 中不包含任何 think 标签残留
      text.value = text.value.replace(/<\/?think[^>]*>/g, '')
    },
    { immediate: true },
  )
})
</script>
<template>
  <div v-if="action.length && !editing" class="action">
    <NAlert :show-icon="false">
      {{ action }}
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
          {{ text }}
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
