import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AddTaskDto } from '../DTOs/addTaskDto.model';
import { Observable } from 'rxjs';
import { AccountService } from './account.service';
import { PagedResultModel } from '../models/pagedResult.model';
import { UserTask } from '../models/userTask.model';
import { UserTaskDetails } from '../models/userTaskDetails.model';
import { UpdateTaskDto } from '../DTOs/updateTaskDto.model';

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

  getAllTasksPaged(params: HttpParams): Observable<PagedResultModel<UserTask>>
  {
    let header = this.account.getHeader();
    return this.http.get<PagedResultModel<UserTask>>(this.apiUrl,
      {
        headers: header,
        params: params
      })
  }

  getTaskDetails(taskId: number): Observable<UserTaskDetails>
  {
    let header = this.account.getHeader()
    return this.http.get<UserTaskDetails>(this.apiUrl + taskId, { headers: header });
  }

  updateTask(task: UpdateTaskDto): Observable<any>
  {
    let header = this.account.getHeader()
    return this.http.patch(this.apiUrl, task, { headers: header })
  }
}
