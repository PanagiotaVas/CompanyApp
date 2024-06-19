import {  Component     } from '@angular/core';
import {  CommonModule  } from '@angular/common';
import {  RouterOutlet  } from "@angular/router";

@Component({
  selector: 'app-public',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  templateUrl: './public.component.html',
  styleUrls: ['./public.component.css']
})
export class PublicComponent {

}
