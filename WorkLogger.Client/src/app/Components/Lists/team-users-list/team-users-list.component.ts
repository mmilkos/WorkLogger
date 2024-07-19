import { Component, Input, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { User } from '../../../models/User.model';
import { Subscription } from 'rxjs';
import { UserService } from '../../../services/user.service';
import { CommonService } from '../../../services/common.service';
import { HttpParams } from '@angular/common/http';
import { FormControl } from '@angular/forms';
import { AccountService } from '../../../services/account.service';
import { TeamService } from '../../../services/team.service';
import { ToastrService } from 'ngx-toastr';
import { UserTeamDto } from '../../../DTOs/UserTeamDto';

@Component({
  selector: 'app-team-users-list',
  templateUrl: './team-users-list.component.html',
  styleUrl: './team-users-list.component.css'
})
export class TeamUsersListComponent
{
  @Input() teamMode :null | boolean = false;
  @Input() teamId :null | number = 0;

  displayedColumns: string[] = ['name', 'surrname', 'team','role', 'details'];
  datasource: MatTableDataSource<User> = new MatTableDataSource()
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  private refreshSubscription?: Subscription;

  selectedRow = new FormControl(null);
  selectedRowId: string | null = null;

  totalRecords: number | undefined;
  pageSize: number | undefined;
  pageIndex: number | undefined;

  constructor(private usersService: UserService,
              private commonService: CommonService,
              private teamService: TeamService,
              private toastrService: ToastrService) {}



  ngOnInit(): void
  {
    this.datasource.paginator = this.paginator;
    this.refreshSubscription = this.commonService.refreshNeeded$.subscribe(() => {
      this.loadUsers(this.paginator.pageIndex + 1, this.paginator.pageSize);
    })
  }

  ngAfterViewInit(): void
  {
    if (this.teamMode)
    {
      this.loadTeamMembers(this.paginator.pageIndex + 1, this.paginator.pageSize);
      this.paginator.page.subscribe(() => {
        this.loadTeamMembers(this.paginator.pageIndex + 1, this.paginator.pageSize);
      });
    }
    else
    {
      this.loadUsers(this.paginator.pageIndex + 1, this.paginator.pageSize);
      this.paginator.page.subscribe(() => {
        this.loadUsers(this.paginator.pageIndex + 1, this.paginator.pageSize);
      });
    }

  }

  private loadTeamMembers(page: number, pageSize: number): void
  {
    const params: HttpParams = this.commonService.getParams(pageSize, page);

    this.teamService.getTeamMembersPaged(params, this.teamId).subscribe(
      pagedResult =>
      {
        this.totalRecords = pagedResult.totalRecords;
        this.pageSize = pageSize;
        this.pageIndex = page -1;
        this.datasource.data = pagedResult.dataList
        this.datasource._updateChangeSubscription()
        console.log(pagedResult.dataList)
      },
      (error) =>
      {
        console.log(error)
      })
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

  add()
  {
    let dto: UserTeamDto =
      {
        // @ts-ignore
        userId: this.selectedRowId || 0,
        teamId: this.teamId || 0
      }
    this.teamService.assignUserToTeam(dto).subscribe(
      ()=>
      {
        this.toastrService.success("Assigned user", "Succes");
        location.reload()
      } ,
      (error) => error.error.forEach((error: string) => this.toastrService.error(error, 'Error'))
    )
  }
  remove()
  {
    let dto: UserTeamDto =
      {
        // @ts-ignore
        userId: this.selectedRowId || 0,
        teamId: this.teamId || 0
      }
    this.teamService.unAssignUserToTeam(dto).subscribe(
      ()=>
      {
        this.toastrService.success("Unassigned user", "Succes");
        location.reload()
      },
      (error) => error.error.forEach((error: string) => this.toastrService.error(error, 'Error'))
    )
  }
  ngOnDestroy()
  {
    if (this.refreshSubscription) this.refreshSubscription.unsubscribe();
  }
}
