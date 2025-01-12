/* eslint-disable */
import { Component, computed, inject, signal } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { BaseTask, Task } from "@core/task/task.model";
import { TaskFilterService } from "@features/task-manager/services/task-filter.service";
import { LucideAngularModule, Plus } from "lucide-angular";
import { CreateModalService } from "@shared/components/create-modal/service/create-modal.service";
import { CreateModalComponent } from "@shared/components/create-modal/create-modal.component";

type priority = "High" | "Medium" | "Low";

interface ListTask extends BaseTask {
  selected: boolean;
  status: "Draft" | "Scheduled";
}

@Component({
  selector: "app-task-list",
  imports: [FormsModule, LucideAngularModule, CreateModalComponent],
  templateUrl: "./task-list.component.html",
  styleUrl: "./task-list.component.scss",
})
export class TaskListComponent {
  private taskFilterService = inject(TaskFilterService);
  private createModalService = inject(CreateModalService);

  public priorities: priority[] = ["High", "Medium", "Low"] as const;

  expandedTaskId: any;
  showDraftsOnly: any;
  selectedPriorities = signal<priority[]>([]);
  filteredTasks = this.taskFilterService.filteredTasks;
  searchQuery: any;

  selectedTasks = signal(new Map<string, Task>());
  tasksWithSelection = computed(() => {
    const filtered = this.filteredTasks();
    return filtered.map((task) => ({
      ...task,
      selected: this.selectedTasks().has(task.id),
    }));
  });

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

  protected readonly Plus = Plus;

  openModal() {
    this.createModalService.open("task");
  }
}
