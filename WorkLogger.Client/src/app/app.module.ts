import { NgModule } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import {FormsModule} from "@angular/forms";
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './Components/header/header.component';
import { RegisterFormComponent } from './Components/register-form/register-form.component';
import { AboutComponent } from './Components/about/about.component';
import { TeamsComponent } from './Components/teams/teams.component';
import { SideBarComponent } from './Components/side-bar/side-bar.component';
import { AddUserFormComponent } from './Components/add-user-form/add-user-form.component';
import { AddTeamFormComponent } from './Components/add-team-form/add-team-form.component';
import { TeamsListComponent } from './Components/teams-list/teams-list.component';
import {PaginationModule} from "ngx-bootstrap/pagination";
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import {MatCheckbox} from "@angular/material/checkbox";


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
