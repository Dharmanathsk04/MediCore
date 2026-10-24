export interface Appointment {
  id: string;
  patientId: string;
  patientName: string;
  doctorId: string;
  doctorName: string;
  timeSlotId: string;
  appointmentDate: Date;
  startTime: string;
  endTime: string;
  appointmentType: AppointmentType;
  status: AppointmentStatus;
  symptoms?: string;
  diagnosis?: string;
  visitAddress?: string;
  totalFee?: number;
}

export interface CreateAppointmentRequest {
  patientId: string;
  doctorId: string;
  timeSlotId: string;
  appointmentType: AppointmentType;
  symptoms?: string;
  visitAddress?: string;
  notes?: string;
}

export enum AppointmentType {
  InClinic = 1,
  HomeVisit = 2,
  Telemedicine = 3
}

export enum AppointmentStatus {
  Scheduled = 1,
  Confirmed = 2,
  InProgress = 3,
  Completed = 4,
  Cancelled = 5,
  NoShow = 6
}
