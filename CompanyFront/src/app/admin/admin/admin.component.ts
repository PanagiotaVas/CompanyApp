import {  Component             } from '@angular/core';
import {  CommonModule          } from '@angular/common';
import {  MatSidenavModule      } from "@angular/material/sidenav";
import {  map, Observable, shareReplay} from "rxjs";
import {  BreakpointObserver, Breakpoints} from "@angular/cdk/layout";
import {  MatToolbarModule      } from "@angular/material/toolbar";
import {  RouterLink, RouterOutlet} from "@angular/router";
import {  MatButtonModule       } from "@angular/material/button";
import {  MatIconModule         } from "@angular/material/icon";
import {  MatListModule         } from "@angular/material/list";
import {  MatExpansionModule    } from "@angular/material/expansion";
import {  AuthService           } from "../../services/auth.service";
import {  FooterComponent       } from "../footer/footer.component";

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [CommonModule, MatSidenavModule, MatToolbarModule, RouterLink, MatButtonModule, MatIconModule, MatListModule, MatExpansionModule, RouterOutlet, FooterComponent],
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})

export class AdminComponent {

  isHandset$: Observable<boolean> = this.breakpointObserver
    .observe(Breakpoints.Handset)
    .pipe(
      map((result) => result.matches),
      shareReplay()
    );

  constructor(private breakpointObserver: BreakpointObserver, private authService: AuthService) {}

  logout() {
    this.authService.logout()
  }
}
