import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders, HttpParams} from "@angular/common/http";
import {CreateTeamDto} from '../DTOs/createTeamDto';
import {Observable} from "rxjs";
import { AccountService } from './account.service';
import { PagedResultModel } from '../models/pagedResult.model';
import { Team } from '../models/team.model';

@Injectable({
  providedIn: 'root'
})
export class TeamService {
  apiUrl = "http://localhost:41669/api/team";
  constructor(private http : HttpClient, private accoount: AccountService) { }

  createTeam(dto: CreateTeamDto): Observable<any>
  {
    const header : HttpHeaders = this.accoount.getHeader();
    return this.http.post(this.apiUrl + "/create", dto, {headers: header})
  }

  getTeamsPaged(params: HttpParams): Observable<PagedResultModel<Team>>
  {
    const header : HttpHeaders = this.accoount.getHeader();
    return this.http.get<PagedResultModel<Team>>(this.apiUrl,
      {
        headers: header,
        params: params
      })
  }

  getParams(pageSize: number, pageNumber : number ): HttpParams
  {
    let params = new HttpParams()
      .set('page', pageNumber)
      .set('pageSize', pageSize)

    return params;
  }
}
