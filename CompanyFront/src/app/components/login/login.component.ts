import {    Component         } from '@angular/core';
import {    CommonModule      } from '@angular/common';
import {    FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {    MatInputModule    } from "@angular/material/input";
import {    MatCardModule     } from "@angular/material/card";
import {    Router, RouterLink} from "@angular/router";
import {    MatCheckboxModule } from "@angular/material/checkbox";
import {    UserLogin         } from "../../models/user-login";
import {    AuthService       } from "../../services/auth.service";

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatInputModule, MatCardModule, RouterLink, MatCheckboxModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm: FormGroup

  constructor(
    private formBuilder: FormBuilder, private authService: AuthService,
    private router: Router
  ) {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.pattern('^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*\\W).{8,}$')]],
      keepLoggedIn: ['']
    })
  }

  loginUser() {
    const userLogin: UserLogin = {
      username: this.loginForm.controls['username'].value,
      password: this.loginForm.controls['password'].value,
      keepLoggedIn: this.loginForm.controls['keepLoggedIn'].value
    }

    if(userLogin.username === 'admin') {
      this.authService.loginUser(userLogin).subscribe({
        complete: () => this.router.navigateByUrl('/admin/home'),
        next: (res) => {
          const loggedInUser = res as UserLogin
          this.authService.makeLoginSession(loggedInUser)
        },
        error: (err) => console.log(err)
      })       
    } else {
      this.authService.loginUser(userLogin).subscribe({
        complete: () => this.router.navigateByUrl('/employee/home'),
        next: (res) => {
          const loggedInUser = res as UserLogin
          this.authService.makeLoginSession(loggedInUser)
        },
        error: (err) => console.log(err)
      })  
    }
  }
}
