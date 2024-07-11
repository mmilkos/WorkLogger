import {Component, OnDestroy, OnInit} from '@angular/core';
import {ToastrService} from "ngx-toastr";
import { TeamService } from '../../../services/team.service';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import { AccountService } from '../../../services/account.service';
import {RegisterUserDto} from "../../../DTOs/RegisterUserDto";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-add-user-form',
  templateUrl: './add-user-form.component.html',
  styleUrl: './add-user-form.component.css'
})
export class AddUserFormComponent implements OnInit, OnDestroy
{
  isFormValidValue = false;
  private statusSubscription?: Subscription;
  constructor(private teamService: TeamService,
              private accountService: AccountService,
              private toastrService : ToastrService) {}

  ngOnInit(): void
  {
    this.isFormValid()
  }

  addUserForm = new FormGroup(
    {
      name: new FormControl('', Validators.required),
      surname: new FormControl('', Validators.required),
      username: new FormControl('', Validators.required),
      role: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
    })

  isFormValid()
  {
    this.statusSubscription = this.addUserForm.statusChanges.subscribe(status => {
      console.log(status)
      if (status == 'VALID') this.isFormValidValue = true;
      else this.isFormValidValue = false;
    });
  }

  onSubmit()
  {
    let dto: RegisterUserDto =
      {
        name: this.addUserForm.controls.name.value || "",
        surname: this.addUserForm.controls.surname.value || "",
        username: this.addUserForm.controls.username.value || "",
        role: this.addUserForm.controls.role.value || "",
        password: this.addUserForm.controls.password.value || "",
      }

    this.accountService.registerUser(dto).subscribe(
      () => {
        this.toastrService.success('User added successfully', "Succes");
        this.addUserForm.reset();
        },
      (error) => error.error.forEach((error: string) => this.toastrService.error(error, 'Error')))
  }

  ngOnDestroy() {
    if (this.statusSubscription) {
      this.statusSubscription.unsubscribe();
    }
  }
}
