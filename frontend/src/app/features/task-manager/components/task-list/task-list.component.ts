import { Component, inject } from "@angular/core";
import { Calendar, Clock, LucideAngularModule, Plus } from "lucide-angular";
import { TaskManagerService } from "@core/task/task-manager.service";

@Component({
  selector: "app-task-list",
  imports: [LucideAngularModule],
  templateUrl: "./task-list.component.html",
  styleUrl: "./task-list.component.scss",
})
export class TaskListComponent {
  protected readonly Plus = Plus;

  private taskMangerService: TaskManagerService = inject(TaskManagerService);

  public tasks = this.taskMangerService.Tasks;

  protected readonly Clock = Clock;
  protected readonly Calendar = Calendar;
}
