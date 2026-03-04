import { TestBed } from '@angular/core/testing';
import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest';

import { ToastService } from './toast-service';

describe('ToastService', () => {
  let service: ToastService;

  beforeEach(() => {
    vi.useFakeTimers();
    TestBed.configureTestingModule({});
    service = TestBed.inject(ToastService);
  });

  afterEach(() => {
    vi.runOnlyPendingTimers();
    vi.useRealTimers();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should publish a toast and auto-remove after delay', () => {
    service.show('hi', { delay: 1000 });

    expect(service.toasts().length).toBe(1);

    vi.advanceTimersByTime(1000);
    vi.runOnlyPendingTimers();

    expect(service.toasts().length).toBe(0);
  });

  it('manual remove clears and stays removed after time passes', () => {
    service.show('bye', { delay: 5000 });

    const toast = service.toasts()[0]!;
    service.remove(toast);

    expect(service.toasts().length).toBe(0);

    vi.advanceTimersByTime(5000);
    vi.runOnlyPendingTimers();

    expect(service.toasts().length).toBe(0);
  });
});