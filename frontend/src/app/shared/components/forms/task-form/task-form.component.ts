import {
  Component,
  computed,
  effect,
  EffectRef,
  inject,
  input,
  OnDestroy,
  signal,
} from "@angular/core";
import { FormsModule } from "@angular/forms";
import {
  CreateTaskRequest,
  PriorityLevel,
  UpdateTaskRequest,
} from "@myschedulerapp/api-client";
import { TaskManagerService } from "@core/task/task-manager.service";
import { ModalService } from "@shared/components/modal/service/modal.service";
import { TimeSpan } from "@shared/models/timespan.model";
import { DatePickerComponent } from "@shared/components/date-picker/date-picker.component";
import { TaskForm } from "@shared/components/forms/forms.models";

const defaultTaskForm: TaskForm = {
  name: "",
  dueDate: new Date(Date.now()),
  duration: new TimeSpan(),
  priority: "Medium" as PriorityLevel,
};

@Component({
  selector: "app-task-form",
  imports: [FormsModule, DatePickerComponent],
  templateUrl: "./task-form.component.html",
  styleUrl: "./task-form.component.scss",
})
export class TaskFormComponent implements OnDestroy {
  private taskManagerService = inject(TaskManagerService);
  private createModalService = inject(ModalService);

  private effectRef: EffectRef;

  constructor() {
    this.effectRef = effect(() => {
      const input = this.inputTaskForm();
      this.formState.update((current) => ({
        ...current,
        name: input.name,
        dueDate: input.dueDate,
        duration: input.duration,
        priority: input.priority,
      }));

      this.durationString.set(input.duration.toHourMinuteString());
    });
  }

  ngOnDestroy() {
    this.effectRef.destroy();
  }

  // Input signal for initial values
  inputTaskForm = input(defaultTaskForm, {
    transform: (value: TaskForm | null) => value ?? defaultTaskForm,
  });
  taskFormType = input<"Create" | "Update">("Create");
  taskId = input<string | null>(null);

  // Internal form state
  formState = signal<TaskForm>(this.inputTaskForm());
  buttonText = computed(() => {
    return this.taskFormType() === "Create" ? "Create Task" : "Update Task";
  });

  durationString = signal("00:00");
  readonly priorities: PriorityLevel[] = ["Low", "Medium", "High"] as const;

  async onSubmit() {
    if (this.taskFormType() === "Create") {
      await this.CreateTask();
    } else if (this.taskFormType() === "Update") {
      await this.UpdateTask();
    }

    this.createModalService.close();
  }

  private async CreateTask() {
    const dueDate = new Date(this.formState().dueDate);

    const createRequest: CreateTaskRequest = {
      duration: this.formState().duration.toString(),
      dueDate: dueDate.toISOString(),
      name: this.formState().name,
      priority: this.formState().priority,
    };
    await this.taskManagerService.createTask(createRequest);
  }

  private async UpdateTask() {
    if (!this.taskId()) {
      throw new Error("Task Id is required");
    }

    const dueDate = new Date(this.formState().dueDate);

    const updateRequest: UpdateTaskRequest = {
      duration: this.formState().duration.toString(),
      dueDate: dueDate.toISOString(),
      name: this.formState().name,
      priority: this.formState().priority,
    };

    await this.taskManagerService.updateTask(this.taskId()!, updateRequest);
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
      this.formState.update((prev) => ({
        ...prev,
        duration: new TimeSpan({ hours, minutes }),
      }));
      this.durationString.set(value);
    }
  }
}
