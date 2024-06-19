import {  Injectable      } from '@angular/core';
import {  environment     } from "../../environments/environment";
import {  HttpClient, HttpHeaders} from "@angular/common/http";
import {  Router          } from "@angular/router";
import {  InsertAssignment} from '../models/assignmentinsert';
import {  Assignment      } from '../models/assignment';


@Injectable({
  providedIn: 'root'
})
export class AssignmentService {
  url: string = environment.BACKEND_API_URL;

  constructor(private http: HttpClient, private router: Router) { }

  public assignProject(assignment: Assignment) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Access-Control-Allow-Origin': '*'
      })
    }
    return this.http.post(`${this.url}/api/employeesxtasks/insertAssignment`, assignment, httpOptions)
  }

  public deleteAssignment(assignment: InsertAssignment) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Access-Control-Allow-Origin': '*'
      })
    }
    return this.http.delete(`${this.url}/api/employeesxtasks/removeAssignment/${assignment.username}/${assignment.title}`, httpOptions)
  }

  public getAllAssignments() {
    const httpOptions = {
      headers: new HttpHeaders({
        'Access-Control-Allow-Origin': '*'
      })
    }
    return this.http.get(`${this.url}/api/employeesxtasks/getAllAssignmentsFormatted`, httpOptions)
  }
}
