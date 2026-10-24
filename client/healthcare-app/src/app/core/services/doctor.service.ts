import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Doctor, TimeSlot } from '../models/doctor.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DoctorService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/doctors`;

  getAllDoctors(): Observable<Doctor[]> {
    return this.http.get<Doctor[]>(this.apiUrl);
  }

  getDoctorById(id: string): Observable<Doctor> {
    return this.http.get<Doctor>(`${this.apiUrl}/${id}`);
  }

  getDoctorTimeSlots(doctorId: string, date?: string): Observable<TimeSlot[]> {
    const params = date ? { date } : {};
    return this.http.get<TimeSlot[]>(`${this.apiUrl}/${doctorId}/timeslots`, { params });
  }

  createDoctor(doctor: Partial<Doctor>): Observable<Doctor> {
    return this.http.post<Doctor>(this.apiUrl, doctor);
  }

  updateDoctor(id: string, doctor: Partial<Doctor>): Observable<Doctor> {
    return this.http.put<Doctor>(`${this.apiUrl}/${id}`, doctor);
  }
}
