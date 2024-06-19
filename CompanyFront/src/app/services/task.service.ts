import { Injectable } from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Router} from "@angular/router";
import { Task } from '../models/task';


import { TaskInsert } from '../models/taskInsert';
import { TaskUpdate } from '../models/taskupdate';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  url: string = environment.BACKEND_API_URL;
  constructor(private http: HttpClient, private router: Router) { }

  public insertTask(task: TaskInsert) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Access-Control-Allow-Origin': '*'
      })
    }
    return this.http.post<TaskInsert>(`${this.url}/api/tasks/insertTask`, task, httpOptions)
  }

  public updateProject(updateTask: TaskUpdate) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Access-Control-Allow-Origin': '*'
      })
    }
    return this.http.put<TaskUpdate>(`${this.url}/api/tasks/updateTask`, updateTask, httpOptions)
  }

  public getTaskByTitle(title: string) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Access-Control-Allow-Origin': '*'
      })
    }
    return this.http.get<Task>(`${this.url}/api/tasks/getTaskByTitle/${title}`, httpOptions)
  }

  public deleteProject(id: number) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Access-Control-Allow-Origin': '*'
      })
    }
    return this.http.delete<Task>(`${this.url}/api/tasks/deleteTask/${id}`, httpOptions)
  }

  public getAllTasks() {
    const httpOptions = {
      headers: new HttpHeaders({
        'Access-Control-Allow-Origin': '*'
      })
    }
    return this.http.get<Task[]>(`${this.url}/api/tasks/getAllTasks`, httpOptions)
  }

  public getAllTasksOfUser(userId: number) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Access-Control-Allow-Origin': '*'
      })
    }
    return this.http.get<Task[]>(`${this.url}/api/employeesxtasks/getAllAssignedTasksOfUser/${userId}`, httpOptions)
  }
}
