import { Component, computed, signal } from "@angular/core";
import { ModalType } from "@features/calendar/types/calendar.types";
import { LucideAngularModule, X } from "lucide-angular";
import { FormsModule } from "@angular/forms";
import { TitleCasePipe } from "@angular/common";
import { TaskFormComponent } from "@features/calendar/components/task-form/task-form.component";
import { EventFormComponent } from "@features/calendar/components/event-form/event-form.component";

@Component({
  selector: "app-create-modal",
  imports: [
    LucideAngularModule,
    FormsModule,
    TitleCasePipe,
    TaskFormComponent,
    EventFormComponent,
  ],
  templateUrl: "./create-modal.component.html",
  styleUrl: "./create-modal.component.scss",
})
export class CreateModalComponent {
  modalOpen = signal(false);
  modalType = signal<ModalType>("event");

  readonly isTaskForm = computed(() => this.modalType() === "task");
  readonly X = X;

  open(type: ModalType) {
    this.modalType.set(type);
    this.modalOpen.set(true);
  }

  close() {
    this.modalOpen.set(false);
  }

  onSubmit() {
    if (this.isTaskForm()) {
      console.log("Task submitted from modal");
    } else {
      console.log("Event submitted from modal");
    }
    this.close();
  }
}
