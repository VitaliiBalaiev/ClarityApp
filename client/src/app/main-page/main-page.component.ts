import {Component, OnInit} from '@angular/core';
import {User} from "../_models/user";
import {AccountService} from "../_services/account.service";
import {NgIf} from "@angular/common";
import {Router} from "@angular/router";
import {ChatComponent} from "../chat/chat.component";
import {SearchbarComponent} from "../searchbar/searchbar.component";
import {FormsModule} from "@angular/forms";

@Component({
  selector: 'main-page',
  standalone: true,
  imports: [
    NgIf,
    ChatComponent,
    SearchbarComponent,
    FormsModule
  ],
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.css'
})
export class MainPageComponent implements OnInit {
  currentUser: User | null = null

  constructor(private accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
    this.accountService.currentUser$.subscribe(user => {
      this.currentUser = user;
    });
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/login').then(() => {
      console.log("Successfully logged out");
    });
  }

}
