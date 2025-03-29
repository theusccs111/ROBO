import { Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { NavbarComponent } from './components/navbar/navbar.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TopbarComponent } from './components/topbar/topbar.component';
import { TokenStorageService } from './services/token.storage.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, NavbarComponent, TopbarComponent, SidebarComponent, RouterOutlet]

})
export class AppComponent implements OnInit, OnChanges {
  title = '.:: SISTEMA ::.';
  isShowSideBar = true;
  toolbarIsVisible: boolean = false
  robo: any;

  constructor(private router: Router,
    private tokenStorageService: TokenStorageService) { }

  ngOnInit(): void {
    this.robo = this.tokenStorageService.getRobo();
    this.router.events.subscribe((event: any) => {
      if (event instanceof NavigationEnd) {

        let activeRoute = event.url;
        if (event.url === '/login' || event.url === '/') {
          this.isShowSideBar = false;
        } else {
          this.isShowSideBar = true;
        }
      }
    });
  }

  logout() {
    this.tokenStorageService.signOut();
    this.router.navigate(['/login']);
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.robo = this.tokenStorageService.getRobo();
  }
}
