import {Component, OnInit} from '@angular/core';
import {RouterLink, RouterOutlet} from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AccountService } from "../_services/account.service";
import { HttpClientModule } from "@angular/common/http";

@Component({
  selector: 'login-page',
  standalone: true,
  imports: [RouterOutlet, FormsModule, HttpClientModule, RouterLink],
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.css'
})
export class LoginPageComponent implements OnInit {
  model: any = {}
  loggedIn: boolean;
  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  }

  login() {
    this.accountService.login(this.model).subscribe({
      next: response => {
        console.log(response);
        this.loggedIn = true;
      },
      error: error => {
        console.error(error); // Log the error
      }
    });
  }

}
