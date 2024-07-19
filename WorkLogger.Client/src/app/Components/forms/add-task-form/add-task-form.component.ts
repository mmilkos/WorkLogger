import { Component, OnDestroy, OnInit } from '@angular/core';
import {FormControl, FormGroup, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { TeamService } from '../../../services/team.service';
import { Team } from '../../../models/team.model';
import { ToastrService } from 'ngx-toastr';
import { UserIdAndName } from '../../../models/UserIdAndName.model';
import { AddTaskDto } from '../../../DTOs/addTaskDto.model';
import { AccountService } from '../../../services/account.service';
import { TaskService } from '../../../services/task.service';

@Component({
  selector: 'app-add-task-form',
  templateUrl: './add-task-form.component.html',
  styleUrl: './add-task-form.component.css'
})
export class AddTaskFormComponent implements OnDestroy, OnInit
{
  isFormValidValue = false;
  teamsNames: Team[] = [];
  teamMembers: UserIdAndName[] = [];

  constructor(private teamService: TeamService,
              private toastrService: ToastrService,
              private accountService: AccountService,
              private taskService: TaskService)
  {
    this.isFormValid();
    this.getTeamsNames();
  }

  ngOnInit(): void {
    // @ts-ignore
    this.addTaskForm.get('team').valueChanges.subscribe(selectedTeam => {
      if (!selectedTeam) return;

      // @ts-ignore
      this.addTaskForm.get('assignedUser').enable();
      this.updateTeamMembers(selectedTeam);
    });
  }

  updateTeamMembers(selectedTeam: any) : void {
    this.teamService.getTeamMembersNames(selectedTeam).subscribe(
      response => {

        this.teamMembers = response.names.map((name: any) => ({
          id: name.id,
          name: name.name,
          surname: name.surname
        }));
      }
    );
  }


  private statusSubscription?: Subscription;

  addTaskForm = new FormGroup(
    {
      taskName: new FormControl('', Validators.required),
      taskDesc: new FormControl('', Validators.required),
      team: new FormControl('', Validators.required),
      assignedUser: new FormControl({value: '', disabled: true}, Validators.required),
    }
  )

  isFormValid()
  {
    this.statusSubscription = this.addTaskForm.statusChanges.subscribe(status => {
      if (status == 'VALID') this.isFormValidValue = true;
      else this.isFormValidValue = false;
    });
  }

  getTeamsNames()
  {
    this.teamService.getTeamsNames().subscribe(
      teamsDto =>
      {
        this.teamsNames = teamsDto.teams;
      },
      error => this.toastrService.error("There was an error with loading teams names", "Error")
    )
  }

  onSubmit()
  {

    let dto: AddTaskDto =
      {
        // @ts-ignore
        assignedUserId: +this.addTaskForm.get("assignedUser").value || 0,
        // @ts-ignore
        authorId: +this.accountService.currentUser.id,
        // @ts-ignore
        teamId: +this.addTaskForm.get("team").value || 0,
        // @ts-ignore
        name: this.addTaskForm.get("taskName").value || "",
        // @ts-ignore
        description: this.addTaskForm.get("taskDesc").value || ""
      }

      this.taskService.addTask(dto).subscribe(
        ()=>
        {
          this.toastrService.success("Added succesfuly", "Succes")
          this.addTaskForm.reset()

        } ,
        error => this.toastrService.error(error.error, "Error")
      )
  }
  ngOnDestroy() {
    if (this.statusSubscription) {
      this.statusSubscription.unsubscribe();
    }
  }
}
