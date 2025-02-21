import { defineStore } from "pinia";
import { ref } from "vue";

export interface Model {
  id: number;
  name: string;
  modelName: string;
  description: string;
  provider: Provider;
}

export interface Provider {
  id: number;
  name: string;
  url: string;
  apiKey: string;
  type: "openai" | "google" | "azure";
  models: Model[]
}

export const useProviderStore = defineStore("providers", ()=>{
  const providers = ref<Provider[]>([]);
  return providers;
})