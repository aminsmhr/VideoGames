import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { ToastService } from '../../../services/toast-service/toast-service';

@Injectable()
export class ToastInterceptor implements HttpInterceptor {
  private lastSuccess: { method: string; url: string; time: number } | null = null;

  constructor(private toast: ToastService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      tap({
        next: () => {
          const now = Date.now();
          if (
            this.lastSuccess &&
            this.lastSuccess.method === req.method &&
            this.lastSuccess.url === req.url &&
            now - this.lastSuccess.time < 500
          ) {
            this.lastSuccess.time = now;
            return;
          }

          this.lastSuccess = { method: req.method, url: req.url, time: now };

          if (req.method === 'GET') {
            this.toast.show('Loaded', { classname: 'bg-light text-success', delay: 1000 });
          } else {
            this.toast.show('Game updated successfully', { classname: 'bg-success text-white' });
          }
        },
        error: () => {
          this.toast.show('Request failed', { classname: 'bg-danger text-white' });
        }
      })
    );
  }
}
