import { NgModule             } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', loadComponent: () => import('./public/public.component').then(m => m.PublicComponent),
    children: [
      { path: '', loadComponent: () => import('./landing/landing-page/landing-page.component').then(m => m.LandingPageComponent) }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PublicRoutingModule { }
