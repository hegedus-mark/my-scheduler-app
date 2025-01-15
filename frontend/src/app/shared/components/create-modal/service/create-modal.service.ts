import { Injectable, signal } from "@angular/core";

@Injectable({
  providedIn: "root",
})
export class CreateModalService {
  private modalOpenState = signal(false);

  readonly isOpen = this.modalOpenState.asReadonly();

  open() {
    this.modalOpenState.set(true);
  }

  close() {
    this.modalOpenState.set(false);
  }
}
