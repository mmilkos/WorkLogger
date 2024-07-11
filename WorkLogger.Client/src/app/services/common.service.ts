import { Injectable } from '@angular/core';
import {Subject} from "rxjs";
import {HttpClient, HttpParams} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class CommonService {

  private _refreshNeeded$ = new Subject<void>();
  constructor() { }

  getParams(pageSize: number, pageNumber : number ): HttpParams
  {
    let params = new HttpParams()
      .set('page', pageNumber)
      .set('pageSize', pageSize)

    return params;
  }

  get refreshNeeded$() {
    return this._refreshNeeded$.asObservable();
  }

  refresh() {
    this._refreshNeeded$.next();
  }
}
