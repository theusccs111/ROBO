import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { CurrencyPipe, registerLocaleData } from '@angular/common';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { provideClientHydration } from '@angular/platform-browser';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { environment } from 'ng-zorro-antd/core/environments';
import { provideNzI18n, pt_BR } from 'ng-zorro-antd/i18n';
import { NzModalService } from 'ng-zorro-antd/modal';
import { DemoNgZorroAntdModule } from './ng-zorro-antd.module';
import { provideNzIcons } from './icons-provider';
import pt from '@angular/common/locales/pt';
import { AuthGuard } from './guards/auth.guard';
import { authInterceptor } from './interceptors/auth.interceptor';

registerLocaleData(pt);

export const appConfig: ApplicationConfig = {
  providers: [
    NzModalService,
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideClientHydration(),
    provideNzIcons(),
    provideNzI18n(pt_BR),
    importProvidersFrom(
      FormsModule,
      ReactiveFormsModule,
      DemoNgZorroAntdModule
    ),
    provideAnimationsAsync(),
    provideHttpClient(withInterceptors([authInterceptor])),
    CurrencyPipe,
    AuthGuard,
  ]
};
