import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { HttpClient } from '@angular/common/http';
import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest';

import { HttpModule } from '../http.module';
import { ToastService } from '../../../services/toast-service/toast-service';

describe('ToastInterceptor (integrated via HttpModule)', () => {
  let http: HttpClient;
  let httpMock: HttpTestingController;

  // keep it simple: we only need show()
  let toast: { show: ReturnType<typeof vi.fn> };

  beforeEach(() => {
    toast = { show: vi.fn() };

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, HttpModule],
      providers: [{ provide: ToastService, useValue: toast }],
    });

    http = TestBed.inject(HttpClient);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should show loaded toast for GET requests', () => {
    http.get('/foo').subscribe();

    const req = httpMock.expectOne('/foo');
    req.flush({});

    expect(toast.show).toHaveBeenCalledWith('Loaded', {
      classname: 'bg-light text-success',
      delay: 1000,
    });
  });
});