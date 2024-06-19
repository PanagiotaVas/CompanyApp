import {  NgModule            } from '@angular/core';
import {  CommonModule        } from '@angular/common';
import {  AdminRoutingModule  } from './admin-routing.module';
import {  MatDialogModule     } from "@angular/material/dialog";
import {  MatNativeDateModule } from "@angular/material/core";


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    AdminRoutingModule,
    MatDialogModule,
    MatNativeDateModule,
  ]
})

export class AdminModule { }
