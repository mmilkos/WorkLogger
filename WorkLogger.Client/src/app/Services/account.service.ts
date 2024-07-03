import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import { LoginUserDto } from '../DTOs/loginDto.model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  apiUrl = "http://localhost:41669/api/account/";

  constructor(private http: HttpClient) { }

  login(dto: LoginUserDto): Observable<any>
  {
    return this.http.post(this.apiUrl + "login", dto);
  }
}
