import {  Injectable  } from '@angular/core';
import {  environment } from "../../environments/environment";
import {  HttpClient, HttpHeaders} from "@angular/common/http";
import {  Router      } from "@angular/router";
import {  UserLogin   } from '../models/user-login';
import {  Employee    } from '../models/employee';
import {  EmployeeDisplay } from '../models/employeedisplay';
import {  EmployeeUpdate  } from '../models/employeeupdate';
import {  InsertEmployee  } from '../models/employeeinsert';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  url: string = environment.BACKEND_API_URL;
  constructor(private http: HttpClient, private router: Router) { }

  public getAllEmployees() {
    const httpOptions = {
      headers: new HttpHeaders({
        'Access-Control-Allow-Origin': '*'
      })
    }
    return this.http.get<Employee[]>(`${this.url}/api/employees/getAllEmployees`, httpOptions)
  }

  public getAllEmployeesPresenter() {
    const httpOptions = {
      headers: new HttpHeaders({
        'Access-Control-Allow-Origin': '*'
      })
    }
    return this.http.get<EmployeeDisplay[]>(`${this.url}/api/employees/getAllEmployeesPresenter`, httpOptions)
  }

  public insertEmployee(employee: InsertEmployee) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Access-Control-Allow-Origin': '*'
      })
    }
    return this.http.post(`${this.url}/api/employees/signupEmployee`, employee, httpOptions)
  }

  public updateEmployee(employee: EmployeeUpdate) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Access-Control-Allow-Origin': '*'
      })
    }
    return this.http.put(`${this.url}/api/employees/updateEmployee`, employee, httpOptions)
  }

  public getEmployee(id: number) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Access-Control-Allow-Origin': '*'
      })
    }
    return this.http.get<Employee>(`${this.url}/api/employees/getEmployeeById/${id}}`, httpOptions)
  }

  public deleteEmployee(id: number, username: string) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Access-Control-Allow-Origin': '*'
      })
    }
    return this.http.delete(`${this.url}/api/employees/deleteEmployee/${id}/${username}`, httpOptions)
  }


  public getEmployeeByEmail(email: string) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Access-Control-Allow-Origin': '*'
      })
    }
    return this.http.get<Employee>(`${this.url}/api/employees/getEmployeeByEmail/${email}`, httpOptions)
  }

  public getUserById(id: number) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Access-Control-Allow-Origin': '*'
      })
    }
    return this.http.get<UserLogin>(`${this.url}/api/users/getUserById/${id}`, httpOptions)    
  }
}
