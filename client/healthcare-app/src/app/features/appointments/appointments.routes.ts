import { Routes } from '@angular/router';

export const APPOINTMENT_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./appointment-list/appointment-list.component').then(m => m.AppointmentListComponent)
  },
  {
    path: 'new',
    loadComponent: () => import('./appointment-create/appointment-create.component').then(m => m.AppointmentCreateComponent)
  }
];
