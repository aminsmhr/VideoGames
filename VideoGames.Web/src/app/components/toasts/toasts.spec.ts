import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CommonModule } from '@angular/common';
import { describe, it, expect, beforeEach } from 'vitest';

import { Toasts } from './toasts';
import { ToastService } from '../../services/toast-service/toast-service';

describe('Toasts', () => {
  let component: Toasts;
  let fixture: ComponentFixture<Toasts>;

  beforeEach(async () => {
    const toastServiceMock = {
      // if your component uses `toastService.toasts()` (signal)
      toasts: () => [],
      // if you also call remove() from template
      remove: () => {},
    };

    await TestBed.configureTestingModule({
      imports: [CommonModule],
      declarations: [Toasts],
      providers: [{ provide: ToastService, useValue: toastServiceMock }],
    }).compileComponents();

    fixture = TestBed.createComponent(Toasts);
    component = fixture.componentInstance;

    fixture.detectChanges();
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});