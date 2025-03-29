import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { AuthGuard } from './guards/auth.guard';
import { RoboComponent } from './pages/Robo/robo.component';

export const routes: Routes = [
    
    {
        path: 'login',
        component: LoginComponent,
    },
    {
        path: 'robo',
        component: RoboComponent,

        data: {
            title: 'R.O.B.O.',
            info: 'Interface gráfica do R.O.B.O.',
        },
        canActivate: [AuthGuard]
    },
    { path: '**', component: LoginComponent },
];
