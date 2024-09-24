import {Component, OnInit} from '@angular/core';
import {AccountService} from "../_services/account.service";
import {RouterLink, RouterOutlet} from "@angular/router";
import {FormsModule} from "@angular/forms";
import {HttpClientModule} from "@angular/common/http";

@Component({
  selector: 'register-page',
  standalone: true,
  imports: [RouterOutlet, FormsModule, HttpClientModule, RouterLink],
  templateUrl: './register-page.component.html',
  styleUrl: './register-page.component.css'
})
export class RegisterPageComponent implements OnInit{
  model: any = {}
  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  }

  register() {
    this.accountService.register(this.model).subscribe({
      next: response => {
        console.log(response);
      },
      error: error => {
        console.error(error); // Log the error
      }
    });
  }
}
