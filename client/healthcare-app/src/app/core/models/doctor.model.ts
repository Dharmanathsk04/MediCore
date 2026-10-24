export interface Doctor {
  id: string;
  userId: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  specialization: string;
  licenseNumber: string;
  qualification: string;
  experienceYears: number;
  consultationFee: number;
  isAvailableForHomeVisit: boolean;
  homeVisitFee?: number;
  bio?: string;
}

export interface TimeSlot {
  id: string;
  doctorId: string;
  doctorName: string;
  date: Date;
  startTime: string;
  endTime: string;
  isBooked: boolean;
  isAvailable: boolean;
  slotType: number;
}
