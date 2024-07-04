import { Component } from '@angular/core';
import {NavigationEnd, Router} from '@angular/router';
import {filter} from "rxjs";
import { RoutesEnum } from './enums/RoutesEnum';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'WorkLogger.Client';
  isCenter: boolean = true;

  constructor(private router: Router)
  {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((event: any) =>
    {
      this.isCenter = event.urlAfterRedirects != '/' + RoutesEnum.Teams
    })
  }

}
