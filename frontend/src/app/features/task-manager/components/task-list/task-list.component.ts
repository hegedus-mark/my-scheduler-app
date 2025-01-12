/* eslint-disable */
import { Component, computed, inject, signal } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { BaseTask } from "@core/task/task.model";
import { TaskFilterService } from "@features/task-manager/services/task-filter.service";

type priority = "High" | "Medium" | "Low";

interface ListTask extends BaseTask {
  selected: boolean;
  status: "Draft" | "Scheduled";
}

@Component({
  selector: "app-task-list",
  imports: [FormsModule],
  templateUrl: "./task-list.component.html",
  styleUrl: "./task-list.component.scss",
})
export class TaskListComponent {
  private taskFilterService = inject(TaskFilterService);

  expandedTaskId: any;
  showDraftsOnly: any;
  selectedPriorities = signal<priority[]>([]);
  filteredTasks = this.taskFilterService.filteredTasks;
  searchQuery: any;

  public priorities: priority[] = ["High", "Medium", "Low"] as const;

  editTask(task: any) {}

  getPriorityClass(priority: "High" | "Low" | "Medium") {
    switch (priority) {
      case "High":
        return "badge bg-red-500";
      case "Medium":
        return "badge bg-green-500";
      case "Low":
        return "badge bg-yellow-500";
    }
  }

  toggleTaskSelection(id: string) {}

  expandTask(id: string) {}

  toggleDraftFilter() {}

  togglePriorityFilter(priority: priority) {}
}
