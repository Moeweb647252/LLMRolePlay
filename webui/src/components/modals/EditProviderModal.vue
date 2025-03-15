<script setup lang="ts">
import type {
  AddModelForm,
  EditModelForm,
  EditProviderForm,
} from '@/types/modal'
import {
  NDynamicTags,
  NForm,
  NFormItem,
  NModal,
  NTag,
  NButton,
  useMessage,
  useModal,
  NIcon,
} from 'naive-ui'
import { h, ref, toRef } from 'vue'
import SettingsInput from '../SettingsInput.vue'
import { api } from '@/api'
import { MdAdd } from '@vicons/ionicons4'
import AddModelModal from './AddModelModal.vue'
import EditModelModal from './EditModelModal.vue'

const message = useMessage()
const modal = useModal()

const renderModelTags = (model: EditModelForm) => {
  return h(
    NTag,
    {
      closable: true,
      onClose: () => {
        modal.create({
          title: '删除模型',
          content: `确定删除模型 ${model.name} ?`,
          preset: 'dialog',
          positiveText: '确定',
          negativeText: '取消',
          onPositiveClick: async () => {
            try {
              await api.deleteModel(model.id!)
              form.value!.models.splice(form.value!.models.indexOf(model), 1)
              message.success('删除成功')
            } catch (e) {
              console.log(e)
              message.error('删除失败')
            }
          },
          onNegativeClick: () => {
            console.log('取消删除')
          },
        })
      },
      onClick: () => {
        editingModel.value = model
        editModelKey.value += 1
        editModelShow.value = true
      },
    },
    {
      default: () => model,
    },
  )
}

const props = defineProps<{
  value: EditProviderForm
}>()

const form = toRef(props, 'value')

const emit = defineEmits(['confirm'])

const show = defineModel('show', {
  type: Boolean,
  default: false,
})

const addModelShow = ref(false)
const addModelKey = ref(0)
const editModelShow = ref(false)
const editModelKey = ref(0)
const editingModel = ref<EditModelForm | null>(null)

const startAddModel = () => {
  addModelKey.value += 1
  addModelShow.value = true
}

const onConfirmAddModel = async (model: AddModelForm) => {
  let id = await api.addModel(
    model.name!,
    model.modelName!,
    model.description,
    form.value.id,
    model.isPublic,
    model.settings,
  )
  form.value.models.push({
    name: model.name!,
    modelName: model.modelName!,
    description: model.description,
    settings: model.settings,
    isPublic: model.isPublic,
    id,
  })
}
</script>

<template>
  <NModal v-model:show="show">
    <NForm v-if="form">
      <NFormItem label="名称">
        <SettingsInput
          :value="form.name"
          @confirm="
            async (name) => {
              await api.updateProvider(form.id, { name })
              form.name = name
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
      <NFormItem label="描述">
        <SettingsInput
          :value="form.description"
          @confirm="
            async (description) => {
              await api.updateProvider(form.id, { description })
              form.description = description
              emit('confirm', form)
            }
          "
        />
      </NFormItem>
      <NFormItem label="模型">
        <NDynamicTags :render-tag="renderModelTags as any">
          <template #trigger>
            <NButton size="small" type="primary" dashed @click="startAddModel">
              <template #icon>
                <NIcon>
                  <MdAdd />
                </NIcon>
              </template>
              添加模型
            </NButton>
          </template>
        </NDynamicTags>
      </NFormItem>
    </NForm>
  </NModal>
  <AddModelModal
    :key="addModelKey"
    v-model:show="addModelShow"
    @confirm="onConfirmAddModel"
  />
  <EditModelModal
    v-if="editingModel"
    :key="editModelKey"
    v-model:show="editModelShow"
    :value="editingModel"
  />
</template>
