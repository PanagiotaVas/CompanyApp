import {  Component           } from '@angular/core';
import {  CommonModule        } from '@angular/common';
import {  FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {  MatDialogModule     } from "@angular/material/dialog";
import {  MatFormFieldModule  } from "@angular/material/form-field";
import {  MatInputModule      } from "@angular/material/input";
import {  AssignmentService   } from "../../../../services/assignment.service";
import {  MatSelectModule     } from "@angular/material/select";
import {  MessagesService     } from "../../../../services/messages.service";
import {  InsertAssignment    } from 'src/app/models/assignmentinsert';
import {  TaskService         } from 'src/app/services/task.service';
import {  Assignment          } from 'src/app/models/assignment';


@Component({
  selector: 'app-insert-assignment',
  standalone: true,
  imports: [CommonModule, FormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, ReactiveFormsModule, MatSelectModule],
  templateUrl: './insert-assignment.component.html',
  styleUrls: ['./insert-assignment.component.css']
})
export class InsertAssignmentComponent {
  createAssignmentForm: FormGroup

  constructor(
    private fb: FormBuilder, private assignmentService: AssignmentService,
    private taskService: TaskService, private messagesService: MessagesService
  ) {
    this.createAssignmentForm = this.fb.group({
      username: ['', Validators.required],
      title: ['', Validators.required]
    })
  }

  createAssignment() {
    let data: InsertAssignment = {
      username: this.createAssignmentForm.controls['username'].value,
      title: this.createAssignmentForm.controls['title'].value,
    }

    const taskToInsert = this.taskService.getTaskByTitle(data.title).subscribe({
      complete: () => {},
      next: (task) => {

        let assignmentToInsert: Assignment = {
          username: data.username,
          taskId: task.id
        }

        this.assignmentService.assignProject(assignmentToInsert).subscribe({
          complete: () => {},
          next: () => {
            this.messagesService.showSuccessMessage('Success!', 'Project assigned successfully')
            this.createAssignmentForm.reset()
            this.createAssignmentForm.markAsUntouched()
            this.createAssignmentForm.markAsPristine()
          },
          error: (error) => this.messagesService.showErrorMessage('Error: ' + error.status, error.message)
        })
      },
      error: (error) => {}
    });
  }
}
