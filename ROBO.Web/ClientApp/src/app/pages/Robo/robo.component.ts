import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DemoNgZorroAntdModule } from '../../ng-zorro-antd.module';
import { NzModalComponent } from 'ng-zorro-antd/modal';
import { getBracoDireitoCotovelo, getBracoDireitoPulso, getBracoEsquerdoCotovelo, getBracoEsquerdoPulso, getCabecaInclinacao, getCabecaRotacao } from '../../helpers/getEnums';
import { NzMessageService } from 'ng-zorro-antd/message';
import { RoboService } from '../../services/robo.service';
import { TokenStorageService } from '../../services/token.storage.service';


@Component({
  selector: 'app-robo',
  templateUrl: './robo.component.html',
  styleUrl: './robo.component.scss',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    DemoNgZorroAntdModule,
  ],
  providers: [NzModalComponent]
})
export class RoboComponent implements OnInit {
  // Opções para os controles
  bracoDireitoCotoveloOptions = getBracoDireitoCotovelo();
  bracoDireitoPulsoOptions = getBracoDireitoPulso();
  bracoEsquerdoCotoveloOptions = getBracoEsquerdoCotovelo();
  bracoEsquerdoPulsoOptions = getBracoEsquerdoPulso();
  cabecaInclinacaoOptions = getCabecaInclinacao();
  cabecaRotacaoOptions = getCabecaRotacao();

  // Valores selecionados
  bracoDireitoCotoveloValue: number = 0;
  bracoDireitoPulsoValue: number = 2;
  bracoEsquerdoCotoveloValue: number = 0;
  bracoEsquerdoPulsoValue: number = 2;
  cabecaInclinacaoValue: number = 1;
  cabecaRotacaoValue: number = 2;

  // Estado atual do robô
  estadoAtual: any = {};
  isLoading = false;
  showError: boolean = false;
  errorMsg: any;

  constructor(
    private roboService: RoboService,
    private tokenStorageService: TokenStorageService,
    private message: NzMessageService
  ) { }

  ngOnInit(): void {
    let robo = this.tokenStorageService.getRobo()?.robo;
    this.carregarEstadoAtual(robo);
  }

  carregarEstadoAtual(robo: any): void {
    this.estadoAtual = robo;
    this.bracoDireitoCotoveloValue = robo.bracoDireitoCotovelo;
    this.bracoDireitoPulsoValue = robo.bracoDireitoPulso;
    this.bracoEsquerdoCotoveloValue = robo.bracoEsquerdoCotovelo;
    this.bracoEsquerdoPulsoValue = robo.bracoEsquerdoPulso;
    this.cabecaInclinacaoValue = robo.cabecaInclinacao;
    this.cabecaRotacaoValue = robo.cabecaRotacao;
  }

  moverArticulacao(): void {
    this.isLoading = true;
    this.showError = false;
    this.errorMsg = '';

    let comando = {
      bracoDireitoCotovelo: this.bracoDireitoCotoveloValue,
      bracoDireitoPulso: this.bracoDireitoPulsoValue,
      bracoEsquerdoCotovelo: this.bracoEsquerdoCotoveloValue,
      bracoEsquerdoPulso: this.bracoEsquerdoPulsoValue,
      cabecaInclinacao: this.cabecaInclinacaoValue,
      cabecaRotacao: this.cabecaRotacaoValue
    }

    this.roboService.mover(comando).subscribe((res: any) => {
      if (res.success) {
        this.isLoading = false;
        this.tokenStorageService.setRobo(res.data.robo);
        this.tokenStorageService.setToken(res.data.token);
        this.carregarEstadoAtual(res.data.robo);
        this.message.success('Movimento executado com sucesso!');
      } else {
        this.isLoading = false;
        this.showError = true;
        this.errorMsg = res.messageDetail;
      }
    }, (error: any) => {
      this.isLoading = false;
      this.showError = true;
      this.errorMsg = error.error.messageDetail;
    })
  }

}
