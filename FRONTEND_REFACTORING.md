# Frontend Refactoring for Global User Model

## Overview
Updated the Angular frontend to align with the backend refactoring that implements global users (no tenant-specific authentication).

## Date
October 15, 2025

## Changes Made

### 1. Auth Models (`auth.model.ts`)

#### LoginRequest Interface
**Before:**
```typescript
export interface LoginRequest {
  email: string;
  password: string;
  tenantId?: string;
}
```

**After:**
```typescript
export interface LoginRequest {
  email: string;
  password: string;
}
```

#### RegisterRequest Interface
**Before:**
```typescript
export interface RegisterRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  phone: string;
  userType: UserType;
  tenantId: string;
}
```

**After:**
```typescript
export interface RegisterRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  phone: string;
  userType: UserType;
}
```

#### AuthResponse Interface
**Before:**
```typescript
export interface AuthResponse {
  token: string;
  refreshToken: string;
  expiration: Date;
  userId: string;
  email: string;
  userType: string;
  tenantId: string;
}
```

**After:**
```typescript
export interface AuthResponse {
  token: string;
  refreshToken: string;
  expiration: Date;
  userId: string;
  email: string;
  userType: string;
  firstName: string;
  lastName: string;
}
```

#### User Interface
**Before:**
```typescript
export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  phone: string;
  userType: UserType;
  tenantId: string;
}
```

**After:**
```typescript
export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  phone: string;
  userType: UserType;
}
```

### 2. Register Component (`register.component.ts`)

#### Form Controls
**Removed:**
- `tenantId` form control and validator

**Before:**
```typescript
this.registerForm = this.fb.group({
  email: ['', [Validators.required, Validators.email]],
  password: ['', [Validators.required, Validators.minLength(6)]],
  confirmPassword: ['', [Validators.required]],
  firstName: ['', [Validators.required]],
  lastName: ['', [Validators.required]],
  phone: ['', [Validators.required]],
  userType: [UserType.Patient, [Validators.required]],
  tenantId: ['', [Validators.required]]
}, { validators: this.passwordMatchValidator });
```

**After:**
```typescript
this.registerForm = this.fb.group({
  email: ['', [Validators.required, Validators.email]],
  password: ['', [Validators.required, Validators.minLength(6)]],
  confirmPassword: ['', [Validators.required]],
  firstName: ['', [Validators.required]],
  lastName: ['', [Validators.required]],
  phone: ['', [Validators.required]],
  userType: [UserType.Patient, [Validators.required]]
}, { validators: this.passwordMatchValidator });
```

### 3. Register Template (`register.component.html`)

**Removed:**
- Entire tenant ID input field section (lines 92-109)

### 4. Auth Service (`auth.service.ts`)

#### handleAuthResponse Method
**Before:**
```typescript
private handleAuthResponse(response: AuthResponse): void {
  localStorage.setItem('currentUser', JSON.stringify(response));
  localStorage.setItem('token', response.token);
  localStorage.setItem('refreshToken', response.refreshToken);
  localStorage.setItem('tenantId', response.tenantId);
  this.currentUserSubject.next(response);
}
```

**After:**
```typescript
private handleAuthResponse(response: AuthResponse): void {
  localStorage.setItem('currentUser', JSON.stringify(response));
  localStorage.setItem('token', response.token);
  localStorage.setItem('refreshToken', response.refreshToken);
  this.currentUserSubject.next(response);
}
```

#### clearAuthData Method
**Before:**
```typescript
private clearAuthData(): void {
  localStorage.removeItem('currentUser');
  localStorage.removeItem('token');
  localStorage.removeItem('refreshToken');
  localStorage.removeItem('tenantId');
  this.currentUserSubject.next(null);
}
```

**After:**
```typescript
private clearAuthData(): void {
  localStorage.removeItem('currentUser');
  localStorage.removeItem('token');
  localStorage.removeItem('refreshToken');
  this.currentUserSubject.next(null);
}
```

#### Removed Method
**Removed:**
```typescript
getTenantId(): string | null {
  return localStorage.getItem('tenantId');
}
```

### 5. Tenant Interceptor (`tenant.interceptor.ts`)

**Before:**
```typescript
import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const tenantInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const tenantId = authService.getTenantId();

  if (tenantId) {
    req = req.clone({
      setHeaders: {
        'X-Tenant-Id': tenantId
      }
    });
  }

  return next(req);
};
```

**After:**
```typescript
import { HttpInterceptorFn } from '@angular/common/http';

// Note: Tenant context is now determined server-side based on the resource being accessed
// (e.g., appointment, time slot) rather than from the user's JWT token.
// This interceptor is kept for future use if needed for specific tenant-scoped operations.
export const tenantInterceptor: HttpInterceptorFn = (req, next) => {
  // No longer adding X-Tenant-Id header since users are global
  // Tenant context is managed server-side through resource associations
  return next(req);
};
```

## Impact on User Experience

### Registration Flow
**Before:**
1. User enters email, password, name, phone
2. User selects user type
3. **User must enter Hospital/Tenant ID** ❌
4. User creates account

**After:**
1. User enters email, password, name, phone
2. User selects user type
3. User creates account ✅

### Login Flow
**Before:**
1. User enters email and password
2. Optionally enters tenant ID
3. User logs in

**After:**
1. User enters email and password
2. User logs in ✅

### Key Benefits
- **Simplified Registration**: No need to know tenant ID upfront
- **Global Account**: One account works across all hospitals
- **Better UX**: Fewer fields, less confusion
- **Portable Identity**: Users can access multiple hospitals with same credentials

## Tenant Context Management

### How It Works Now

1. **Authentication**: Users log in globally without specifying a hospital
2. **Resource Access**: Tenant context is determined by the resource being accessed:
   - When booking an appointment → Hospital is selected
   - When viewing time slots → Hospital context from the time slot
   - When accessing medical records → Hospital context from the appointment

3. **Server-Side Logic**: The backend determines tenant context based on:
   - The resource being accessed (e.g., appointment has TenantId)
   - Doctor-hospital associations
   - Staff/Admin roles (which are tenant-specific)

### Example Scenarios

#### Scenario 1: Patient Booking Appointment
```typescript
// Patient selects a hospital and doctor
const appointmentRequest = {
  patientId: currentUser.userId,
  doctorId: selectedDoctor.id,
  hospitalId: selectedHospital.id, // Tenant context
  timeSlotId: selectedSlot.id,
  appointmentType: 'Consultation'
};

// Backend validates doctor is associated with the hospital
// and creates appointment with the hospital's TenantId
```

#### Scenario 2: Doctor Viewing Schedule
```typescript
// Doctor selects which hospital to view schedule for
const timeSlots = await getTimeSlots({
  doctorId: currentUser.doctorId,
  hospitalId: selectedHospital.id // Tenant context
});

// Backend filters time slots for that specific hospital
```

#### Scenario 3: Staff Accessing Dashboard
```typescript
// Staff member's role is tied to a specific hospital
const staffRole = await getStaffRole(currentUser.userId);
// staffRole.tenantId determines which hospital's data they see

// All subsequent requests use this tenant context
```

## Migration Notes

### For Existing Users
If you have users in localStorage with the old format:

1. **Clear localStorage** on first load after update
2. **Force re-login** to get new auth response format
3. **Update any cached user data**

### Recommended Approach
Add a version check in the auth service:

```typescript
private getUserFromStorage(): AuthResponse | null {
  const userJson = localStorage.getItem('currentUser');
  if (!userJson) return null;
  
  const user = JSON.parse(userJson);
  
  // Check if old format (has tenantId)
  if ('tenantId' in user) {
    // Clear old data and force re-login
    this.clearAuthData();
    return null;
  }
  
  return user;
}
```

## Testing Checklist

### Registration
- [ ] Can register without tenant ID
- [ ] Email validation works
- [ ] Password validation works
- [ ] User type selection works
- [ ] Successful registration redirects to dashboard
- [ ] Error messages display correctly

### Login
- [ ] Can login with email and password only
- [ ] Successful login redirects to dashboard
- [ ] Auth token is stored correctly
- [ ] User info is stored correctly (without tenantId)
- [ ] Error messages display correctly

### Auth Service
- [ ] Token is included in API requests
- [ ] Refresh token works
- [ ] Logout clears all auth data
- [ ] No tenantId in localStorage after login/register

### Interceptors
- [ ] Auth interceptor adds Bearer token
- [ ] Tenant interceptor no longer adds X-Tenant-Id header
- [ ] API requests work without tenant header

## Future Enhancements

### 1. Hospital Selection UI
Add a hospital selector for users who work at/visit multiple hospitals:

```typescript
interface HospitalContext {
  hospitalId: string;
  hospitalName: string;
  userRole?: 'patient' | 'doctor' | 'staff' | 'admin';
}

// Store current hospital context
localStorage.setItem('currentHospital', JSON.stringify(hospitalContext));
```

### 2. Multi-Hospital Dashboard
For doctors/staff working at multiple hospitals:

```typescript
// Show all hospitals user is associated with
const userHospitals = await getUserHospitals(userId);

// Allow switching between hospitals
function switchHospital(hospitalId: string) {
  // Update context and reload relevant data
}
```

### 3. Patient Hospital History
Show patients which hospitals they've visited:

```typescript
const visitedHospitals = await getPatientHospitals(patientId);
// Display list of hospitals with appointment counts
```

## Files Modified

1. `src/app/core/models/auth.model.ts`
2. `src/app/features/auth/register/register.component.ts`
3. `src/app/features/auth/register/register.component.html`
4. `src/app/core/services/auth.service.ts`
5. `src/app/core/interceptors/tenant.interceptor.ts`

## Conclusion

The frontend has been successfully updated to support the global user model. Users can now register and login without needing to know or specify a tenant ID. The tenant context is now managed server-side based on the resources being accessed, providing a more seamless and user-friendly experience.
