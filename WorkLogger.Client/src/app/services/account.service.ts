import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import { LoginUserDto } from '../DTOs/loginDto.model';
import * as jwt_decoder from 'jwt-decode';
import {LoggedUser} from '../models/loggedUser.model';
@Injectable({
  providedIn: 'root'
})
export class AccountService {
  apiUrl = "http://localhost:41669/api/account/";
  public currentUser: LoggedUser | undefined;
  public isLoggedIn: boolean = false;


  constructor(private http: HttpClient) { }

  login(dto: LoginUserDto): Observable<any>
  {
    return this.http.post(this.apiUrl + "login", dto);
  }

 public setToken(token: string): void
  {
    localStorage.setItem('token', token);
    const decodedToken = jwt_decoder.jwtDecode(token) as any;
    this.currentUser = {
      companyId: decodedToken.CompanyId,
      id: decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'],
      role: decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'],
      username: decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname'],
      name: decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
      surname: decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname']
    }
    this.isLoggedIn = true;
  }

  logOut(): void
  {
    localStorage.removeItem('token');
    this.currentUser = undefined;
    this.isLoggedIn = false;
  }
}
