import { Component, computed, inject } from "@angular/core";
import { LucideAngularModule, X } from "lucide-angular";
import { FormsModule } from "@angular/forms";
import { TaskFormComponent } from "@features/calendar/components/task-form/task-form.component";
import { EventFormComponent } from "@features/calendar/components/event-form/event-form.component";
import { CreateModalService } from "@features/calendar/services/create-modal/create-modal.service";

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
  modalType = this.modalService.type;

  readonly isTaskForm = computed(() => this.modalType() === "task");
  readonly X = X;
}
