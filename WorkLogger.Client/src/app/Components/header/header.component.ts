import { Component } from '@angular/core';
import {FormControl, FormGroup} from "@angular/forms";
import { AccountService } from '../../Services/account.service';
import { LoginUserDto } from '../../DTOs/loginDto.model';
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent
{

  constructor(private accountService: AccountService, private toastrService : ToastrService) {
  }

  loginForm = new FormGroup(
    {
      username: new FormControl(''),
      password: new FormControl('')
    })

  login()
  {
    var dto : LoginUserDto =
      {
        username: this.loginForm.get('username')?.value || "",
        password: this.loginForm.get('password')?.value || ""
      }
    this.accountService.login(dto).subscribe(
      ()=>
      {
        this.toastrService.success("Logged in", "Succes")
      },
      (error) => {
        error.error.forEach((error: string) =>
        {
          this.toastrService.error(error, 'Error');
        })
      }
    );
  }
}
