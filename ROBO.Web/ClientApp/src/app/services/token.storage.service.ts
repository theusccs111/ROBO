import { Injectable } from '@angular/core';

const ROBO_KEY = 'auth';
const TOKEN_KEY = 'auth-token';

@Injectable({
  providedIn: 'root'
})

export class TokenStorageService {

  constructor() { }

  public getRobo(): any {
    const robo = sessionStorage.getItem(ROBO_KEY);
    if (robo)
      return JSON.parse(robo);
  }

  public async setRobo(user: any): Promise<any> {
    window.sessionStorage.removeItem(ROBO_KEY);
    window.sessionStorage.setItem(ROBO_KEY, JSON.stringify(user));
  }

  public getToken(): any {
    return sessionStorage.getItem(TOKEN_KEY);
  }

  setToken(token: string): void {
    window.sessionStorage.removeItem(TOKEN_KEY);
    window.sessionStorage.setItem(TOKEN_KEY, token);
  }

  loggedIn() {
    if (this.getToken()) {
      return true;
    } else {
      return false;
    }
  }

  signOut() {
    window.sessionStorage.clear();
  }
}
