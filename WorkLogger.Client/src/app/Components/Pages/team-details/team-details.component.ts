import { Component } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-team-details',
  templateUrl: './team-details.component.html',
  styleUrl: './team-details.component.css'
})
export class TeamDetailsComponent
{
  teamId : number | null = null
  constructor( private route: ActivatedRoute)
  {
    // @ts-ignore
    this.teamId = +this.route.snapshot.paramMap.get('id') || null;
  }

}
