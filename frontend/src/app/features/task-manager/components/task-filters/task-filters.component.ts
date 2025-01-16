import { Component, output, signal } from "@angular/core";
import { PriorityLevel, TaskItemStatus } from "@myschedulerapp/api-client";
import { TaskFilters } from "@features/task-manager/models/task-manager.model";
import { FormsModule } from "@angular/forms";

@Component({
  selector: "app-task-filters",
  imports: [FormsModule],
  templateUrl: "./task-filters.component.html",
  styleUrl: "./task-filters.component.scss",
})
export class TaskFiltersComponent {
  priorities: PriorityLevel[] = ["High", "Medium", "Low"] as const;
  statuses: TaskItemStatus[] = ["Draft", "Scheduled", "Unscheduled"] as const;

  filtersChange = output<TaskFilters>({});

  searchQuery = signal<string>("");
  selectedStatus = signal<TaskItemStatus[]>([]);
  selectedPriorities = signal<PriorityLevel[]>([]);

  private emitFilters() {
    this.filtersChange.emit({
      searchQuery: this.searchQuery(),
      priorities: this.selectedPriorities(),
      statuses: this.selectedStatus(),
    });
  }

  onSearchChange(query: string) {
    this.searchQuery.set(query);
    this.emitFilters();
  }

  togglePriorityFilter(priority: PriorityLevel) {
    const current = this.selectedPriorities();
    const index = current.indexOf(priority);

    if (index === -1) {
      this.selectedPriorities.set([...current, priority]);
    } else {
      this.selectedPriorities.set(current.filter((p) => p !== priority));
    }

    this.emitFilters();
  }

  toggleStatusFilter(status: TaskItemStatus) {
    const current = this.selectedStatus();
    const index = current.indexOf(status);
    if (index === -1) {
      this.selectedStatus.set([...current, status]);
    } else {
      this.selectedStatus.set(current.filter((p) => p !== status));
    }

    this.emitFilters();
  }
}
