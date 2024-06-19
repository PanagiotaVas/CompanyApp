import { NgModule             } from '@angular/core';
import { CommonModule         } from '@angular/common';
import { LandingPageComponent } from "./landing-page/landing-page.component";
import { RouterModule         } from "@angular/router";



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule,
    LandingPageComponent
  ],
  exports: [
    LandingPageComponent
  ]
})
export class LandingModule { }
