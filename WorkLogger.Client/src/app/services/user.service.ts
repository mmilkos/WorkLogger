import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders, HttpParams} from "@angular/common/http";
import { AccountService } from './account.service';
import {Observable} from "rxjs";
import { PagedResultModel } from '../models/pagedResult.model';
import { User } from '../models/User.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  apiUrl = "http://localhost:41669/api/user";
  constructor(private http : HttpClient, private accoount: AccountService) { }

  getUsersPaged(params: HttpParams): Observable<PagedResultModel<User>>
  {
    const header : HttpHeaders = this.accoount.getHeader();
    return this.http.get<PagedResultModel<User>>(this.apiUrl,
      {
        headers: header,
        params: params });
  }
}
