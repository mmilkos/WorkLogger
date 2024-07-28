import {HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, catchError, of, throwError } from 'rxjs';

@Component({
  selector: 'app-forbidden-interceptor',
  templateUrl: './forbidden-interceptor.component.html',
  styleUrl: './forbidden-interceptor.component.css'
})
export class ForbiddenInterceptorComponent implements HttpInterceptor
{
  constructor(private toastrService: ToastrService,
              private router: Router) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe( catchError(
      (err: HttpErrorResponse) =>
      {
        this.toastrService.error('Forbidden', 'Error');
        this.router.navigate([""])
        return throwError(err);
      }));
  }
}
