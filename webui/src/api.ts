import { sha256 } from "js-sha256"
import { useSettingsStore } from "./stores/settings"
import axios from "axios"

class NoTokenError extends Error {
  constructor() {
    super("No token found")
  }
}

class Api {
  store: ReturnType<typeof useSettingsStore> = useSettingsStore()  
  constructor() {
  }

  async request(path:string, body: object, method: string = "POST") {
    const token = this.store.user?.token
    if (!token) {
      throw new NoTokenError()
    }
    const headers = {
      Token: token
    }
    const response = await axios({
      method: method,
      url: '/api/' + path,
      data: body,
      headers: headers
    })
    if (response.status !== 200) {
      throw new Error("Request failed")
    }
    return response.data
  }

  async login(email: string, password: string) {
    const resp = await this.request("login", {
      email: email,
      password: sha256(password)
    })
    if (resp.code === 200) {
      this.store.user = {
        email: resp.data.email,
        username: resp.data.username,
        id: resp.data.id,
        group: resp.data.group,
        token: resp.data.token,
      }
    } else {
      throw new Error(resp.msg)
    }
  }

  async logout() {
    this.store.user = null
  }
}

export const api = new Api()