import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import {User} from "../_models/user";
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseApiUrl: string = 'http://localhost:5045/api/';
  private userObj: User | null = null;
  constructor(private http: HttpClient) {
    this.loadCurrentUser();
  }

  loadCurrentUser(): void {
    const userData = localStorage.getItem('user');
    this.userObj = userData ? JSON.parse(userData) : null;
  }

  getCurrentUser(): User | null {
    return this.userObj;
  }

  updateUser(user: User): void {
    this.userObj = user;
    localStorage.setItem('user', JSON.stringify(user));
  }
  searchUser(username: string): Observable<User[]> {
    return this.http.get<User[]>(`${this.baseApiUrl}users/${username}`);
  }
}
