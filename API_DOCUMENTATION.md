# Healthcare SaaS API Documentation

## Base URL
```
http://localhost:5000/api
```

## Authentication
All endpoints (except Auth endpoints) require a Bearer token in the Authorization header:
```
Authorization: Bearer <your_jwt_token>
```

---

## Auth Endpoints

### 1. Register User
**POST** `/auth/register`

Creates a new global user account.

**Request Body:**
```json
{
  "email": "john.doe@example.com",
  "password": "SecurePassword123",
  "firstName": "John",
  "lastName": "Doe",
  "phone": "+1234567890",
  "userType": 1
}
```

**User Types:**
- `1` - Patient
- `2` - Doctor
- `3` - Staff
- `4` - Admin
- `5` - SuperAdmin

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "base64_encoded_refresh_token",
  "expiration": "2025-10-16T01:00:00Z",
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "email": "john.doe@example.com",
  "userType": "Patient",
  "firstName": "John",
  "lastName": "Doe"
}
```

**Error Responses:**
- `409 Conflict` - User with email already exists
- `400 Bad Request` - Invalid data

---

### 2. Login
**POST** `/auth/login`

Authenticates a user and returns JWT token.

**Request Body:**
```json
{
  "email": "john.doe@example.com",
  "password": "SecurePassword123"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "base64_encoded_refresh_token",
  "expiration": "2025-10-16T01:00:00Z",
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "email": "john.doe@example.com",
  "userType": "Patient",
  "firstName": "John",
  "lastName": "Doe"
}
```

**Error Responses:**
- `401 Unauthorized` - Invalid credentials or inactive account

---

### 3. Refresh Token
**POST** `/auth/refresh-token`

Refreshes an expired JWT token.

**Request Body:**
```json
"base64_encoded_refresh_token"
```

**Response (200 OK):**
```json
{
  "token": "new_jwt_token",
  "refreshToken": "new_refresh_token",
  "expiration": "2025-10-16T01:00:00Z",
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "email": "john.doe@example.com",
  "userType": "Patient",
  "firstName": "John",
  "lastName": "Doe"
}
```

---

### 4. Logout
**POST** `/auth/logout`

Revokes the refresh token.

**Request Body:**
```json
"base64_encoded_refresh_token"
```

**Response (200 OK):**
```json
{
  "success": true
}
```

---

## Users Endpoints

### 1. Get All Users (Paginated)
**GET** `/users`

Retrieves a paginated list of registered users with optional filtering.

**Query Parameters:**
- `page` (optional, default: 1) - Page number
- `pageSize` (optional, default: 10) - Items per page
- `userType` (optional) - Filter by user type (Patient, Doctor, Staff, Admin, SuperAdmin)
- `searchTerm` (optional) - Search by email, first name, or last name

**Examples:**
```
GET /users?page=1&pageSize=20
GET /users?userType=Doctor
GET /users?searchTerm=john
GET /users?userType=Patient&searchTerm=smith&page=2&pageSize=15
```

**Response (200 OK):**
```json
{
  "data": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "email": "john.doe@example.com",
      "firstName": "John",
      "lastName": "Doe",
      "phone": "+1234567890",
      "userType": "Patient",
      "isActive": true,
      "lastLoginDate": "2025-10-15T01:00:00Z",
      "createdDate": "2025-10-14T10:00:00Z"
    }
  ],
  "pagination": {
    "currentPage": 1,
    "pageSize": 10,
    "totalCount": 45,
    "totalPages": 5
  }
}
```

---

### 2. Get User by ID
**GET** `/users/{id}`

Retrieves detailed information about a specific user.

**Path Parameters:**
- `id` - User GUID

**Example:**
```
GET /users/3fa85f64-5717-4562-b3fc-2c963f66afa6
```

**Response (200 OK):**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "email": "john.doe@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "phone": "+1234567890",
  "userType": "Patient",
  "isActive": true,
  "lastLoginDate": "2025-10-15T01:00:00Z",
  "createdDate": "2025-10-14T10:00:00Z",
  "modifiedDate": "2025-10-15T00:30:00Z"
}
```

**Error Responses:**
- `404 Not Found` - User not found or deleted

---

### 3. Get User Statistics
**GET** `/users/stats`

Retrieves statistics about registered users.

**Response (200 OK):**
```json
{
  "totalUsers": 150,
  "activeUsers": 142,
  "inactiveUsers": 8,
  "usersByType": {
    "patients": 100,
    "doctors": 25,
    "staff": 15,
    "admins": 2,
    "superAdmins": 0
  },
  "recentRegistrations": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "email": "jane.smith@example.com",
      "firstName": "Jane",
      "lastName": "Smith",
      "userType": "Doctor",
      "createdDate": "2025-10-15T01:00:00Z"
    }
  ]
}
```

---

### 4. Toggle User Status
**PUT** `/users/{id}/toggle-status`

Activates or deactivates a user account.

**Path Parameters:**
- `id` - User GUID

**Example:**
```
PUT /users/3fa85f64-5717-4562-b3fc-2c963f66afa6/toggle-status
```

**Response (200 OK):**
```json
{
  "message": "User activated successfully",
  "isActive": true
}
```

**Error Responses:**
- `404 Not Found` - User not found or deleted

---

### 5. Delete User (Soft Delete)
**DELETE** `/users/{id}`

Soft deletes a user (marks as deleted, doesn't remove from database).

**Path Parameters:**
- `id` - User GUID

**Example:**
```
DELETE /users/3fa85f64-5717-4562-b3fc-2c963f66afa6
```

**Response (200 OK):**
```json
{
  "message": "User deleted successfully"
}
```

**Error Responses:**
- `404 Not Found` - User not found or already deleted

---

## Tenants Endpoints

### 1. Get All Tenants
**GET** `/tenants`

Retrieves all hospital/tenant records.

**Response (200 OK):**
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "City General Hospital",
    "address": "123 Main St, City, State 12345",
    "contactEmail": "contact@cityhospital.com",
    "contactPhone": "+1234567890",
    "subscriptionPlan": "Premium",
    "isActive": true,
    "subscriptionStartDate": "2025-01-01T00:00:00Z",
    "subscriptionEndDate": "2025-12-31T23:59:59Z"
  }
]
```

---

### 2. Get Tenant by ID
**GET** `/tenants/{id}`

Retrieves a specific tenant's details.

**Response (200 OK):**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "City General Hospital",
  "address": "123 Main St, City, State 12345",
  "contactEmail": "contact@cityhospital.com",
  "contactPhone": "+1234567890",
  "subscriptionPlan": "Premium",
  "isActive": true,
  "subscriptionStartDate": "2025-01-01T00:00:00Z",
  "subscriptionEndDate": "2025-12-31T23:59:59Z"
}
```

---

### 3. Create Tenant
**POST** `/tenants`

Creates a new hospital/tenant.

**Request Body:**
```json
{
  "name": "City General Hospital",
  "address": "123 Main St, City, State 12345",
  "contactEmail": "contact@cityhospital.com",
  "contactPhone": "+1234567890",
  "subscriptionPlan": "Premium",
  "subscriptionStartDate": "2025-01-01T00:00:00Z",
  "subscriptionEndDate": "2025-12-31T23:59:59Z"
}
```

**Response (200 OK):**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "City General Hospital",
  "isActive": true
}
```

---

### 4. Update Tenant
**PUT** `/tenants/{id}`

Updates an existing tenant.

**Request Body:**
```json
{
  "name": "City General Hospital - Updated",
  "address": "456 New St, City, State 12345",
  "contactEmail": "newcontact@cityhospital.com",
  "contactPhone": "+0987654321",
  "subscriptionPlan": "Enterprise"
}
```

**Response (200 OK):**
```json
{
  "message": "Tenant updated successfully"
}
```

---

### 5. Delete Tenant
**DELETE** `/tenants/{id}`

Soft deletes a tenant.

**Response (200 OK):**
```json
{
  "message": "Tenant deleted successfully"
}
```

---

## Common Response Codes

- `200 OK` - Request successful
- `400 Bad Request` - Invalid request data
- `401 Unauthorized` - Missing or invalid authentication token
- `403 Forbidden` - Insufficient permissions
- `404 Not Found` - Resource not found
- `409 Conflict` - Resource already exists
- `500 Internal Server Error` - Server error

---

## Error Response Format

All error responses follow this format:

```json
{
  "message": "Error description here"
}
```

---

## Notes

### Global User Model
- Users are **global** entities (not tied to a specific hospital)
- A single user account can access multiple hospitals
- Tenant context is determined by the resource being accessed (appointments, time slots, etc.)

### Authentication Flow
1. Register or login to get JWT token
2. Include token in `Authorization: Bearer <token>` header for all subsequent requests
3. Token expires after 24 hours
4. Use refresh token endpoint to get a new token without re-logging in

### Pagination
Most list endpoints support pagination with these query parameters:
- `page` - Page number (1-indexed)
- `pageSize` - Number of items per page

Response includes pagination metadata:
```json
{
  "data": [...],
  "pagination": {
    "currentPage": 1,
    "pageSize": 10,
    "totalCount": 100,
    "totalPages": 10
  }
}
```

### Soft Deletes
The system uses soft deletes - deleted records are marked as `IsDeleted = true` but not removed from the database. Soft-deleted records are automatically filtered out from query results.

---

## Testing with Swagger

The API includes Swagger UI for interactive testing:

**URL:** `http://localhost:5000/swagger`

1. Navigate to Swagger UI
2. Click "Authorize" button
3. Enter: `Bearer <your_jwt_token>`
4. Test endpoints interactively

---

## Example Usage

### Complete Registration and Login Flow

**1. Register a new user:**
```bash
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "doctor@example.com",
    "password": "SecurePass123",
    "firstName": "Dr. Jane",
    "lastName": "Smith",
    "phone": "+1234567890",
    "userType": 2
  }'
```

**2. Login:**
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "doctor@example.com",
    "password": "SecurePass123"
  }'
```

**3. Get all users (with token):**
```bash
curl -X GET http://localhost:5000/api/users?page=1&pageSize=10 \
  -H "Authorization: Bearer <your_token_here>"
```

**4. Get user statistics:**
```bash
curl -X GET http://localhost:5000/api/users/stats \
  -H "Authorization: Bearer <your_token_here>"
```

**5. Search for users:**
```bash
curl -X GET "http://localhost:5000/api/users?searchTerm=jane&userType=Doctor" \
  -H "Authorization: Bearer <your_token_here>"
```
