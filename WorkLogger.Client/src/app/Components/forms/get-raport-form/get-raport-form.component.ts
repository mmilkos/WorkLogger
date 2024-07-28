import { Component } from '@angular/core';
import { TeamService } from '../../../services/team.service';
import { GetSummaryFileDto } from '../../../DTOs/getSummaryFileDto';
import { HttpResponse } from '@angular/common/http';
import { Team } from '../../../models/team.model';
import { ToastrService } from 'ngx-toastr';
import {FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-get-raport-form',
  templateUrl: './get-raport-form.component.html',
  styleUrl: './get-raport-form.component.css'
})
export class GetRaportFormComponent
{
  teamsNames: Team[] = [];
  constructor(private teamService: TeamService,
              private toastrService: ToastrService)
  {
    this.getTeamsNames()
  }

  getRaportForm = new FormGroup(
    {
      team: new FormControl(),
      startDate: new FormControl(),
      endDate: new FormControl()
    });

  getTeamsNames()
  {
    this.teamService.getTeamsNames().subscribe(
      teamsDto => this.teamsNames = teamsDto.teams,
      error => this.toastrService.error("There was an error with loading teams names", "Error")
    )
  }

  getSummaryFile(dto: GetSummaryFileDto)
  {
    this.teamService.getSummaryFile(dto).subscribe(
      (response: HttpResponse<Blob>) => {
        if (response.body) {
          let blob = new Blob([response.body], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
          let url = window.URL.createObjectURL(blob);
          let contentDisposition = response.headers.get('Content-Disposition') || '';
          let fileName = contentDisposition.split(';')[1].split("=")[1];
          fileName = fileName.replace(/\./g, "-")
          let link = document.createElement('a');
          link.href = url;
          link.download = fileName;
          link.click();
        } else console.log("No response body");
      },
      (error) => this.toastrService.error("There was an error with downloading file", "Error")
    );
  }

  onSubmit()
  {
    var dto: GetSummaryFileDto =
      {
        // @ts-ignore
        teamId: +this.getRaportForm.get("team").value || 0,
        // @ts-ignore
        startDate: this.getRaportForm.get("startDate").value || 0,
        // @ts-ignore
        endDate:  this.getRaportForm.get("endDate").value || 0
      }

    this.getSummaryFile(dto)
  }
}
