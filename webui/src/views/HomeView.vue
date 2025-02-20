<script setup lang="ts">
import ChatBox from '@/components/ChatBox.vue'
import { useSettingsStore } from '@/stores/settings'
import { IosMenu, MdContact } from '@vicons/ionicons4'
import { useRouter } from 'vue-router'

const settings = useSettingsStore()
const router = useRouter()

if (!settings.user) {
  router.push('/login')
}

const dropdownOptions = [
  {
    label: 'Option 1',
    key: 'option1',
  },
  {
    label: 'Option 2',
    key: 'option2',
  },
]
</script>
<template>
  <n-layout has-sider class="full bfc">
    <n-layout-sider width="200" theme="dark" class="bfc">
      <div class="sider bfc">
        <div
          style="
            margin: 2px 4px;
            height: 3.5em;
            display: flex;
            align-items: center;
            justify-content: center;
          "
        >
          <h2>LLMRolePlay</h2>
        </div>
        <div style="margin: 2px 4px; height: 2.5em">
          <n-button type="default" @click="() => {}" style="width: 100%; height: 100%"
            >Button 1</n-button
          >
        </div>
        <n-scrollbar style="height: 100%" content-style="margin: 0 4px">
          <n-list :show-divider="false">
            <n-list-item v-for="item in 10" :key="item" style="height: 2.5em; margin-left: 1em">
              Chat {{ item }}
            </n-list-item>
          </n-list>
        </n-scrollbar>
      </div>
    </n-layout-sider>
    <n-layout class="full bfc">
      <n-layout-header
        style="
          display: flex;
          align-items: center;
          justify-content: space-between;
          padding-top: 0.5em;
          height: 3em;
        "
      >
        <n-button type="default" @click="() => {}" strong secondary>
          <n-icon size="2em">
            <IosMenu />
          </n-icon>
        </n-button>
        <n-dropdown :options="dropdownOptions">
          <n-icon size="2.5em" style="padding-right: 0.5em">
            <MdContact />
          </n-icon>
        </n-dropdown>
      </n-layout-header>
      <n-layout-content class="bfc" style="height: calc(100% - 3.5em)">
        <ChatBox chat-id="1"></ChatBox>
      </n-layout-content>
    </n-layout>
  </n-layout>
</template>

<style scoped>
.full {
  height: 100%;
  width: 100%;
}

.sider {
  height: 100%;
  width: 100%;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.bfc {
  overflow: hidden;
}
</style>
