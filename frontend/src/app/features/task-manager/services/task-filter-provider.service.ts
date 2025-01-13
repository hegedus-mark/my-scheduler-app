import { Injectable } from "@angular/core";
import { Task } from "@core/task/task.model";
import { TaskFilters } from "@features/task-manager/models/task-manager.model";

@Injectable({
  providedIn: "root",
})
export class TaskFilterProvider {
  private isSameDate(date1: Date, date2: Date): boolean {
    return (
      date1.getFullYear() === date2.getFullYear() &&
      date1.getMonth() === date2.getMonth() &&
      date1.getDate() === date2.getDate()
    );
  }

  private isEmptyFilters(filters: TaskFilters): boolean {
    return !Object.values(filters).some(
      (value) => Array.isArray(value) && value.length > 0,
    );
  }

  applyFilters(tasks: Task[], filters: TaskFilters): Task[] {
    if (this.isEmptyFilters(filters)) {
      return tasks;
    }

    return tasks.filter((task) => {
      const matchesDueDate =
        !filters.dueDates?.length ||
        filters.dueDates.some((date) => this.isSameDate(task.dueDate, date));

      const matchesLength =
        !filters.durations?.length || filters.durations.includes(task.duration);

      const matchesPriority =
        !filters.priorities?.length ||
        filters.priorities.includes(task.priority);

      const matchesStatus =
        !filters.statuses?.length || filters.statuses.includes(task.status);

      return (
        matchesDueDate && matchesLength && matchesPriority && matchesStatus
      );
    });
  }
}
