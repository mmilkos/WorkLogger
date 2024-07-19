import {AfterViewInit, Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {MatTableDataSource} from "@angular/material/table";
import {HttpParams} from "@angular/common/http";
import { MatPaginator } from '@angular/material/paginator';
import { Team } from '../../../models/team.model';
import { TeamService } from '../../../services/team.service';
import {Subscription} from "rxjs";
import { CommonService } from '../../../services/common.service';
import { FormControl } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-teams-list',
  templateUrl: './teams-list.component.html',
  styleUrl: './teams-list.component.css'
})
export class TeamsListComponent implements OnInit, AfterViewInit, OnDestroy
{
  displayedColumns: string[] = ['name', 'details'];
  dataSource : MatTableDataSource<Team> = new MatTableDataSource()
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  private pageSubscription?: Subscription;
  private refreshSubscription?: Subscription;
  selectedRow = new FormControl(null);
  selectedRowId: string | null = null;

  totalRecords: number | undefined;
  pageSize: number | undefined;
  pageIndex: number | undefined;

  constructor(private teamsService: TeamService,
              private commonService: CommonService,
              private router: Router) {}

  ngOnInit(): void
  {
    this.loadTeams(1, 5)

    this.refreshSubscription = this.commonService.refreshNeeded$.subscribe(() => {
      this.loadTeams(this.paginator.pageIndex + 1, this.paginator.pageSize);
    })
  }

  ngAfterViewInit(): void {
    this.loadTeams(this.paginator.pageIndex + 1, this.paginator.pageSize);
    this.pageSubscription = this.paginator.page.subscribe(() => this.loadTeams(this.paginator.pageIndex + 1, this.paginator.pageSize));
  }


  private loadTeams(page: number, pageSize: number): void {
    const params: HttpParams =  this.commonService.getParams(pageSize, page);

    this.teamsService.getTeamsPaged(params).subscribe(pagedResult => {
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
    let link : string = this.router.url + "/" + this.selectedRowId;
    this.router.navigateByUrl(link)
  }


  ngOnDestroy()
  {
    if (this.pageSubscription) this.pageSubscription.unsubscribe();
    if (this.refreshSubscription) this.refreshSubscription.unsubscribe();
  }
}
