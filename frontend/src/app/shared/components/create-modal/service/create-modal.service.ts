import { Injectable, signal } from "@angular/core";
import { ModalType } from "@shared/components/create-modal/create-modal.models";

@Injectable({
  providedIn: "root",
})
export class CreateModalService {
  private modalOpenState = signal(false);
  private modalTypeState = signal<ModalType>("event");

  readonly isOpen = this.modalOpenState.asReadonly();
  readonly type = this.modalTypeState.asReadonly();

  open(type: ModalType) {
    this.modalTypeState.set(type);
    this.modalOpenState.set(true);
  }

  close() {
    this.modalOpenState.set(false);
  }

  changeType(type: ModalType) {
    this.modalTypeState.set(type);
  }
}
