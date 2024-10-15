import {Component, EventEmitter, Output} from '@angular/core';
import {FormsModule} from "@angular/forms";
import {UserService} from "../_services/user.service";
import {User} from "../_models/user";
import {SharedResourcesService} from "../_services/shared-resources.service";

@Component({
  selector: 'app-searchbar',
  standalone: true,
  imports: [
    FormsModule
  ],
  templateUrl: './searchbar.component.html',
  styleUrl: './searchbar.component.css'
})

export class SearchbarComponent {
  public searchProperty = '';
  public users: User[] = [];

  @Output() searchResults = new EventEmitter<User[]>();
  constructor(private userService: UserService, private sharedResourcesService: SharedResourcesService) { }

  onSearch(event: any){
    this.searchProperty = event.target.value;
    console.log(this.searchProperty);
    this.searchUser();
  }
  searchUser(){
    if(this.searchProperty.trim() !== ""){
      this.userService.searchUser(this.searchProperty).subscribe((users: User[]) => {
        this.users = users;
        console.log(users);
        this.sharedResourcesService.updateUsers(this.users);
      })
    } else {
      this.users = [];
      this.sharedResourcesService.updateUsers(this.users); //if the input bar is empty, list of displayed users is empty too
    }
  }
}

