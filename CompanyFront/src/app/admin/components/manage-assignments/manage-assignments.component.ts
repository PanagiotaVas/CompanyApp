import {  Component, OnInit } from '@angular/core';
import {  CommonModule      } from '@angular/common';
import {  AssignmentService } from "../../../services/assignment.service";
import {  MatCardModule     } from "@angular/material/card";
import {  InsertAssignmentComponent} from "./insert-assignment/insert-assignment.component";
import {  MatDialog         } from "@angular/material/dialog";
import {  AuthService       } from "../../../services/auth.service";
import {  MatTableModule    } from '@angular/material/table';
import {  MatTableDataSource} from '@angular/material/table';
import {  InsertAssignment  } from 'src/app/models/assignmentinsert';
import {  MessagesService   } from 'src/app/services/messages.service';

@Component({
  selector: 'app-manage-assignments',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatTableModule],
  templateUrl: './manage-assignments.component.html',
  styleUrls: ['./manage-assignments.component.css']
})
export class ManageAssignmentsComponent implements OnInit {
  assignments: InsertAssignment[] = [];
  displayColumns: string[] = [];
  dataSource: MatTableDataSource<InsertAssignment> = new MatTableDataSource<InsertAssignment>();
  user = this.authService.session;

  constructor(
    private assignmentService: AssignmentService, public dialog: MatDialog, private authService: AuthService,
    private messagesService: MessagesService
  ) {}

  ngOnInit() {
      this.onLoadPage()
  }

  onLoadPage() {
    this.assignmentService.getAllAssignments().subscribe({
      complete: () => {
        console.log(this.assignments)
        this.dataSource = new MatTableDataSource<InsertAssignment>(this.assignments);
        this.displayColumns = ['username', 'title', 'actions']
      },
      next: (res) => {
        this.assignments = res as InsertAssignment[]
      },
      error: (err) => console.log(err)
    })
  }

  openCreateAssignmentModal() {
    const dialogRef = this.dialog.open(InsertAssignmentComponent, {
      width: '800px',
      autoFocus: false
    })

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        window.location.reload()
      }
    })
  }

  deleteAssignment(assignment: InsertAssignment) {
    if (confirm(`Delete assignment for user ${assignment.username}?`)) {

      this.assignmentService.deleteAssignment(assignment).subscribe({
        complete: () => {
          this.assignments = this.assignments.filter(t => (t.username !== assignment.username) && (t.title != assignment.title));
          this.dataSource.data = this.assignments;
          window.location.reload()
        },
        next: () => {
          this.messagesService.showSuccessMessage('Success!', 'The task was successfully deleted!');
        },
        error: (error) => console.log(error)
      })


    }
  }
}
