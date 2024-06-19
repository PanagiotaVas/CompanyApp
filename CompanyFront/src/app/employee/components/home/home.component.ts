import { Component, OnInit } from '@angular/core';
import { CommonModule      } from '@angular/common';
import { FooterComponent   } from "../../footer/footer.component";
import { AuthService       } from 'src/app/services/auth.service';
import { MatSidenavModule  } from '@angular/material/sidenav';
import { MatToolbarModule  } from '@angular/material/toolbar';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { map, Observable, shareReplay    } from "rxjs";
import { MatIconModule                   } from '@angular/material/icon';
import { RouterLink, RouterOutlet        } from '@angular/router';
import { MatButtonModule                 } from '@angular/material/button';
import { MatListModule      } from '@angular/material/list';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatCardModule      } from '@angular/material/card';
import { Task               } from 'src/app/models/task';
import { TaskService        } from 'src/app/services/task.service';
import { EmployeeService    } from 'src/app/services/employee.service';

@Component({
    selector: 'app-home',
    standalone: true,
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css'],
    imports: [CommonModule, MatSidenavModule, MatToolbarModule,
      MatTableModule, MatCardModule,
       RouterLink, MatButtonModule, MatIconModule, MatListModule, MatExpansionModule, RouterOutlet, FooterComponent
    ]
})
export class HomeComponent implements OnInit {

  tasks: Task[] = [];
  displayColumns: string[] = [];
  dataSource: MatTableDataSource<Task> = new MatTableDataSource<Task>()

  isHandset$: Observable<boolean> = this.breakpointObserver
  .observe(Breakpoints.Handset)
  .pipe(
    map((result) => result.matches),
    shareReplay()
  );

  constructor( private authService: AuthService, private breakpointObserver: BreakpointObserver,
              private taskService: TaskService, private employeeService: EmployeeService) {}

  ngOnInit() {
    this.onLoadPage()
  }

  onLoadPage() {
    const user = this.authService.session

    this.taskService.getAllTasksOfUser(user.id).subscribe({
      complete: () => {
        this.dataSource = new MatTableDataSource<Task>(this.tasks);
        this.displayColumns = ['title', 'description', 'deadline']
      },
      next: (res) => {
        this.tasks = res as Task[]
      },
      error: (err) => console.log(err)
    })
  }

  logout() {
    this.authService.logout()
  }
}
