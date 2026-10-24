import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/dashboard',
    pathMatch: 'full'
  },
  {
    path: 'auth',
    loadChildren: () => import('./features/auth/auth.routes').then(m => m.AUTH_ROUTES)
  },
  {
    path: 'dashboard',
    canActivate: [authGuard],
    loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent)
  },
  {
    path: 'appointments',
    canActivate: [authGuard],
    loadChildren: () => import('./features/appointments/appointments.routes').then(m => m.APPOINTMENT_ROUTES)
  },
  {
    path: 'doctors',
    canActivate: [authGuard],
    loadChildren: () => import('./features/doctors/doctors.routes').then(m => m.DOCTOR_ROUTES)
  },
  {
    path: 'patients',
    canActivate: [authGuard],
    loadChildren: () => import('./features/patients/patients.routes').then(m => m.PATIENT_ROUTES)
  },
  {
    path: '**',
    redirectTo: '/dashboard'
  }
];
