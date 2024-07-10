import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegisterFormComponent } from './Components/register-form/register-form.component';
import { AboutComponent } from './Components/about/about.component';
import { RoutesEnum } from './enums/RoutesEnum';
import { TeamsComponent } from './Components/teams/teams.component';
import { AddTeamFormComponent } from './Components/add-team-form/add-team-form.component';

const routes: Routes =
  [
    {path: RoutesEnum.Register, component: RegisterFormComponent },
    {path: RoutesEnum.About, component: AboutComponent },
    {path: RoutesEnum.Teams, component: TeamsComponent,
    children:
      [
        {path: RoutesEnum.Create, component: AddTeamFormComponent}
      ] },
    {path: '**', component: AboutComponent, pathMatch: 'full' },
  ];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
