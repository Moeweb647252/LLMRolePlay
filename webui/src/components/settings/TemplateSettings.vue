<script setup lang="ts">
import { api } from '@/api'
import { Template } from '@/types/template'
import { useMessage, useModal } from 'naive-ui'
import { ref } from 'vue'
import SettingsInput from '../SettingsInput.vue'

const templates = ref(await api.getTemplates())
const message = useMessage()
const model = useModal()

const addTemplateForm = ref({
  visible: false,
  name: '',
  description: '',
  content: '',
  isPublic: false,
})

const addTemplate = async () => {
  let data = JSON.parse(JSON.stringify(addTemplateForm.value))
  addTemplateForm.value = {
    name: '',
    description: '',
    content: '',
    visible: false,
    isPublic: false,
  }
  let id = await api.addTemplate(data.name, data.content, data.description)
  templates.value.push(new Template(id, data.name, data.description, data.content, data.isPublic))
  message.success('添加成功')
}

const cancelAddTemplate = () => {
  addTemplateForm.value = {
    name: '',
    description: '',
    content: '',
    visible: false,
    isPublic: false,
  }
}

const editTemplateForm = ref({
  visible: false,
  template: null as Template | null,
})

const editTemplate = (template: Template) => {
  editTemplateForm.value = {
    visible: true,
    template,
  }
}

const deleteTemplate = async (template: Template) => {
  model.create({
    title: '删除模板',
    content: `确定删除模板 ${template.name} ?`,
    preset: 'dialog',
    positiveText: '确定',
    negativeText: '取消',
    onPositiveClick: async () => {
      try {
        await api.deleteTemplate(template.id!)
        templates.value.splice(templates.value.indexOf(template), 1)
        message.success('删除成功')
      } catch (e) {
        message.error('删除失败')
      }
    },
  })
}
</script>
<template>
  <div style="padding: 2em">
    <div class="header">
      <h3>模板</h3>
      <n-button type="primary" @click="addTemplateForm.visible = true">添加</n-button>
    </div>
    <div>
      <n-list>
        <n-list-item v-for="template in templates" :key="template.id">
          {{ template.name }}
          <template #suffix>
            <n-space :wrap="false">
              <n-button type="primary" @click="editTemplate(template)">编辑</n-button>
              <n-button type="error" @click="deleteTemplate(template)">删除</n-button>
            </n-space>
          </template>
        </n-list-item>
      </n-list>
    </div>
  </div>
  <n-modal
    title="添加模板"
    size="medium"
    v-model:show="addTemplateForm.visible"
    preset="card"
    style="width: fit-content; min-width: 25em"
  >
    <n-form label-placement="left">
      <n-form-item label="名称">
        <n-input v-model:value="addTemplateForm.name" />
      </n-form-item>
      <n-form-item label="描述">
        <n-input v-model:value="addTemplateForm.description" />
      </n-form-item>
      <n-form-item label="公开">
        <n-switch v-model:value="addTemplateForm.isPublic" />
      </n-form-item>
      <n-form-item label="内容">
        <n-input type="textarea" v-model:value="addTemplateForm.content" />
      </n-form-item>
    </n-form>
    <template #footer>
      <n-space justify="end">
        <n-button @click="cancelAddTemplate">取消</n-button>
        <n-button type="primary" @click="addTemplate">保存</n-button>
      </n-space>
    </template>
  </n-modal>
  <n-modal
    title="编辑模板"
    size="medium"
    v-model:show="editTemplateForm.visible"
    preset="card"
    style="width: fit-content; min-width: 25em"
  >
    <n-form label-placement="left">
      <n-form-item label="名称">
        <SettingsInput
          :value="editTemplateForm.template!.name"
          @confirm="
            async () =>
              await api.updateTemplate(editTemplateForm.template!.id!, {
                name: editTemplateForm.template!.name!,
              })
          "
        ></SettingsInput>
      </n-form-item>
      <n-form-item label="描述">
        <SettingsInput
          :value="editTemplateForm.template!.description"
          @confirm="
            async () =>
              await api.updateTemplate(editTemplateForm.template!.id!, {
                description: editTemplateForm.template!.description!,
              })
          "
        ></SettingsInput>
      </n-form-item>
      <n-form-item label="公开">
        <SettingsSwitch
          :value="editTemplateForm.template!.isPublic"
          @confirm="
            async (isPublic: boolean) => {
              await api.updateTemplate(editTemplateForm.template!.id!, {
                isPublic: isPublic,
              })
              editTemplateForm.template!.isPublic = isPublic
            }
          "
        ></SettingsSwitch>
      </n-form-item>
      <n-form-item label="内容">
        <SettingsInput
          :value="editTemplateForm.template!.content"
          type="textarea"
          @confirm="
            async (content: any) => {
              await api.updateTemplate(editTemplateForm.template!.id!, {
                content,
              })
              editTemplateForm.template!.content = content
            }
          "
        ></SettingsInput>
      </n-form-item>
    </n-form>
  </n-modal>
</template>

<style scoped>
.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1em;
}
</style>
