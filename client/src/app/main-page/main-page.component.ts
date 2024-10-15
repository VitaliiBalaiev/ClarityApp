import {Component, OnInit} from '@angular/core';
import {User} from "../_models/user";
import {AccountService} from "../_services/account.service";
import {NgIf} from "@angular/common";
import {Router} from "@angular/router";
import {ChatComponent} from "../chat/chat.component";
import {SearchbarComponent} from "../searchbar/searchbar.component";

@Component({
  selector: 'main-page',
  standalone: true,
    imports: [
        NgIf,
        ChatComponent,
        SearchbarComponent
    ],
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.css'
})
export class MainPageComponent implements OnInit {
  currentUser: User | null = null;
  public foundUsers: User[] = [];

  constructor(private accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
    // Subscribe to the current user observable
    this.accountService.currentUser$.subscribe(user => {
      this.currentUser = user; // Update current user on login/logout
    });
  }

  handleSearchResults(users: User[]) {
    this.foundUsers = users; // Store the found users
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/login').then(() => {
      console.log("Successfully logged out");
    });
  }

}
