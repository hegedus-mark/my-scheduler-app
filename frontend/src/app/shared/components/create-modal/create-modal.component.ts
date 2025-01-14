import { Component, inject } from "@angular/core";
import { LucideAngularModule, X } from "lucide-angular";
import { FormsModule } from "@angular/forms";
import { TaskFormComponent } from "@shared/components/create-modal/forms/task-form/task-form.component";
import { EventFormComponent } from "@shared/components/create-modal/forms/event-form/event-form.component";
import { CreateModalService } from "@shared/components/create-modal/service/create-modal.service";

@Component({
  selector: "app-create-modal",
  imports: [
    LucideAngularModule,
    FormsModule,
    TaskFormComponent,
    EventFormComponent,
  ],
  templateUrl: "./create-modal.component.html",
  styleUrl: "./create-modal.component.scss",
})
export class CreateModalComponent {
  modalService = inject(CreateModalService);

  modalOpen = this.modalService.isOpen;

  readonly X = X;
}
