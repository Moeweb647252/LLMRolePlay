<script setup lang="ts">
import {
  NModal,
  NForm,
  NFormItem,
  NButton,
  NDynamicTags,
  NIcon,
} from 'naive-ui'
import SettingsInput from '../SettingsInput.vue'
import { ref } from 'vue'

const show = defineModel<boolean>('show', {
  default: false,
})

const form = ref(null)
</script>

<template>
  <n-modal
    v-model:show="show"
    title="编辑聊天"
    preset="card"
    style="width: fit-content; min-width: 25em"
    size="medium"
  >
    <n-form label-placement="left">
      <n-form-item label="名称">
        <SettingsInput
          :value="form.chat!.name!"
          @confirm="
            async (name) => {
              await api.updateChat(form.chat!.id!, {
                name: name,
              })
              form.chat!.name = name
            }
          "
        />
      </n-form-item>
      <n-form-item label="描述">
        <SettingsInput
          :value="form.chat!.description!"
          @confirm="
            async (description) => {
              console.log(description)
              await api.updateChat(form.chat!.id!, {
                description: description,
              })
              form.chat!.description = description
            }
          "
        />
      </n-form-item>
      <n-form-item label="参与者">
        <n-dynamic-tags
          v-model:value="form.chat!.participants!"
          :render-tag="renderEditChatParticipant"
        >
          <template #trigger>
            <n-button
              size="small"
              type="primary"
              dashed
              @click="editChatAddParticipant"
            >
              <template #icon>
                <n-icon>
                  <MdAdd />
                </n-icon>
              </template>
              添加参与者
            </n-button>
          </template>
        </n-dynamic-tags>
      </n-form-item>
    </n-form>
  </n-modal>
</template>
