import {AfterViewInit, Component, OnInit, ViewChild} from '@angular/core';
import {MatTableDataSource} from "@angular/material/table";
import { Team } from '../../models/team.model';
import { TeamService } from '../../services/team.service';
import {HttpParams} from "@angular/common/http";
import { MatPaginator } from '@angular/material/paginator';

@Component({
  selector: 'app-teams-list',
  templateUrl: './teams-list.component.html',
  styleUrl: './teams-list.component.css'
})
export class TeamsListComponent implements OnInit, AfterViewInit
{
  displayedColumns: string[] = ['name', 'details'];
  dataSource : MatTableDataSource<Team> = new MatTableDataSource()
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  totalRecords: number | undefined;
  pageSize: number | undefined;
  pageIndex: number | undefined;

  constructor(private teamsService: TeamService,) {}

  ngOnInit(): void
  {
    this.loadTeams(1, 10)
  }

  ngAfterViewInit(): void {
    this.loadTeams(this.paginator.pageIndex + 1, this.paginator.pageSize);
    this.paginator.page.subscribe(() => this.loadTeams(this.paginator.pageIndex + 1, this.paginator.pageSize));
  }


  private loadTeams(page: number, pageSize: number): void {
    const params: HttpParams =  this.teamsService.getParams(pageSize, page);
    console.log(page)

    this.teamsService.getTeamsPaged(params).subscribe(pagedResult => {
      this.totalRecords = pagedResult.totalRecords;
      this.pageSize = pageSize;
      this.pageIndex = page -1;
      this.dataSource.data = pagedResult.data;
    });
  }
}
