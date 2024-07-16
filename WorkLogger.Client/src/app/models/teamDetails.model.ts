import { User } from "./User.model";

export interface TeamDetails
{
  teamId: number;
  name: string;
  manager?: User
}
