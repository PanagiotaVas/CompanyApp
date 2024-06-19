import {  ChangeDetectorRef, Component, NgZone, OnInit} from '@angular/core';
import {  CommonModule          } from '@angular/common';
import {  MatTableDataSource, MatTableModule} from "@angular/material/table";
import {  MatCardModule         } from "@angular/material/card";
import {  InsertProjectComponent} from "./insert-project/insert-project.component";
import {  MatDialog             } from "@angular/material/dialog";
import {  UpdateProjectComponent} from "./update-project/update-project.component";
import {  MessagesService       } from "../../../services/messages.service";
import {  TaskService           } from 'src/app/services/task.service';
import {  Task                  } from 'src/app/models/task';

@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatCardModule],
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.css']
})
export class ProjectsComponent implements OnInit {

  displayColumns: string[] = [];
  tasks: Task[] = [];
  dataSource: MatTableDataSource<Task> = new MatTableDataSource<Task>();

  constructor(
    private taskService: TaskService, public dialog: MatDialog,
    private messagesService: MessagesService,  private zone: NgZone, private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.onLoadPage()
  }

  onLoadPage() {
    this.taskService.getAllTasks().subscribe({
      complete: () => {
        console.log(this.tasks)
        this.dataSource = new MatTableDataSource<Task>(this.tasks);
        this.displayColumns = ['title', 'description', 'deadline', 'actions']
      },
      next: (res) => {
        this.tasks = res as Task[]
        this.dataSource.data = this.tasks;
      },
      error: (err) => console.log(err)
    })
  }

  openCreateProjectModal() {
    const dialogRef = this.dialog.open(InsertProjectComponent, {
      width: '800px',
      autoFocus: false
    })

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        window.location.reload()
      }
    })
  }

  openEditModal(task: Task) {
    const dialogRef = this.dialog.open(UpdateProjectComponent, {
      data: task,
      width: '800px',
      autoFocus: false
    })

    const sub = dialogRef.componentInstance.isUpdated.subscribe((data) => {
      this.messagesService.showSuccessMessage('Success!', 'The task was successfully updated!')
      dialogRef.close()
      window.location.reload()
    })
  }


  deleteProject(task: Task) {
    if (confirm(`Delete task ${task.title}?`)) {
      this.taskService.deleteProject(task.id).subscribe({
        next: () => {
          this.tasks = this.tasks.filter(t => t.id !== task.id);
          this.dataSource.data = this.tasks;
          this.messagesService.showSuccessMessage('Success!', 'The task was successfully deleted!');
          this.cdr.detectChanges(); // Trigger change detection manually
        },
        error: (error) => {
          console.error('Error deleting task', error);
          this.messagesService.showErrorMessage('Error!', 'Failed to delete the task.');
        }
      });
    }
  }
}
