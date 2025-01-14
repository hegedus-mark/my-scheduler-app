import { Component, inject } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { TaskForm } from "@shared/components/create-modal/create-modal.models";
import { CreateTaskRequest, PriorityLevel } from "@myschedulerapp/api-client";
import { TaskManagerService } from "@core/task/task-manager.service";
import { CreateModalService } from "@shared/components/create-modal/service/create-modal.service";
import { TimeSpan } from "@shared/models/timespan.model";

@Component({
  selector: "app-task-form",
  imports: [FormsModule],
  templateUrl: "./task-form.component.html",
  styleUrl: "./task-form.component.scss",
})
export class TaskFormComponent {
  taskManagerService = inject(TaskManagerService);
  //we might not want to tie it to the createModal in the future, in that case this component should emit a submit event
  createModalService = inject(CreateModalService);

  durationString = "00:00";
  taskForm: TaskForm = {
    name: "",
    dueDate: new Date(),
    duration: new TimeSpan(),
    priority: "Medium",
  };

  readonly priorities: PriorityLevel[] = ["Low", "Medium", "High"] as const;

  async onSubmit() {
    console.log(this.taskForm);
    const createRequest: CreateTaskRequest = {
      duration: this.taskForm.duration.toString(),
      dueDate: this.taskForm.dueDate.toString(),
      name: this.taskForm.name,
      priority: this.taskForm.priority,
    };
    await this.taskManagerService.createTask(createRequest);
    this.createModalService.close();
  }

  validateDuration(value: string): boolean {
    const timeRegex = /^([0-9]{1,2}):([0-9]{2})$/;
    if (!timeRegex.test(value)) return false;

    const [hours, minutes] = value.split(":").map(Number);
    return hours >= 0 && minutes >= 0 && minutes < 60;
  }

  onDurationChange(value: string) {
    if (this.validateDuration(value)) {
      const [hours, minutes] = value.split(":").map(Number);
      this.taskForm.duration = new TimeSpan({ hours, minutes });
      this.durationString = value;
    }
  }
}
