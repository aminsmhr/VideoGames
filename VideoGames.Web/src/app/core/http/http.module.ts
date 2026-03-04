import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ToastInterceptor } from './interceptors/toast-interceptor';

@NgModule({
  imports: [HttpClientModule],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ToastInterceptor, multi: true }
  ],
})
export class HttpModule {}
