import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { describe, it, expect, beforeEach } from 'vitest';

import { GameApiService } from './game-api-service';

describe('GameApiService', () => {
  let service: GameApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
    });

    service = TestBed.inject(GameApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});