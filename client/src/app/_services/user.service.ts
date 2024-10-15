import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import {User} from "../_models/user";
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseApiUrl: string = 'http://localhost:5045/api/';
  constructor(private http: HttpClient) { }

  searchUser(username: string): Observable<User[]> {
    return this.http.get<User[]>(`${this.baseApiUrl}users/${username}`);
  }
}
