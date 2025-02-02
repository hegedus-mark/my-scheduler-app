import { HttpErrorResponse, HttpInterceptorFn } from "@angular/common/http";
import { catchError, throwError } from "rxjs";

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      // Let 400s pass through for form handling
      if (error.status === 400) {
        return throwError(() => error);
      }

      switch (error.status) {
        case 401:
          // Handle unauthorized
          console.error("Unauthorized access");
          break;
        case 403:
          // Handle forbidden
          console.error("Forbidden access");
          break;
        case 404:
          // Handle not found
          console.error("Resource not found");
          break;
        case 500:
          // Handle server errors
          console.error("Server error");
          break;
        default:
          console.error("An error occurred");
      }

      return throwError(() => error);
    }),
  );
};
