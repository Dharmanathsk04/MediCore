# Healthcare SaaS - Quick Setup Guide

## Step-by-Step Installation

### 1. Prerequisites Installation

#### Install .NET 9.0 SDK
1. Download from: https://dotnet.microsoft.com/download/dotnet/9.0
2. Run installer and follow prompts
3. Verify installation:
   ```bash
   dotnet --version
   ```

#### Install Node.js and npm
1. Download from: https://nodejs.org/ (LTS version)
2. Run installer
3. Verify installation:
   ```bash
   node --version
   npm --version
   ```

#### Install Angular CLI
```bash
npm install -g @angular/cli@19
ng version
```

#### SQL Server Setup
1. Install SQL Server 2019 or later
2. Install SQL Server Management Studio (SSMS)
3. Create a new database or use the default instance

### 2. Backend Configuration

#### Step 1: Update Connection String
Navigate to `src/HealthcareSaaS.API/appsettings.json` and update:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=HealthcareSaaSDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

**For SQL Server Authentication:**
```json
"DefaultConnection": "Server=localhost;Database=HealthcareSaaSDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
```

#### Step 2: Install EF Core Tools
```bash
dotnet tool install --global dotnet-ef
```

#### Step 3: Create Database
```bash
cd src/HealthcareSaaS.API
dotnet ef migrations add InitialCreate --project ../HealthcareSaaS.Infrastructure --startup-project .
dotnet ef database update
```

#### Step 4: Run Backend
```bash
dotnet run
```

The API will start at:
- HTTPS: `https://localhost:7001`
- HTTP: `http://localhost:5001`
- Swagger: `https://localhost:7001/swagger`

### 3. Frontend Configuration

#### Step 1: Install Dependencies
```bash
cd client/healthcare-app
npm install
```

#### Step 2: Configure API URL
Edit `src/environments/environment.ts`:
```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7001/api'  // Match your backend URL
};
```

#### Step 3: Run Frontend
```bash
npm start
```

The app will open at: `http://localhost:4200`

### 4. Initial Data Setup

#### Create a Tenant (Hospital)
Use Swagger UI or Postman to create a tenant:

**POST** `https://localhost:7001/api/tenants`
```json
{
  "name": "City General Hospital",
  "address": "123 Main Street, City",
  "contactEmail": "admin@cityhospital.com",
  "contactPhone": "+1234567890",
  "subscriptionPlan": "Premium"
}
```

#### Register First User
**POST** `https://localhost:7001/api/auth/register`
```json
{
  "email": "admin@cityhospital.com",
  "password": "Admin@123",
  "firstName": "John",
  "lastName": "Doe",
  "phone": "+1234567890",
  "userType": 4,
  "tenantId": "TENANT_ID_FROM_PREVIOUS_STEP"
}
```

#### Login
**POST** `https://localhost:7001/api/auth/login`
```json
{
  "email": "admin@cityhospital.com",
  "password": "Admin@123",
  "tenantId": "TENANT_ID"
}
```

### 5. Verify Installation

1. **Backend Health Check**
   - Open `https://localhost:7001/swagger`
   - You should see the API documentation

2. **Frontend Check**
   - Open `http://localhost:4200`
   - You should see the login page

3. **Database Check**
   - Open SQL Server Management Studio
   - Connect to your server
   - Verify `HealthcareSaaSDb` database exists
   - Check that tables are created

### 6. Common Issues & Solutions

#### Issue: Database Connection Failed
**Solution:**
- Verify SQL Server is running
- Check connection string in appsettings.json
- Ensure Windows Authentication or SQL Auth credentials are correct
- Try adding `TrustServerCertificate=True` to connection string

#### Issue: CORS Error in Browser
**Solution:**
- Verify Angular app URL in `Program.cs` CORS policy
- Default is `http://localhost:4200`
- Clear browser cache and restart

#### Issue: JWT Token Not Working
**Solution:**
- Check JWT configuration in appsettings.json
- Ensure Key is at least 32 characters
- Verify token is being sent in Authorization header

#### Issue: npm install fails
**Solution:**
```bash
# Clear npm cache
npm cache clean --force
# Delete node_modules and package-lock.json
rm -rf node_modules package-lock.json
# Reinstall
npm install
```

#### Issue: Migration fails
**Solution:**
```bash
# Remove existing migrations
rm -rf src/HealthcareSaaS.Infrastructure/Migrations
# Create new migration
dotnet ef migrations add InitialCreate --project src/HealthcareSaaS.Infrastructure --startup-project src/HealthcareSaaS.API
# Update database
dotnet ef database update --project src/HealthcareSaaS.API
```

### 7. Development Workflow

#### Running Both Backend and Frontend

**Terminal 1 - Backend:**
```bash
cd src/HealthcareSaaS.API
dotnet watch run
```

**Terminal 2 - Frontend:**
```bash
cd client/healthcare-app
npm start
```

#### Making Database Changes

1. Modify entities in `HealthcareSaaS.Domain/Entities`
2. Create migration:
   ```bash
   cd src/HealthcareSaaS.API
   dotnet ef migrations add YourMigrationName --project ../HealthcareSaaS.Infrastructure
   ```
3. Update database:
   ```bash
   dotnet ef database update
   ```

#### Adding New API Endpoints

1. Create DTO in `Application/Features/[Feature]/DTOs`
2. Create Command/Query in `Application/Features/[Feature]/Commands` or `Queries`
3. Create Handler for the command/query
4. Add controller method in `API/Controllers`

#### Adding New Angular Components

```bash
cd client/healthcare-app
ng generate component features/[feature-name]/[component-name] --standalone
```

### 8. Testing the Application

#### Test User Roles

Create users with different roles to test:

**Patient:**
```json
{
  "email": "patient@test.com",
  "password": "Patient@123",
  "firstName": "Jane",
  "lastName": "Patient",
  "phone": "+1234567891",
  "userType": 1,
  "tenantId": "YOUR_TENANT_ID"
}
```

**Doctor:**
```json
{
  "email": "doctor@test.com",
  "password": "Doctor@123",
  "firstName": "Dr. Mike",
  "lastName": "Smith",
  "phone": "+1234567892",
  "userType": 2,
  "tenantId": "YOUR_TENANT_ID"
}
```

### 9. Production Deployment

#### Backend Deployment
```bash
cd src/HealthcareSaaS.API
dotnet publish -c Release -o ./publish
```

#### Frontend Deployment
```bash
cd client/healthcare-app
npm run build --prod
```

Deploy the contents of `dist/healthcare-app/` to your web server.

### 10. Next Steps

1. ✅ Complete the remaining CRUD operations for all entities
2. ✅ Add more sophisticated UI components
3. ✅ Implement real-time notifications
4. ✅ Add comprehensive error handling
5. ✅ Implement unit and integration tests
6. ✅ Add logging and monitoring
7. ✅ Implement caching strategies
8. ✅ Add email/SMS notifications

---

**Need Help?** Check the main README.md for detailed documentation.
