import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormGroup, FormsModule, NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { DemoNgZorroAntdModule } from '../../ng-zorro-antd.module';
import { NzModalComponent } from 'ng-zorro-antd/modal';
import { TokenStorageService } from '../../services/token.storage.service';
import { RoboService } from '../../services/robo.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    DemoNgZorroAntdModule,
  ],
  providers: [NzModalComponent]
})
export class LoginComponent implements OnInit {
  protected loading = 0;
  protected showError: boolean = false;
  protected errorMsg: any;
  protected showPassword: boolean = false;
  protected form!: FormGroup;

  constructor(
    private roboService: RoboService,
    private tokenStorageService: TokenStorageService,
  ) { }

  ngOnInit(): void {
    this.showError = false;
      this.loading++;
      this.roboService.iniciar().subscribe(async (res) => {
        if (res.success) {
          this.loading--;
          await this.tokenStorageService.setRobo(res.data);
          await this.tokenStorageService.setToken(res.data.token);
          window.location.href = '/robo';
        } else {
          this.loading--;
          this.showError = true;
          this.errorMsg = "Falha ao iniciar o R.O.B.O.!!!";
        }
      }, (error: any) => {
        this.loading--;
        this.showError = true;
        this.errorMsg = "Falha ao iniciar o R.O.B.O.!!!";
      })
  }

}
