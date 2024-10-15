import { Injectable } from '@angular/core';
import {BehaviorSubject} from "rxjs";
import {User} from "../_models/user";

@Injectable({
  providedIn: 'root'
})
export class SharedResourcesService {
  private sharedResource = new BehaviorSubject<User[]>([]);
  currentResource = this.sharedResource.asObservable();

  constructor() { }

  updateUsers(users: User[]) {
    this.sharedResource.next(users);
  }
}
