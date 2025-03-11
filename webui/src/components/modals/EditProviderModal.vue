<script setup lang="ts">
import type { EditModelForm } from '@/types/modal'
import { toRef } from 'vue'

const props = defineProps<{
  form: EditModelForm
}>()

const form = toRef(props, 'form')

const show = defineModel('show', {
  type: Boolean,
  default: false,
})
</script>

<template></template>
