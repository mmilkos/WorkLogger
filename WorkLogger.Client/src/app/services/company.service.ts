import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import { RegisterCompanyDto } from '../DTOs/registerCompanyDto';
import { WLConfig } from '../WLConfig';

@Injectable({
  providedIn: 'root'
})
export class CompanyService {
  apiUrl = WLConfig.apiUrl + "/company";
  constructor(private http: HttpClient) { }

  registerCompanyWithCeo(dto: RegisterCompanyDto): Observable<any>
  {
    return this.http.post(this.apiUrl + "/register", dto)
  }
}
