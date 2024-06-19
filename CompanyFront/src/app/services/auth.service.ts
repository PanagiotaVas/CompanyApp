import {  Injectable             } from '@angular/core';
import {  environment            } from "../../environments/environment";
import {  HttpClient, HttpHeaders} from "@angular/common/http";
import {  UserLogin              } from "../models/user-login";
import {  UserRegister           } from "../models/user-register";
import {  Router                 } from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  url: string = environment.BACKEND_API_URL;
  session: any = []

  constructor(private http: HttpClient, private router: Router) {
    let session: any = localStorage.getItem('session');
    if (session) {
      session = JSON.parse(session)
    }
    this.session = session as UserLogin
  }

  public loginUser(userCredentials: UserLogin) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Access-Control-Allow-Origin': '*'
      })
    }
    return this.http.post(`${this.url}/api/users/login`, userCredentials, httpOptions)
  }


  public registerUser(userCredentials: UserRegister) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Access-Control-Allow-Origin': '*'
      })
    }
    return this.http.post(`${this.url}/api/employees/signupEmployee`, userCredentials, httpOptions)
  }

  public makeLoginSession(loginUser: UserLogin) {
    this.session = loginUser
    localStorage.setItem('session', JSON.stringify(this.session))
  }

  public logout() {
    this.session = undefined;
    localStorage.removeItem('session');
    this.router.navigateByUrl('/');
  }
}
