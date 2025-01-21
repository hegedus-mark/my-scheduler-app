/* eslint-disable */
import {
  ChangeDetectionStrategy,
  Component,
  computed,
  DestroyRef,
  HostListener,
  inject,
  OnDestroy,
  OnInit,
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
import { TaskFormComponent } from "@shared/components/create-modal/forms/task-form/task-form.component";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { TaskFiltersComponent } from "@features/task-manager/components/task-filters/task-filters.component";

@Component({
  selector: "app-task-list",
  imports: [
    FormsModule,
    LucideAngularModule,
    CreateModalComponent,
    TaskItemComponent,
    TaskFormComponent,
    TaskFiltersComponent,
  ],
  templateUrl: "./task-list.component.html",
  styleUrl: "./task-list.component.scss",
  providers: [AccordionService],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TaskListComponent implements OnDestroy, OnInit {
  taskManager = inject(TaskManagerService);
  private filterProvider = inject(TaskFilterProvider);
  private createModalService = inject(CreateModalService);
  private accordionService = inject(AccordionService);
  private destroyRef = inject(DestroyRef);

  editedTask = signal<Task | null>(null);
  deletedTask = signal<Task | null>(null);
  modalType = signal<"Delete" | "Task">("Task");

  ngOnInit(): void {
    this.createModalService.onClose
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(() => {
        this.handleModalClose();
      });
  }

  private filteredTasks = computed(() => {
    const tasks = this.taskManager.Tasks();
    return this.filterProvider.applyFilters(tasks);
  });

  selectedTasks = signal(new Map<string, Task>());

  // Final view model for the template
  tasksWithSelection = computed((): TaskListItem[] => {
    const filtered = this.filteredTasks();
    return filtered.map((task) => ({
      task: task,
      selected: this.selectedTasks().has(task.id),
    }));
  });

  editTask(task: Task) {
    this.editedTask.set(task);
    this.modalType.set("Task");
    this.createModalService.open();
  }

  openDeleteModal(task: Task) {
    this.deletedTask.set(task);
    this.modalType.set("Delete");
    this.createModalService.open();
  }

  closeModal() {
    this.createModalService.close();
  }

  async deleteConfirmation() {
    if (!this.deletedTask()?.id) {
      throw new Error("Deleting task with id, cannot be done, id is undefined");
    }
    await this.taskManager.deleteTask(this.deletedTask()!.id);
    this.createModalService.close();
  }

  toggleTaskSelection(id: string) {}

  openCreateModal() {
    this.modalType.set("Task");
    this.createModalService.open();
  }

  private handleModalClose() {
    this.editedTask.set(null);
  }

  @HostListener("document:click", ["$event"])
  onDocumentClick(event: MouseEvent) {
    this.accordionService.handleDocumentClick(event);
  }

  ngOnDestroy() {
    this.accordionService.collapse();
  }

  //icons
  protected readonly Plus = Plus;
}
