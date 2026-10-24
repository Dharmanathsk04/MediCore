import { Routes } from '@angular/router';

export const DOCTOR_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./doctor-list/doctor-list.component').then(m => m.DoctorListComponent)
  }
];
