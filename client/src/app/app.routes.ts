import { Routes } from '@angular/router';
import { LoginPageComponent } from "./login-page/login-page.component";
import {RegisterPageComponent} from "./register-page/register-page.component";
import {MainPageComponent} from "./main-page/main-page.component";

export const routes: Routes = [
  {path: '', redirectTo: '/login', pathMatch: 'full'},
  {path: 'login', component: LoginPageComponent},
  {path: 'register', component: RegisterPageComponent},
  {path: 'main', component: MainPageComponent},
];

