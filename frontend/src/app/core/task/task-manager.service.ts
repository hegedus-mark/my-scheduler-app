import { inject, Injectable, signal } from "@angular/core";
import { Task } from "./task.model";
import { firstValueFrom } from "rxjs";
import {
  CreateTaskRequest,
  TaskService,
  UpdateTaskRequest,
} from "@myschedulerapp/api-client/src";
import { TaskAdapter } from "@core/task/task.adapter";

@Injectable({
  providedIn: "root",
})
export class TaskManagerService {
  private taskApi: TaskService = inject(TaskService);

  private readonly tasks = signal<readonly Task[]>([]);
  public readonly Tasks = this.tasks.asReadonly();

  constructor() {
    this.loadTasks();
  }

  private async loadTasks(): Promise<void> {
    const response = await firstValueFrom(this.taskApi.apiTaskAllGet());
    if (response.data) {
      this.tasks.set(response.data.map(TaskAdapter.toTask));
    }
  }

  public async createTask(createRequest: CreateTaskRequest): Promise<void> {
    const response = await firstValueFrom(
      this.taskApi.apiTaskPost(createRequest),
    );
    if (response.data) {
      const task = TaskAdapter.toTask(response.data);
      this.tasks.update((t) => [...t, task]);
    }
  }

  public async updateTask(
    taskId: string,
    updateRequest: UpdateTaskRequest,
  ): Promise<void> {
    const response = await firstValueFrom(
      this.taskApi.apiTaskIdPatch(taskId, updateRequest),
    );
    if (response.data) {
      const updatedTask = TaskAdapter.toTask(response.data);
      this.tasks.update((tasks) =>
        tasks.map((task) => (task.id === taskId ? updatedTask : task)),
      );
    }
  }

  public async deleteTask(taskId: string): Promise<void> {
    await firstValueFrom(this.taskApi.apiTaskIdDelete(taskId));
    this.tasks.update((tasks) => tasks.filter((task) => task.id !== taskId));
  }
}
