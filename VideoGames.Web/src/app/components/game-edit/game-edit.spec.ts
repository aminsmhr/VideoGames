import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { of } from 'rxjs';
import { describe, it, expect, beforeEach, vi } from 'vitest';

import { GameEdit } from './game-edit';
import { GameApiService } from '../../services/game-api-service/game-api-service';

describe('GameEdit', () => {
  let component: GameEdit;
  let fixture: ComponentFixture<GameEdit>;

  beforeEach(async () => {
    const apiMock = {
      get: vi.fn(() =>
        of({ id: '1', title: 'Test', platform: 'PC', releaseYear: 2020, price: 10 })
      ),
      update: vi.fn(() => of(void 0)),
    };

    const routerMock = {
      navigate: vi.fn(),
    };

    const routeMock = {
      snapshot: {
        paramMap: {
          get: vi.fn(() => '1'),
        },
      },
    };

    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule],
      declarations: [GameEdit],
      providers: [
        { provide: GameApiService, useValue: apiMock },
        { provide: Router, useValue: routerMock },
        { provide: ActivatedRoute, useValue: routeMock },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(GameEdit);
    component = fixture.componentInstance;

    // triggers ngOnInit + bindings
    fixture.detectChanges();
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});