import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {CreateTeamDto} from '../DTOs/createTeamDto';
import {Observable} from "rxjs";
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class TeamService {
  apiUrl = "http://localhost:41669/api/team/";
  constructor(private http : HttpClient, private accoount: AccountService) { }

  createTeam(dto: CreateTeamDto): Observable<any>
  {
    const header : HttpHeaders = this.accoount.getHeader();
    return  this.http.post(this.apiUrl + "create", dto, {headers: header})
  }
}
