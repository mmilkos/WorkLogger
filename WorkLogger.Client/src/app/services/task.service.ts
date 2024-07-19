import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AddTaskDto } from '../DTOs/addTaskDto.model';
import { Observable } from 'rxjs';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class TaskService {

   apiUrl = "http://localhost:41669/api/task/";
  constructor(private http: HttpClient, private account: AccountService) { }

  addTask(task: AddTaskDto): Observable<any>
  {
    let header = this.account.getHeader();
    return this.http.post(this.apiUrl, task, {headers: header})

  }
}
