import { Component, OnInit, signal } from '@angular/core';
import { GameApiService, VideoGameDto } from '../../services/game-api-service/game-api-service'
import { Router } from '@angular/router';

@Component({
  selector: 'app-game-list',
  standalone: false,
  templateUrl: './game-list.html',
  styleUrl: './game-list.css',
})
export class GameList implements OnInit {
  readonly games = signal<VideoGameDto[]>([]);
  readonly loading = signal(true);
  readonly error = signal<string | null>(null);

  constructor(private api: GameApiService, private router: Router) {}

  ngOnInit(): void {

    this.api.list().subscribe({
      next: g => {
        this.games.set(g);
        this.loading.set(false);
      },
      error: e => {
        this.error.set('Failed to load games.');
        this.loading.set(false);
      }
    });
  }

  edit(id: string) {
    this.router.navigate(['/games', id, 'edit']);
  }
}
