import { Injectable, TemplateRef, signal, NgZone } from '@angular/core';

export interface ToastInfo {
  id: number;
  text?: string;
  template?: TemplateRef<any>;
  classname?: string;
  delay?: number;
  timeoutId?: ReturnType<typeof setTimeout> | null;
}

@Injectable({ providedIn: 'root' })
export class ToastService {
  // keep signal API the same (your components/tests use toasts())
  readonly toasts = signal<ToastInfo[]>([]);

  private nextId = 1;

  constructor(private zone: NgZone) {}

  show(text: string, options: Partial<Omit<ToastInfo, 'id' | 'timeoutId' | 'text'>> = {}) {
    const toast: ToastInfo = {
      id: this.nextId++,
      text,
      delay: 3000,
      timeoutId: null,
      ...options,
    };

    // ✅ update synchronously (no microtask)
    this.zone.run(() => {
      this.toasts.update(list => [...list, toast]);
    });

    // ✅ schedule auto-remove
    if (toast.delay && toast.delay > 0) {
      toast.timeoutId = setTimeout(() => {
        // run removal inside zone
        this.zone.run(() => this.removeById(toast.id));
      }, toast.delay);
    }
  }

  remove(toast: ToastInfo) {
    this.removeById(toast.id);
  }

  removeById(id: number) {
    const existing = this.toasts().find(t => t.id === id);
    if (!existing) return;

    if (existing.timeoutId) {
      clearTimeout(existing.timeoutId);
    }

    // ✅ remove synchronously
    this.zone.run(() => {
      this.toasts.update(list => list.filter(t => t.id !== id));
    });
  }

  clear() {
    // clear timers first
    for (const t of this.toasts()) {
      if (t.timeoutId) clearTimeout(t.timeoutId);
    }

    this.zone.run(() => {
      this.toasts.set([]);
    });
  }
}