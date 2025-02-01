import { Component, inject, output } from "@angular/core";
import { PriorityLevel, TaskItemStatus } from "@myschedulerapp/api-client";
import { TaskFilters } from "@features/task-manager/models/task-manager.model";
import { FormsModule } from "@angular/forms";
import { TaskFilterProvider } from "@features/task-manager/services/task-filter-provider.service";

@Component({
  selector: "app-task-filters",
  imports: [FormsModule],
  templateUrl: "./task-filters.component.html",
  styleUrl: "./task-filters.component.scss",
})
export class TaskFiltersComponent {
  private filterService = inject(TaskFilterProvider);

  priorities: PriorityLevel[] = ["High", "Medium", "Low"] as const;
  statuses: TaskItemStatus[] = ["Draft", "Scheduled"] as const;

  filtersChange = output<TaskFilters>({});

  searchQuery = this.filterService.searchQuery;
  selectedStatus = this.filterService.statuses;
  selectedPriorities = this.filterService.priorities;

  onSearchChange(query: string) {
    this.filterService.updateFilters({ searchQuery: query });
  }

  togglePriorityFilter(priority: PriorityLevel) {
    const current = this.selectedPriorities();
    const index = current.indexOf(priority);

    if (index === -1) {
      this.filterService.updateFilters({ priorities: [...current, priority] });
    } else {
      this.filterService.updateFilters({
        priorities: current.filter((p) => p !== priority),
      });
    }
  }

  toggleStatusFilter(status: TaskItemStatus) {
    const current = this.selectedStatus();
    const index = current.indexOf(status);
    if (index === -1) {
      this.filterService.updateFilters({ statuses: [...current, status] });
    } else {
      this.filterService.updateFilters({
        statuses: current.filter((p) => p !== status),
      });
    }
  }
}
