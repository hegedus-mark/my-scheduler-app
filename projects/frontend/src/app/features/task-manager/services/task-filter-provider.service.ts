import { computed, Injectable, signal } from "@angular/core";
import { Task } from "@core/task/task.model";
import { TaskFilters } from "@features/task-manager/models/task-manager.model";

@Injectable({
  providedIn: "root",
})
export class TaskFilterProvider {
  private readonly filterState = signal<TaskFilters>({
    searchQuery: "",
    priorities: [],
    statuses: [],
  });

  readonly filters = this.filterState.asReadonly();

  readonly searchQuery = computed(() => this.filterState().searchQuery);
  readonly priorities = computed(() => this.filterState().priorities);
  readonly statuses = computed(() => this.filterState().statuses);

  updateFilters(filters: Partial<TaskFilters>) {
    this.filterState.update((current) => ({
      ...current,
      ...filters,
    }));
  }

  private isEmptyFilters(filters: TaskFilters): boolean {
    return !Object.values(filters).some(
      (value) => Array.isArray(value) && value.length > 0,
    );
  }

  applyFilters(tasks: Task[]): Task[] {
    const filters = this.filterState();

    if (this.isEmptyFilters(filters)) {
      return tasks;
    }

    return tasks.filter((task) => {
      const matchesPriority =
        !filters.priorities?.length ||
        filters.priorities.includes(task.priority);

      const matchesStatus =
        !filters.statuses?.length || filters.statuses.includes(task.status);

      return matchesPriority && matchesStatus;
    });
  }
}
