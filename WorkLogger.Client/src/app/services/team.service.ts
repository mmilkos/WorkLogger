import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders, HttpParams} from "@angular/common/http";
import {CreateTeamDto} from '../DTOs/createTeamDto';
import {Observable, Subject} from "rxjs";
import { AccountService } from './account.service';
import { PagedResultModel } from '../models/pagedResult.model';
import { Team } from '../models/team.model';
import { TeamDetails } from '../models/teamDetails.model';
import {User, UsersNamesResponseDto} from '../models/User.model';
import { UserTeamDto } from '../DTOs/UserTeamDto';
import { TeamsNamesDto } from '../DTOs/TeamsNamesDto.model';
import { UserIdAndName } from '../models/UserIdAndName.model';

@Injectable({
  providedIn: 'root'
})
export class TeamService {
  apiUrl = "http://localhost:41669/api/team";
  constructor(private http : HttpClient, private accoount: AccountService) { }

  createTeam(dto: CreateTeamDto): Observable<any>
  {
    const header : HttpHeaders = this.accoount.getHeader();
    return this.http.post(`${this.apiUrl}/create`, dto, {headers: header})
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

  getTeamDetails(teamId: number) : Observable<TeamDetails>
  {
    const header : HttpHeaders = this.accoount.getHeader();
    return this.http.get<TeamDetails>(`${this.apiUrl}/${teamId}`,
      {
        headers: header,
      })
  }

  assignUserToTeam(dto: UserTeamDto): Observable<any>
  {
    const header : HttpHeaders = this.accoount.getHeader();
    return this.http.post(`${this.apiUrl}/assignUser`, dto, {headers: header});
  }

  unAssignUserToTeam(dto: UserTeamDto): Observable<any>
  {
    const header : HttpHeaders = this.accoount.getHeader();
    return this.http.post(`${this.apiUrl}/unAssignUser`, dto, {headers: header});
  }

  getTeamMembersPaged(params: HttpParams, teamId: number | null): Observable<PagedResultModel<User>>
  {
    const header : HttpHeaders = this.accoount.getHeader();
    return this.http.get<PagedResultModel<User>>(`${this.apiUrl}/${teamId}/teamMembers`,
      {
        headers: header,
        params: params });
  }

  getTeamsNames() : Observable<TeamsNamesDto>
  {
    const header : HttpHeaders = this.accoount.getHeader();
    return this.http.get<TeamsNamesDto>(`${this.apiUrl}/names`,
      {
        headers: header
      })
  }

  getTeamMembersNames(teamId: string | null) : Observable<UsersNamesResponseDto>
  {
    const header : HttpHeaders = this.accoount.getHeader();
    return this.http.get<UsersNamesResponseDto>(`${this.apiUrl}/${teamId}/teamMembers/names`,
      {
        headers: header
      })
  }
}
