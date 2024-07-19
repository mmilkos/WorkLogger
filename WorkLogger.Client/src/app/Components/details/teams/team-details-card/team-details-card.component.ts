import {Component, Input, OnInit} from '@angular/core';
import {HttpParams} from "@angular/common/http";
import { TeamDetails } from '../../../../models/teamDetails.model';
import { TeamService } from '../../../../services/team.service';

@Component({
  selector: 'app-team-details-card',
  templateUrl: './team-details-card.component.html',
  styleUrl: './team-details-card.component.css'
})
export class TeamDetailsCardComponent implements OnInit
{
  @Input() teamId :null | number = 0;
  teamDetails : TeamDetails =
    {
    teamId: 0,
    name: '',
    manager: undefined
  };

  managerName: string = "";

  constructor(private teamService: TeamService) {}

  ngOnInit(): void
  {
    // @ts-ignore
    this.teamService.getTeamDetails(this.teamId).subscribe(details =>
    {
      this.teamDetails = details;
      if (this.teamDetails.manager != undefined)
        this.managerName = this.teamDetails.manager?.name + " " + this.teamDetails.manager?.surname
    });
  }
}
