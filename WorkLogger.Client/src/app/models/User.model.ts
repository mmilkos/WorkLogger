export interface User
{
  id: number,
  name: string,
  surname: string,
  team: string,
  role: string
}

export interface UsersNamesResponseDto {
  names: User[];
}
