export interface AddUserForm {
  username: string | null
  password: string | null
  email: string | null
  group: 1 | 2 | null
}

export interface EditUserForm {
  id: number
  username: string
  email: string
  group: 1 | 2
}
