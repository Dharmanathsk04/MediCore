import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  private authService = inject(AuthService);
  
  currentUser: any;
  stats = {
    totalAppointments: 0,
    todayAppointments: 0,
    pendingRequests: 0,
    completedAppointments: 0
  };

  ngOnInit(): void {
    this.currentUser = this.authService.currentUserValue;
    this.loadDashboardData();
  }

  loadDashboardData(): void {
    // Load dashboard statistics
    // This would typically call a service
    this.stats = {
      totalAppointments: 45,
      todayAppointments: 8,
      pendingRequests: 12,
      completedAppointments: 33
    };
  }

  logout(): void {
    this.authService.logout();
  }
}
