import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import { RegisterCompanyDto } from '../DTOs/registerCompanyDto';

@Injectable({
  providedIn: 'root'
})
export class CompanyService {
  apiUrl = "http://localhost:41669/api/company/";
  constructor(private http: HttpClient) { }

  registerCompanyWithCeo(dto: RegisterCompanyDto): Observable<any>
  {
    return this.http.post(this.apiUrl + "register", dto)
  }
}
