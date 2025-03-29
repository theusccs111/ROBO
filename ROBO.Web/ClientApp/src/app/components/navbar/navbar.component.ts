import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { TokenStorageService } from '../../services/token.storage.service';
import { DemoNgZorroAntdModule } from '../../ng-zorro-antd.module';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { RoboService } from '../../services/robo.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    DemoNgZorroAntdModule,
    NzIconModule,
    NzModalModule
  ]
})
export class NavbarComponent implements OnInit {
  username: string = "Nome do usuário";
  profile: string = "";
  title: string = "";
  user: any;
  hasNotifications = signal<boolean>(false);
  loading = 0;
  constructor(
    private router: Router,
    private tokenStorageService: TokenStorageService,
    private roboService: RoboService,
  ) {
  }

  ngOnInit(): void {
    this.getUser();
    
  }

  getUser() {
    this.loading++;
    this.roboService.obter().subscribe(async (res) => {
      if (res.success) {
        this.loading--;
        await this.tokenStorageService.setRobo(res.data);
        this.user = this.tokenStorageService.getRobo();
        this.username = this.user?.nome;
      } else {
        this.loading--;
      }
    }, (error: any) => {
      this.loading--;
    })
  }

  shouldDisplayNavbar(): boolean {
    return !this.router.url.includes('login');
  }

  logout() {
    this.tokenStorageService.signOut();
    this.router.navigate(['/login']);
  }
}
