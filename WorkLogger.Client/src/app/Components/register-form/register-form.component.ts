import {Component, OnInit} from '@angular/core';
import {FormGroup, FormControl, Validators, AbstractControl} from '@angular/forms';
import { CompanyService } from '../../services/company.service';
import { RegisterCompanyDto } from '../../DTOs/registerCompanyDto';
import { ToastrService } from "ngx-toastr";
import {error} from "@angular/compiler-cli/src/transformers/util";

@Component({
  selector: 'app-register-form',
  templateUrl: './register-form.component.html',
  styleUrls: ['./register-form.component.css',   ]
})
export class RegisterFormComponent implements OnInit
{

  isFormValidValue = false;
  constructor(private companyService: CompanyService, private toastrService : ToastrService) {}

  ngOnInit(): void {
    this.isFormValid()
  }

  registerForm = new FormGroup({
    companyName: new FormControl('', Validators.required),
    firstName: new FormControl('', Validators.required),
    lastName: new FormControl('', Validators.required),
    username: new FormControl('', Validators.required),
    password: new FormControl('', [Validators.required, Validators.min(8)]),
    confirmPassword: new FormControl(''),
  }, this.passwordMatchValidator);

  isFormValid()
  {
    this.registerForm.statusChanges.subscribe(status => {
      if (status == 'VALID') this.isFormValidValue = true;
      else this.isFormValidValue = false;
    });
  }

  passwordMatchValidator(form: AbstractControl)
  {
    let passwordControl = form.get('password');
    let confirmPasswordControl = form.get('confirmPassword');

    if (passwordControl && confirmPasswordControl) {
      return passwordControl.value === confirmPasswordControl.value
        ? null
        : { mismatch: true };
    }

    return null;
  }

  onSubmit()
  {
    this.registerForm.markAllAsTouched();

    var dto: RegisterCompanyDto =
      {
        companyName: this.registerForm.get('companyName')?.value || "",
        name: this.registerForm.get('firstName')?.value || "",
        surname: this.registerForm.get('lastName')?.value || "",
        userName: this.registerForm.get('username')?.value || "",
        password: this.registerForm.get('password')?.value || "",
      };

    this.companyService.registerCompanyWithCeo(dto).subscribe(
      () => this.toastrService.success('Registered corectly', 'Succes'),

      error => error.error.forEach((error: string) => this.toastrService.error(error, 'Error'))
    );
  }
}
