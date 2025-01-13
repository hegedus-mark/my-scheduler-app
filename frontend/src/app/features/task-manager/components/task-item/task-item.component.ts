import { booleanAttribute, Component, input, output } from "@angular/core";
import { Task } from "@core/task/task.model";
import { PriorityLevel } from "@myschedulerapp/api-client";
import { DatePipe } from "@angular/common";

@Component({
  selector: "app-task-item",
  imports: [DatePipe],
  templateUrl: "./task-item.component.html",
  styleUrl: "./task-item.component.scss",
})
export class TaskItemComponent {
  task = input.required<Task>();
  selected = input(false, { transform: booleanAttribute });
  expanded = input(false, { transform: booleanAttribute });

  select = output<string>();
  expand = output<string>();
  edit = output<Task>();

  getPriorityClass(priority: PriorityLevel): string {
    switch (priority) {
      case "High":
        return "badge bg-red-500 text-white";
      case "Medium":
        return "badge bg-green-500 text-white";
      case "Low":
        return "badge bg-yellow-500 text-white";
    }
  }
}
