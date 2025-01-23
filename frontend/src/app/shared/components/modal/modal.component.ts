import { Component, inject } from "@angular/core";
import { LucideAngularModule, X } from "lucide-angular";
import { FormsModule } from "@angular/forms";
import { ModalService } from "@shared/components/modal/service/modal.service";

@Component({
  selector: "app-modal",
  imports: [LucideAngularModule, FormsModule],
  templateUrl: "./modal.component.html",
  styleUrl: "./modal.component.scss",
})
export class ModalComponent {
  modalService = inject(ModalService);

  modalOpen = this.modalService.isOpen;

  readonly X = X;
}
