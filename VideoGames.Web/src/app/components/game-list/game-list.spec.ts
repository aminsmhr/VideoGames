import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { ChangeDetectorRef } from '@angular/core';
import { of, throwError } from 'rxjs';
import { describe, it, expect, beforeEach, vi } from 'vitest';

import { GameList } from './game-list';
import { GameApiService } from '../../services/game-api-service/game-api-service';

describe('GameList', () => {
  let component: GameList;
  let fixture: ComponentFixture<GameList>;

  beforeEach(async () => {
    const apiMock = {
      list: vi.fn(() =>
        of([{ id: '1', title: 'Test', platform: 'PC', releaseYear: 2020, price: 10 }])
      ),
    };

    const routerMock = {
      navigate: vi.fn(),
    };

    const cdrMock = {
      detectChanges: vi.fn(),
    };

    await TestBed.configureTestingModule({
      declarations: [GameList],
      providers: [
        { provide: GameApiService, useValue: apiMock },
        { provide: Router, useValue: routerMock },
        { provide: ChangeDetectorRef, useValue: cdrMock },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(GameList);
    component = fixture.componentInstance;

    fixture.detectChanges();
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load games on init', () => {
    expect(component.loading()).toBe(false);
    expect(component.error()).toBeNull();
    expect(component.games().length).toBe(1);
    expect(component.games()[0].title).toBe('Test');
  });

  it('edit should navigate to edit route', () => {
    const router = TestBed.inject(Router) as unknown as { navigate: ReturnType<typeof vi.fn> };
    component.edit('123');
    expect(router.navigate).toHaveBeenCalledWith(['/games', '123', 'edit']);
  });
});
