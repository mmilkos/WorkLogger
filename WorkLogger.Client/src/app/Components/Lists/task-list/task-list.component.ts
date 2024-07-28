import { HttpParams } from '@angular/common/http';
import {AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';
import { CommonService } from '../../../services/common.service';
import { TaskService } from '../../../services/task.service';
import { UserTask } from '../../../models/userTask.model';
import { MatDialog } from '@angular/material/dialog';
import { TaskDetailsCardComponent } from '../../details/tasks/task-details-card/task-details-card.component';
import { AddTaskFormComponent } from '../../forms/add-task-form/add-task-form.component';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrl: './task-list.component.css'
})
export class TaskListComponent implements OnDestroy, OnInit, AfterViewInit
{
  selectedRow = new FormControl(null);
  selectedRowId: string | null = null;
  totalRecords: number | undefined;
  pageSize: number | undefined;
  pageIndex: number | undefined;
  displayedColumns: string[] = ['name', 'teamName', 'assignedUser', 'details'];
  dataSource : MatTableDataSource<UserTask> = new MatTableDataSource()
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  private pageSubscription?: Subscription;
  private refreshSubscription?: Subscription;

  constructor(private commonService: CommonService, private taskService: TaskService, private dialog: MatDialog) {}

  ngOnInit(): void
  {
    this.dataSource.paginator = this.paginator;
    this.loadTasks(1, 10)

    this.refreshSubscription = this.commonService.refreshNeeded$.subscribe(
      () => this.loadTasks(this.paginator.pageIndex + 1, this.paginator.pageSize))
  }

  ngAfterViewInit(): void {
    this.loadTasks(this.paginator.pageIndex + 1, this.paginator.pageSize)
    this.pageSubscription = this.paginator.page.subscribe(() => this.loadTasks(this.paginator.pageIndex + 1, this.paginator.pageSize));
  }

  private loadTasks(page: number, pageSize: number): void {
    const params: HttpParams =  this.commonService.getParams(pageSize, page);

    this.taskService.getAllTasksPaged(params).subscribe(pagedResult => {
      this.totalRecords = pagedResult.totalRecords;
      this.pageSize = pageSize;
      this.pageIndex = page -1;
      this.dataSource.data = pagedResult.dataList;
    });
  }


  rowClicked(row: any) {
    if(row.selected){
      this.selectedRowId = null;
      row.selected = false;
    } else {
      this.selectedRowId = row.id;
      row.selected = true;
    }
  }

  selectRow(row: any) {
    if(row.selected){
      this.selectedRowId = null;
      row.selected = false;
    } else {
      this.selectedRowId = row.id;
      row.selected = true;
    }
  }

  onClick()
  {
    this.dialog.open(TaskDetailsCardComponent, {
      data:
        {
          taskId: this.selectedRowId
        }
    });
  }

  ngOnDestroy(): void
  {
    if (this.refreshSubscription) this.refreshSubscription.unsubscribe();
    if (this.pageSubscription) this.pageSubscription.unsubscribe();
  }
}
