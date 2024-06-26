import {  Component     } from '@angular/core';
import {  CommonModule  } from '@angular/common';
import {  MatCardModule } from "@angular/material/card";
import {  RouterLink    } from "@angular/router";

@Component({
  selector: 'app-landing-page',
  standalone: true,
  imports: [CommonModule, MatCardModule, RouterLink],
  templateUrl: './landing-page.component.html',
  styleUrls: ['./landing-page.component.css']
})
export class LandingPageComponent {
  image = "./assets/images/dashboard-background.png"
}
