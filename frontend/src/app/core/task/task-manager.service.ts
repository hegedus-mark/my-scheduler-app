import { computed, inject, Injectable, resource } from "@angular/core";
import { firstValueFrom } from "rxjs";
import {
  CreateTaskRequest,
  TaskService,
  UpdateTaskRequest,
} from "@myschedulerapp/api-client";
import { TaskAdapter } from "@core/task/task.adapter";

@Injectable({
  providedIn: "root",
})
export class TaskManagerService {
  private taskApi: TaskService = inject(TaskService);

  // Main tasks resource for fetching all tasks
  private readonly tasksResource = resource({
    request: () => ({}),
    loader: async () => {
      const response = await firstValueFrom(this.taskApi.apiTaskAllGet());
      return response.data ? response.data.map(TaskAdapter.toTask) : [];
    },
  });

  public readonly Tasks = computed(() => this.tasksResource.value() ?? []);

  public readonly isLoading = this.tasksResource.isLoading;
  public readonly error = this.tasksResource.error;

  public async createTask(createRequest: CreateTaskRequest): Promise<void> {
    const response = await firstValueFrom(
      this.taskApi.apiTaskPost(createRequest),
    );
    if (response.data) {
      const newTask = TaskAdapter.toTask(response.data);
      // Update the local resource state
      this.tasksResource.update((tasks) => [...(tasks ?? []), newTask]);
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
      this.tasksResource.update(
        (tasks) =>
          tasks?.map((task) => (task.id === taskId ? updatedTask : task)) ?? [],
      );
    }
  }

  public async deleteTask(taskId: string): Promise<void> {
    await firstValueFrom(this.taskApi.apiTaskIdDelete(taskId));
    this.tasksResource.update(
      (tasks) => tasks?.filter((task) => task.id !== taskId) ?? [],
    );
  }

  // Method to manually refresh the tasks list
  public refresh(): void {
    this.tasksResource.reload();
  }
}
