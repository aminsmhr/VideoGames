import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GameList } from './components/game-list/game-list';
import { GameEdit } from './components/game-edit/game-edit';

const routes: Routes = [
  { path: '', component: GameList },
  { path: 'games/:id/edit', component: GameEdit },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
