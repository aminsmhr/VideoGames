import { Component, OnInit } from '@angular/core';
import { GameApiService, VideoGameDto } from '../../services/game-api-service/game-api-service'
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-game-list',
  standalone: false,
  templateUrl: './game-list.html',
  styleUrl: './game-list.css',
})
export class GameList implements OnInit {
  games: VideoGameDto[] = [];
  loading = true;
  error: string | null = null;

  constructor(private api: GameApiService, private router: Router, private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {

    this.api.list().subscribe({
      next: g => {
        this.games = g;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: e => {
        this.error = 'Failed to load games.';
        this.loading = false;
      }
    });
  }

  edit(id: string) {
    this.router.navigate(['/games', id, 'edit']);
  }
}
