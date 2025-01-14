/* eslint-disable */
import {
  ChangeDetectionStrategy,
  Component,
  computed,
  HostListener,
  inject,
  OnDestroy,
  signal,
} from "@angular/core";
import { FormsModule } from "@angular/forms";
import { BaseTask, Task } from "@core/task/task.model";
import { TaskFilterProvider } from "@features/task-manager/services/task-filter-provider.service";
import { LucideAngularModule, Plus } from "lucide-angular";
import { CreateModalService } from "@shared/components/create-modal/service/create-modal.service";
import { CreateModalComponent } from "@shared/components/create-modal/create-modal.component";
import { TaskManagerService } from "@core/task/task-manager.service";
import {
  TaskFilters,
  TaskListItem,
} from "@features/task-manager/models/task-manager.model";
import { TaskItemComponent } from "@features/task-manager/components/task-item/task-item.component";
import { PriorityLevel } from "@myschedulerapp/api-client";
import { AccordionService } from "@features/task-manager/services/accordion.service";

@Component({
  selector: "app-task-list",
  imports: [
    FormsModule,
    LucideAngularModule,
    CreateModalComponent,
    TaskItemComponent,
  ],
  templateUrl: "./task-list.component.html",
  styleUrl: "./task-list.component.scss",
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TaskListComponent implements OnDestroy {
  taskManager = inject(TaskManagerService);
  private filterProvider = inject(TaskFilterProvider);
  private createModalService = inject(CreateModalService);
  private accordionService = inject(AccordionService);

  public priorities: PriorityLevel[] = ["High", "Medium", "Low"] as const;

  expandedTaskId: any;
  showDraftsOnly: any;
  selectedPriorities = signal<PriorityLevel[]>([]);
  private filters = signal<TaskFilters>({});
  searchQuery: any;

  private filteredTasks = computed(() => {
    const tasks = this.taskManager.Tasks();
    const currentFilters = this.filters();
    return this.filterProvider.applyFilters(tasks, currentFilters);
  });

  selectedTasks = signal(new Map<string, Task>());

  // Final view model for the template
  tasksWithSelection = computed((): TaskListItem[] => {
    const filtered = this.filteredTasks();
    return filtered.map((task) => ({
      task: task,
      selected: this.selectedTasks().has(task.id),
      expanded: this.expandedTaskId === task.id,
    }));
  });

  updateFilters(newFilters: TaskFilters) {
    this.filters.set(newFilters);
  }

  editTask(task: any) {}

  toggleTaskSelection(id: string) {}

  expandTask(id: string) {}

  toggleDraftFilter() {}

  togglePriorityFilter(priority: PriorityLevel) {}

  protected readonly Plus = Plus;

  openModal() {
    this.createModalService.open("task");
  }

  @HostListener("document:click", ["$event"])
  onDocumentClick(event: MouseEvent) {
    this.accordionService.handleDocumentClick(event);
  }

  ngOnDestroy() {
    this.accordionService.collapse();
  }
}
