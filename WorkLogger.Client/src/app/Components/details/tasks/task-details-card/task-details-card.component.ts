import { Component, Inject, OnInit } from '@angular/core';
import {FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UserTaskDetails } from '../../../../models/userTaskDetails.model';
import { TaskService } from '../../../../services/task.service';
import { UpdateTaskDto } from '../../../../DTOs/updateTaskDto.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-task-details-card',
  templateUrl: './task-details-card.component.html',
  styleUrl: './task-details-card.component.css'
})
export class TaskDetailsCardComponent implements OnInit
{
  constructor(@Inject(MAT_DIALOG_DATA) public data: any, private taskService: TaskService,
              private toastrService: ToastrService,)
  {}

  ngOnInit(): void
  {
    console.log("test")
    this.loadTaskDetails()
  }

  loadTaskDetails()
  {
    this.taskService.getTaskDetails(this.data.taskId).subscribe(
      (response: UserTaskDetails)=> this.updateValues(response),
      (error) => console.log(error))
  }

  updateValues(dto: UserTaskDetails)
  {
    console.log(dto)
    this.editTaskForm.patchValue({
      taskName: dto.name,
      taskDesc: dto.description,
      team: dto.team.name,
      assignedUser: dto.assignedUser.name + " " + dto.assignedUser.surname,
      author: dto.author.name + " " + dto.author.surname,
      loggedHours: dto.loggedHours || 0
    });
  }

  editTaskForm = new FormGroup(
    {
      taskName: new FormControl('', Validators.required),
      taskDesc: new FormControl('', Validators.required),
      team: new FormControl('', Validators.required),
      assignedUser: new FormControl({value: '', disabled: true}, Validators.required),
      author: new FormControl({value: '', disabled: true}, Validators.required),
      loggedHours: new FormControl({value: 0, disabled: false}, Validators.required),
    }
  )
  onSave()
  {

    const dto: UpdateTaskDto =
      {
        id: this.data.taskId,
        name: this.editTaskForm.get("taskName")?.value || "",
        description: this.editTaskForm.get("taskDesc")?.value || "",
        // @ts-ignore
        LoggedHours: parseFloat(this.editTaskForm.get("loggedHours")?.value)  || 0
      }

      this.taskService.updateTask(dto).subscribe(
        response => this.toastrService.success("Updated succesfuly", "Succes"),
        (error) => this.toastrService.error(error.error, "Error")
      )
  }
}
