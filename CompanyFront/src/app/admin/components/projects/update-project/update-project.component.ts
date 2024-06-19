import {  Component, EventEmitter, Inject, OnInit, Output} from '@angular/core';
import {  CommonModule        } from '@angular/common';
import {  FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {  MatDatepickerModule } from "@angular/material/datepicker";
import {  MAT_DIALOG_DATA, MatDialogModule} from "@angular/material/dialog";
import {  MatFormFieldModule  } from "@angular/material/form-field";
import {  MatInputModule      } from "@angular/material/input";
import {  AuthService         } from "../../../../services/auth.service";
import {  MessagesService     } from "../../../../services/messages.service";
import {  TaskService         } from 'src/app/services/task.service';
import {  Task                } from 'src/app/models/task';
import {  TaskUpdate          } from 'src/app/models/taskupdate';

@Component({
  selector: 'app-update-project',
  standalone: true,
    imports: [CommonModule, FormsModule, MatDatepickerModule, MatDialogModule, MatFormFieldModule, MatInputModule, ReactiveFormsModule],
  templateUrl: './update-project.component.html',
  styleUrls: ['./update-project.component.css']
})
export class UpdateProjectComponent implements OnInit {
  updateProjectForm: FormGroup
  @Output() isUpdated: EventEmitter<boolean> = new EventEmitter();

  constructor(
    private fb: FormBuilder, private taskService: TaskService,
    private authService: AuthService, @Inject(MAT_DIALOG_DATA) public data: Task,
    private messagesService: MessagesService
  ) {
    this.updateProjectForm = this.fb.group({
      title: ['', [Validators.required]],
      description: ['', [Validators.required]],
      deadline: ['', [Validators.required]]
    })
  }

  ngOnInit() {
    this.onLoadPage()
  }

  onLoadPage() {
    this.updateProjectForm.controls['title'].setValue(this.data.title)
    this.updateProjectForm.controls['description'].setValue(this.data.description)
    this.updateProjectForm.controls['deadline'].setValue((this.data.deadline))
    console.log(`data is on ${this.updateProjectForm.controls['deadline'].setValue((this.data.deadline))}`)
  }

  saveProject() {
    const updatedProject: TaskUpdate = {
      id: this.data.id, 
      userId: this.authService.session.id,
      title: this.updateProjectForm.controls['title'].value,
      description: this.updateProjectForm.controls['description'].value,
      deadline: this.updateProjectForm.controls['deadline'].value
    }
    this.taskService.updateProject(updatedProject).subscribe({
      complete: () => this.isUpdated.emit(true),
      next: () => {},
      error: (error) => this.messagesService.showErrorMessage('Error: ' + error.status, error.message)
    })
  }
}
