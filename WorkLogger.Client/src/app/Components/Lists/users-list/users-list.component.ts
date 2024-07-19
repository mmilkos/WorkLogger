import {AfterViewInit, Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {MatTableDataSource} from "@angular/material/table";
import { User } from '../../../models/User.model';
import {MatPaginator} from "@angular/material/paginator";
import { UserService } from '../../../services/user.service';
import {HttpParams} from "@angular/common/http";
import { CommonService } from '../../../services/common.service';
import {Subscription} from "rxjs";

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrl: './users-list.component.css'
})
export class UsersListComponent implements OnInit, AfterViewInit, OnDestroy
{
  displayedColumns: string[] = ['name', 'surrname', 'team','role', 'details'];
  datasource: MatTableDataSource<User> = new MatTableDataSource()
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  private refreshSubscription?: Subscription;

  totalRecords: number | undefined;
  pageSize: number | undefined;
  pageIndex: number | undefined;

  constructor(private usersService: UserService,
              private commonService: CommonService) {}



  ngOnInit(): void
  {
    this.datasource.paginator = this.paginator;
    this.refreshSubscription = this.commonService.refreshNeeded$.subscribe(() => {
      this.loadUsers(this.paginator.pageIndex + 1, this.paginator.pageSize);
    })
  }

  ngAfterViewInit(): void
  {
    this.loadUsers(this.paginator.pageIndex + 1, this.paginator.pageSize);
    this.paginator.page.subscribe(() => {
      this.loadUsers(this.paginator.pageIndex + 1, this.paginator.pageSize);
    });
  }

  private loadUsers(page: number, pageSize: number): void
  {
    const params: HttpParams = this.commonService.getParams(pageSize, page);

    this.usersService.getUsersPaged(params).subscribe(
      pagedResult =>
      {
        this.totalRecords = pagedResult.totalRecords;
        this.pageSize = pageSize;
        this.pageIndex = page -1;
        this.datasource.data = pagedResult.dataList
        this.datasource._updateChangeSubscription()
      },
      (error) =>
      {
        console.log(error)
      })
  }
  ngOnDestroy()
  {
    if (this.refreshSubscription) this.refreshSubscription.unsubscribe();
  }

}
