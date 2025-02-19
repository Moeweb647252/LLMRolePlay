<script setup lang="ts">
import { onMounted, ref, watch } from 'vue'
import { IosSend } from '@vicons/ionicons4'
</script>

<template>
  <div class="container">
    <div style="height: 4em">
      <n-flex align="center" justify="space-between" style="height: 100%; width: 100%">
        <h3>Chat Name</h3>
      </n-flex>
    </div>
    <div style="height: calc(100% - 4em); padding-top: 4em">
      <div class="chat-box">
        <div class="messages">
          <Message></Message>
        </div>
        <div class="input">
          <n-input
            type="textarea"
            placeholder="Input Message"
            round
            :autosize="{
              minRows: 1,
              maxRows: 5,
            }"
          >
            <template #suffix>
              <n-button type="primary" size="small" strong secondary>
                <template #icon>
                  <n-icon>
                    <IosSend />
                  </n-icon>
                </template>
              </n-button>
            </template>
          </n-input>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.chat-box {
  display: flex;
  flex-direction: column;
  height: 100%;
  overflow: hidden;
}

.messages {
  overflow: hidden;
  width: 100%;
  height: 100%;
  padding: 10px;
}
.input {
  width: 100%;
  padding-bottom: 1em;
}
.container {
  height: 100%;
  width: calc(100% - 4em);
  overflow: hidden;
  display: flex;
  flex-direction: column;
  margin-left: 2em;
  margin-right: 2em;
}
</style>
