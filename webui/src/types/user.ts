export class User {
  id: number | null
  username: string
  email: string
  token: string
  group: '1' | '2'

  constructor(
    id: number | null = null,
    username: string = '',
    email: string = '',
    token: string = '',
    group: '1' | '2' = '1',
  ) {
    this.id = id
    this.username = username
    this.email = email
    this.token = token
    this.group = group
  }
}
