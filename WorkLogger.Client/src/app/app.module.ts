import { NgModule } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import {FormsModule} from "@angular/forms";
import {PaginationModule} from "ngx-bootstrap/pagination";
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import {MatCheckbox} from "@angular/material/checkbox";
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './Components/header/header.component';
import { RegisterFormComponent } from './Components/forms/register-form/register-form.component';
import { AboutComponent } from './Components/Pages/about/about.component';
import { TeamsComponent } from './Components/Pages/teams/teams.component';
import { SideBarComponent } from './Components/side-bar/side-bar.component';
import { AddUserFormComponent } from './Components/forms/add-user-form/add-user-form.component';
import { AddTeamFormComponent } from './Components/forms/add-team-form/add-team-form.component';
import { TeamsListComponent } from './Components/Lists/teams-list/teams-list.component';
import { UsersComponent } from './Components/Pages/users/users.component';
import { UsersListComponent } from './Components/Lists/users-list/users-list.component';
import { TeamDetailsComponent } from './Components/Pages/team-details/team-details.component';
import { TeamDetailsCardComponent } from './Components/details/teams/team-details-card/team-details-card.component';
import { TeamUsersListComponent } from './Components/Lists/team-users-list/team-users-list.component';
import { TasksComponent } from './Components/Pages/tasks/tasks.component';
import { AddTaskFormComponent } from './Components/forms/add-task-form/add-task-form.component';
import { TaskListComponent } from './Components/lists/task-list/task-list.component';
import { TaskDetailsCardComponent } from './Components/details/tasks/task-details-card/task-details-card.component';




@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    RegisterFormComponent,
    AboutComponent,
    TeamsComponent,
    SideBarComponent,
    AddUserFormComponent,
    AddTeamFormComponent,
    TeamsListComponent,
    UsersComponent,
    UsersListComponent,
    TeamDetailsComponent,
    TeamDetailsCardComponent,
    TeamUsersListComponent,
    TasksComponent,
    AddTaskFormComponent,
    TaskListComponent,
    TaskDetailsCardComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    MatCardModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({positionClass:'toast-top-right',
      preventDuplicates: false, timeOut: 2500 }),
    PaginationModule.forRoot(),
    MatPaginatorModule,
    MatTableModule,
    MatCheckbox
  ],
  providers: [
    provideAnimationsAsync()
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
