import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegisterFormComponent } from './Components/register-form/register-form.component';
import { AboutComponent } from './Components/about/about.component';
import { RoutesEnum } from './enums/RoutesEnum';
import { TeamsComponent } from './Components/teams/teams.component';

const routes: Routes =
  [
    {path: RoutesEnum.Register, component: RegisterFormComponent },
    {path: RoutesEnum.About, component: AboutComponent },
    {path: RoutesEnum.Teams, component: TeamsComponent },
    {path: '**', component: AboutComponent, pathMatch: 'full' },
  ];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
