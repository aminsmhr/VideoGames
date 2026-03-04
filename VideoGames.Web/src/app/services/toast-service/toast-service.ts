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

    this.zone.run(() => {
      this.toasts.update(list => [...list, toast]);
    });

    if (toast.delay && toast.delay > 0) {
      toast.timeoutId = setTimeout(() => {
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

    this.zone.run(() => {
      this.toasts.update(list => list.filter(t => t.id !== id));
    });
  }

  clear() {
    for (const t of this.toasts()) {
      if (t.timeoutId) clearTimeout(t.timeoutId);
    }

    this.zone.run(() => {
      this.toasts.set([]);
    });
  }
}
