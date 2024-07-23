import { User } from "./User.model";

export interface UserTask
{
  id: number;
  name: string;
  Team: string;
  AssignedUser: User
}
