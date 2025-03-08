export interface AddUserForm {
  username: string | null
  password: string | null
  email: string | null
  role: string
}

export interface EditUserForm {
  id: number
  username: string
  email: string | null
  role: string
}
