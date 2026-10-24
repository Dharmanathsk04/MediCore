import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-appointment-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <div class="min-h-screen bg-gray-50 p-8">
      <div class="max-w-7xl mx-auto">
        <div class="flex justify-between items-center mb-6">
          <h2 class="text-3xl font-bold text-gray-900">Appointments</h2>
          <a routerLink="/appointments/new" class="btn-primary">Book New Appointment</a>
        </div>
        <div class="card">
          <p class="text-gray-600">Appointment list will be displayed here.</p>
        </div>
      </div>
    </div>
  `
})
export class AppointmentListComponent {}
