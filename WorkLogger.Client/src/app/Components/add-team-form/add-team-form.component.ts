import {Component, OnInit} from '@angular/core';
import {ToastrService} from "ngx-toastr";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import { TeamService } from '../../services/team.service';
import { CreateTeamDto } from '../../DTOs/createTeamDto';

@Component({
  selector: 'app-add-team-form',
  templateUrl: './add-team-form.component.html',
  styleUrl: './add-team-form.component.css'
})
export class AddTeamFormComponent implements OnInit
{
  isFormValidValue = false;

  constructor(private teamService: TeamService, private toastrService : ToastrService) {}

  ngOnInit(): void
  {
    this.isFormValid()
  }

  addTeamForm = new FormGroup(
    {
      teamName: new FormControl('', Validators.required),
    })

  isFormValid()
  {
    this.addTeamForm.statusChanges.subscribe(status => {
      if (status == 'VALID') this.isFormValidValue = true;
      else this.isFormValidValue = false;
    });
  }

  onSubmit()
  {
    var dto : CreateTeamDto =
      {
        name: this.addTeamForm.get('teamName')?.value || "",
      }
    this.teamService.createTeam(dto).subscribe(
      () =>
      {
        this.toastrService.success("Added team", "Succes")

      },
      (error) => {
        this.toastrService.error(error.error, "Error");
      })
  }
}
