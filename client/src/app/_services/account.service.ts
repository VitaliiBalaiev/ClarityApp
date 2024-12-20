import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { map } from 'rxjs/operators';
import {User} from "../_models/user";
import {ReplaySubject} from "rxjs";
import {Router} from "@angular/router";
import {UserService} from "./user.service";

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = 'http://localhost:5045/api/';
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();
  constructor(private http: HttpClient, private router: Router, private userService: UserService) { }

  login(model: any){
    return this.http.post(`${this.baseUrl}account/login`, model).pipe(
      map((response: User) => {
        const user = response;
        if(user){
          localStorage.setItem("user", JSON.stringify(user));
          this.currentUserSource.next(user);
          this.router.navigate(['/main']);
          this.userService.updateUser(user);
        }
      })
    );

  }

  setCurrentUser(model: User){
    this.currentUserSource.next(model);
  }
  logout(){
    localStorage.removeItem("user");
    localStorage.removeItem("activeChatUser");
    this.currentUserSource.next(null);
  }
  register(model: any){
    return this.http.post(`${this.baseUrl}account/register`, model);
  }
}
