import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddTeamFormComponent } from './Components/forms/add-team-form/add-team-form.component';
import { RegisterFormComponent } from './Components/forms/register-form/register-form.component';
import { AboutComponent } from './Components/Pages/about/about.component';
import { TeamsComponent } from './Components/Pages/teams/teams.component';
import { RoutesEnum } from './enums/RoutesEnum';
import { UsersComponent } from './Components/Pages/users/users.component';
import { TeamDetailsComponent } from './Components/Pages/team-details/team-details.component';
import { TasksComponent } from './Components/Pages/tasks/tasks.component';
import { RaportsComponent } from './Components/Pages/raports/raports.component';


const routes: Routes =
  [
    {path: RoutesEnum.Register, component: RegisterFormComponent },
    {path: RoutesEnum.About, component: AboutComponent },
    {path: RoutesEnum.Teams, component: TeamsComponent},
    {path: RoutesEnum.Teams + RoutesEnum.Id, component: TeamDetailsComponent},
    {path: RoutesEnum.Users, component: UsersComponent},
    {path: RoutesEnum.Tasks, component: TasksComponent},
    {path: RoutesEnum.Raports, component: RaportsComponent},
    {path: '**', component: AboutComponent, pathMatch: 'full' },
  ];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
