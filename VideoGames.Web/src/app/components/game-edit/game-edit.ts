import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GameApiService, UpdateVideoGameRequest } from '../../services/game-api-service/game-api-service';
import { ToastService } from '../../services/toast-service/toast-service';

import {
  FormBuilder,
  Validators,
  FormControl,
  FormGroup
} from '@angular/forms';

type GameForm = FormGroup<{
  title: FormControl<string>;
  platform: FormControl<string>;
  releaseYear: FormControl<number>;
  price: FormControl<number>;
}>;

@Component({
  selector: 'app-game-edit',
  standalone: false,
  templateUrl: './game-edit.html',
  styleUrl: './game-edit.css',
})
export class GameEdit implements OnInit {
  id!: string;
  loaded = false;

  form!: GameForm;

constructor(
  private route: ActivatedRoute,
  private router: Router,
  private api: GameApiService,
  private fb: FormBuilder,
  private toast: ToastService
) {
    this.form = this.fb.nonNullable.group({
      title: ['', Validators.required],
      platform: ['', Validators.required],
      releaseYear: [2020, [Validators.required, Validators.min(1970)]],
      price: [0, [Validators.required, Validators.min(0)]],
    }) as GameForm;
  }

  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id')!;

    this.api.get(this.id).subscribe(g => {
      this.form.setValue({
        title: g.title,
        platform: g.platform,
        releaseYear: g.releaseYear,
        price: g.price
      });
      this.loaded = true;
    });
  }

save(): void {
  if (this.form.invalid) return;

  const v = this.form.getRawValue();
  const req: UpdateVideoGameRequest = {
    title: v.title,
    platform: v.platform,
    releaseYear: v.releaseYear,
    price: v.price
  };

  this.api.update(this.id, req).subscribe({
    next: () => {
      this.back();
    },
    error: () => {
      this.toast.show('Update failed', {
        classname: 'bg-danger text-white',
        delay: 3500
      });
    }
  });
}

  back(): void {
    this.router.navigate(['/']);
  }
}
