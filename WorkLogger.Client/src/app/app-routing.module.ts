import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddTeamFormComponent } from './Components/forms/add-team-form/add-team-form.component';
import { RegisterFormComponent } from './Components/forms/register-form/register-form.component';
import { AboutComponent } from './Components/Pages/about/about.component';
import { TeamsComponent } from './Components/Pages/teams/teams.component';
import { RoutesEnum } from './enums/RoutesEnum';
import { UsersComponent } from './Components/Pages/users/users.component';


const routes: Routes =
  [
    {path: RoutesEnum.Register, component: RegisterFormComponent },
    {path: RoutesEnum.About, component: AboutComponent },
    {path: RoutesEnum.Teams, component: TeamsComponent},
    {path: RoutesEnum.Users, component: UsersComponent},
    {path: '**', component: AboutComponent, pathMatch: 'full' },

  ];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
