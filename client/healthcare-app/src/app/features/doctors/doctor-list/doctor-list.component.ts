import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-doctor-list',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="min-h-screen bg-gray-50 p-8">
      <div class="max-w-7xl mx-auto">
        <h2 class="text-3xl font-bold text-gray-900 mb-6">Doctors</h2>
        <div class="card">
          <p class="text-gray-600">Doctor list will be displayed here.</p>
        </div>
      </div>
    </div>
  `
})
export class DoctorListComponent {}
