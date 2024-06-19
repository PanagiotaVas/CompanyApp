import { Routes    } from '@angular/router';
import { authGuard } from "./guards/auth.guard";

export const routes: Routes = [
  { path: '', loadChildren: () => import('./public/public.module').then(m => m.PublicModule) },
  { path: 'admin', loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule), canActivate: [authGuard] },
  { path: 'employee', loadChildren: () => import('./employee/employee.module').then(m => m.EmployeeModule), canActivate: [authGuard] },
  { path: 'login', loadComponent: () => import('./components/login/login.component').then(m => m.LoginComponent)},
  { path: 'register', loadComponent: () => import('./components/register/register.component').then(m => m.RegisterComponent)},
];

