import {Component, OnDestroy, OnInit} from '@angular/core';
import {ToastrService} from "ngx-toastr";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import { CreateTeamDto } from '../../../DTOs/createTeamDto';
import { TeamService } from '../../../services/team.service';
import { Subscription } from 'rxjs';
import { CommonService } from '../../../services/common.service';

@Component({
  selector: 'app-add-team-form',
  templateUrl: './add-team-form.component.html',
  styleUrl: './add-team-form.component.css'
})
export class AddTeamFormComponent implements OnInit, OnDestroy
{
  isFormValidValue = false;
  private statusSubscription?: Subscription;

  constructor(private teamService: TeamService,
              private toastrService : ToastrService,
              private commonService: CommonService) {}

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
    this.statusSubscription = this.addTeamForm.statusChanges.subscribe(status => {
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
        this.toastrService.success("Added team", "Succes");
        this.addTeamForm.reset();
        this.commonService.refresh()
      },
      (error) => error.error.forEach((error: string) => this.toastrService.error(error, 'Error')))
  }

  ngOnDestroy() {
    if (this.statusSubscription) {
      this.statusSubscription.unsubscribe();
    }
  }
}
