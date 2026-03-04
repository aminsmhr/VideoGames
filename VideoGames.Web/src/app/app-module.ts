import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing-module';
// HttpModule bundles HttpClientModule and interceptor providers
import { HttpModule } from './core/http/http.module';

import { App } from './app';
import { GameList } from './components/game-list/game-list';
import { GameEdit } from './components/game-edit/game-edit';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgbNavLinkButton } from '@ng-bootstrap/ng-bootstrap';
import { Toasts } from './components/toasts/toasts';

@NgModule({
  declarations: [App, GameList, GameEdit, Toasts],
  imports: [BrowserModule, AppRoutingModule, ReactiveFormsModule, HttpModule, NgbModule, NgbNavLinkButton],
  providers: [
    provideBrowserGlobalErrorListeners()
  ],
  bootstrap: [App],
})
export class AppModule {}
