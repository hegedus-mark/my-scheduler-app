import { computed, inject, Injectable, signal } from "@angular/core";
import { TaskManagerService } from "@core/task/task-manager.service";
import { TaskFilters } from "@features/task-manager/models/task-manager.model";

@Injectable({
  providedIn: "root",
})
export class TaskFilterService {
  private taskManager = inject(TaskManagerService);

  private tasks = this.taskManager.Tasks;
  private filters = signal<TaskFilters>({});

  filteredTasks = computed(() => {
    const currentTasks = this.tasks();
    const currentFilters = this.filters();

    if (this.isEmptyFilters(currentFilters)) {
      return currentTasks;
    }

    return currentTasks.filter((task) => {
      const matchesDueDate =
        !currentFilters.dueDates?.length ||
        currentFilters.dueDates.some((date) =>
          this.isSameDate(task.dueDate, date),
        );

      const matchesLength =
        !currentFilters.durations?.length ||
        currentFilters.durations.includes(task.duration);

      const matchesPriority =
        !currentFilters.priorities?.length ||
        currentFilters.priorities.includes(task.priority);

      const matchesStatus =
        !currentFilters.statuses?.length ||
        currentFilters.statuses.includes(task.status);

      return (
        matchesDueDate && matchesLength && matchesPriority && matchesStatus
      );
    });
  });

  updateFilters(filters: TaskFilters) {
    this.filters.set(filters);
  }

  private isEmptyFilters(filters: TaskFilters): boolean {
    return !Object.values(filters).some(
      (value) => Array.isArray(value) && value.length > 0,
    );
  }

  private isSameDate(date1: Date, date2: Date): boolean {
    return (
      date1.getFullYear() === date2.getFullYear() &&
      date1.getMonth() === date2.getMonth() &&
      date1.getDate() === date2.getDate()
    );
  }
}
