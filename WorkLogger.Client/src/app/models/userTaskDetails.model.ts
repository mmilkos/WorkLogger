import { User } from "./User.model";
import { Team } from "./team.model";

export interface UserTaskDetails
{
  id: number,
  assignedUser: User,
  author: User,
  team: Team,
  name: string,
  description: string,
  loggedHours: number,
}
