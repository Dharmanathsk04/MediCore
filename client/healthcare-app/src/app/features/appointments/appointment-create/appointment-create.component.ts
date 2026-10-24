import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-appointment-create',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <div class="min-h-screen bg-gray-50 p-8">
      <div class="max-w-3xl mx-auto">
        <h2 class="text-3xl font-bold text-gray-900 mb-6">Book New Appointment</h2>
        <div class="card">
          <p class="text-gray-600">Appointment booking form will be displayed here.</p>
        </div>
      </div>
    </div>
  `
})
export class AppointmentCreateComponent {}
