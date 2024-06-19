import {  Component         } from '@angular/core';
import {  CommonModule      } from '@angular/common';
import {  FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {  MatDialogModule   } from "@angular/material/dialog";
import {  MatFormFieldModule} from "@angular/material/form-field";
import {  MatInputModule    } from "@angular/material/input";
import {  MatDatepickerModule} from "@angular/material/datepicker";
import {  AuthService       } from "../../../../services/auth.service";
import {  MessagesService   } from "../../../../services/messages.service";
import {  TaskService       } from 'src/app/services/task.service';
import {  TaskInsert        } from 'src/app/models/taskInsert';


@Component({
  selector: 'app-insert-project',
  standalone: true,
  imports: [CommonModule, FormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, ReactiveFormsModule, MatDatepickerModule],
  templateUrl: './insert-project.component.html',
  styleUrls: ['./insert-project.component.css']
})
export class InsertProjectComponent {
  insertProjectForm: FormGroup

  constructor(
    private fb: FormBuilder, private taskService: TaskService, private authService: AuthService,
    private messagesService: MessagesService
  ) {
    this.insertProjectForm = this.fb.group({
      title: ['', [Validators.required]],
      description: ['', [Validators.required]],
      deadline: ['', [Validators.required]],
    })
  }

  convertDateToBackendFormat(date: Date): string {
    date.setHours(0, 0, 0, 0);
    // Convert date to ISO string and add the 'Z' to denote UTC time
    const isoString = date.toISOString();
    return isoString;
  }

  saveProject() {
    const user = this.authService.session;
    const deadlineDate: Date = this.insertProjectForm.controls['deadline'].value;
    const convertedDate = this.convertDateToBackendFormat(deadlineDate);

    const task: TaskInsert = {
      userId: user.id,  // of the admin
      title: this.insertProjectForm.controls['title'].value,
      description: this.insertProjectForm.controls['description'].value,
      deadline: convertedDate
    }

    this.taskService.insertTask(task).subscribe({
      complete: () => {},
      next: () => {
        this.messagesService.showSuccessMessage("Success!", 'Project inserted successfully!')
        this.insertProjectForm.reset()
        this.insertProjectForm.markAsUntouched()
        this.insertProjectForm.markAsPristine()
      },
      error: (error) => console.log(error)
    })
  }
}
