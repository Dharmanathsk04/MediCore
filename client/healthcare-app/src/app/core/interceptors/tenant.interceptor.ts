import { HttpInterceptorFn } from '@angular/common/http';

// Note: Tenant context is now determined server-side based on the resource being accessed
// (e.g., appointment, time slot) rather than from the user's JWT token.
// This interceptor is kept for future use if needed for specific tenant-scoped operations.
export const tenantInterceptor: HttpInterceptorFn = (req, next) => {
  // No longer adding X-Tenant-Id header since users are global
  // Tenant context is managed server-side through resource associations
  return next(req);
};
