import { Component } from '@angular/core';
import {FormControl, FormGroup} from "@angular/forms";
import { LoginUserDto } from '../../DTOs/loginDto.model';
import {ToastrService} from "ngx-toastr";
import {Router} from "@angular/router";
import { RoutesEnum } from '../../enums/RoutesEnum';
import { AccountService } from '../../services/account.service';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent
{
  routesEnum = RoutesEnum


  constructor(private accountService: AccountService,
              private toastrService : ToastrService,
              private router: Router) {
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
      response =>
      {
        this.accountService.setToken(response.jwtToken);
        this.toastrService.success("Logged in", "Succes")
        this.router.navigateByUrl('/' + RoutesEnum.Teams)
        this.loginForm.reset();
      },
      error => error.error.forEach((error: string) => this.toastrService.error(error, 'Error'))
    );
  }
  logOut()
  {
    this.accountService.logOut();
    this.toastrService.success("Logged out", "Success");
    this.router.navigateByUrl('/' + RoutesEnum.Register);
  }

  get isLoggedIn(): boolean {
    return this.accountService.isLoggedIn;
  }
}
