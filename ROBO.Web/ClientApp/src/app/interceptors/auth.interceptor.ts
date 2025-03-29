import { inject } from '@angular/core';
import { HttpRequest, HttpHandlerFn, HttpEvent, HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { TokenStorageService } from '../services/token.storage.service';

const TOKEN_HEADER_KEY = 'Authorization';

export const authInterceptor: HttpInterceptorFn = (req: HttpRequest<any>, next: HttpHandlerFn): Observable<HttpEvent<any>> => {
  const tokenStorage = inject(TokenStorageService);
  const router = inject(Router);

  let authReq = req;
  const token = tokenStorage.getToken();
  if (token != null) {
    authReq = req.clone({
      headers: req.headers.set(TOKEN_HEADER_KEY, 'Bearer ' + token),
      withCredentials: true
    });
  } else {
    authReq = req.clone({
      withCredentials: true
    });
  }

  return next(authReq).pipe(
    catchError((err: HttpErrorResponse) => {
      if (err.status === 401) {
        tokenStorage.signOut();
        router.navigateByUrl(`/`);
      }
      return throwError(err);
    })
  );
};
