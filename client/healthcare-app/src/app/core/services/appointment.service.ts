import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Appointment, CreateAppointmentRequest } from '../models/appointment.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/appointments`;

  getAppointmentById(id: string): Observable<Appointment> {
    return this.http.get<Appointment>(`${this.apiUrl}/${id}`);
  }

  createAppointment(appointment: CreateAppointmentRequest): Observable<Appointment> {
    return this.http.post<Appointment>(this.apiUrl, appointment);
  }

  updateAppointment(id: string, appointment: Partial<Appointment>): Observable<Appointment> {
    return this.http.put<Appointment>(`${this.apiUrl}/${id}`, appointment);
  }

  cancelAppointment(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getPatientAppointments(patientId: string): Observable<Appointment[]> {
    return this.http.get<Appointment[]>(`${this.apiUrl}/patient/${patientId}`);
  }

  getDoctorAppointments(doctorId: string): Observable<Appointment[]> {
    return this.http.get<Appointment[]>(`${this.apiUrl}/doctor/${doctorId}`);
  }
}
