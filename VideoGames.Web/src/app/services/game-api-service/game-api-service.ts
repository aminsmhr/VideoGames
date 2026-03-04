import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface VideoGameDto {
  id: string;
  title: string;
  platform: string;
  releaseYear: number;
  price: number;
}

export interface UpdateVideoGameRequest {
  title: string;
  platform: string;
  releaseYear: number;
  price: number;
}

@Injectable({
  providedIn: 'root',
})
export class GameApiService {
    private baseUrl = 'http://localhost:5076/api';

  constructor(private http: HttpClient) { }

  list(): Observable<VideoGameDto[]> {
    return this.http.get<VideoGameDto[]>(`${this.baseUrl}/games`);
  }

  get(id: string): Observable<VideoGameDto> {
    return this.http.get<VideoGameDto>(`${this.baseUrl}/games/${id}`);
  }

  update(id: string, req: UpdateVideoGameRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/games/${id}`, req);
  }
}
