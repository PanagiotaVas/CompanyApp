import {  Component, OnInit   } from '@angular/core';
import {  CommonModule        } from '@angular/common';
import {  EmployeeService     } from "../../../services/employee.service";
import {  MatTableDataSource, MatTableModule} from "@angular/material/table";
import {  MatCardModule       } from "@angular/material/card";
import {  MatDialog           } from "@angular/material/dialog";
import {  CreateEmployeeComponent} from "./create-employee/create-employee.component";
import {  EditEmployeeComponent} from "./edit-employee/edit-employee.component";
import {  MessagesService     } from "../../../services/messages.service";
import {  User                } from 'src/app/models/user';
import {  Employee            } from 'src/app/models/employee';
import {  EmployeeDisplay     } from 'src/app/models/employeedisplay';

@Component({
  selector: 'app-employees',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatCardModule],
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.css']
})
export class EmployeesComponent implements OnInit {
  employees: EmployeeDisplay[] = []
  displayColumns: string[] = [];
  dataSource: MatTableDataSource<EmployeeDisplay> = new MatTableDataSource<EmployeeDisplay>();

  constructor(
    private employeeService: EmployeeService, public dialog: MatDialog, private messagesService: MessagesService
  ) {}

  ngOnInit() {
    this.onLoadPage()
  }

  onLoadPage() {
    this.employeeService.getAllEmployeesPresenter().subscribe({
      complete: () => {
        this.dataSource = new MatTableDataSource<EmployeeDisplay>(this.employees);
        this.displayColumns = ['firstname', 'lastname', 'email', 'phoneNumber', 'username', 'actions']
      },
      next: (res) => {
        this.employees = res as EmployeeDisplay[]
        console.log(this.employees)
      },
      error: (err) => console.log(err)
    })
  }

  openCreateEmployeeModal() {
    const dialogRef = this.dialog.open(CreateEmployeeComponent, {
      width: '800px',
      autoFocus: false
    })

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        window.location.reload()
      }
    })
  }

  openEditModal(employee: Employee) {
    const dialogRef = this.dialog.open(EditEmployeeComponent, {
      data: employee,
      width: '800px',
      autoFocus: false
    })

    const sub = dialogRef.componentInstance.isUpdated.subscribe((data) => {
      if (!data) {
        dialogRef.close()
        return;
      }

      this.messagesService.showSuccessMessage('Success!', 'The employee was successfully updated!')
      dialogRef.close()
      window.location.reload()
    })
  }
  

  deleteEmployee(employee: Employee) {
    if (confirm(`Delete employee ${employee.firstname} ${employee.lastname}?`)) {
      this.employeeService.getUserById(employee.userId).subscribe({
        complete: () => {},
        next: (user: User) => {
           let username: string= user.username;

           this.employeeService.deleteEmployee(employee.id, username).subscribe({
            next: () => {},
            error: (error) => console.log(error)
          });           

        // Remove deleted employee from the local array
        this.employees = this.employees.filter(e => e.id !== employee.id);
        // Update the MatTableDataSource with the updated array
        this.dataSource.data = this.employees;
        this.messagesService.showSuccessMessage('Success!', 'The employee was successfully deleted!');
        },
        error: (error) => console.log(error)
      })
    }
  }
}
