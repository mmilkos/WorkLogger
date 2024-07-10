import { Component } from '@angular/core';
import { RoutesEnum } from '../../enums/RoutesEnum';

@Component({
  selector: 'app-side-bar',
  templateUrl: './side-bar.component.html',
  styleUrl: './side-bar.component.css'
})
export class SideBarComponent
{
  routesEnum = RoutesEnum

}
