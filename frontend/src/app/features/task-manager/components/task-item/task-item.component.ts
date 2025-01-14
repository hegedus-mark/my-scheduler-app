import {
  booleanAttribute,
  Component,
  computed,
  ElementRef,
  inject,
  input,
  output,
} from "@angular/core";
import { Task } from "@core/task/task.model";
import { PriorityLevel } from "@myschedulerapp/api-client";
import { DatePipe } from "@angular/common";
import { AccordionService } from "@features/task-manager/services/accordion.service";

@Component({
  selector: "app-task-item",
  imports: [DatePipe],
  templateUrl: "./task-item.component.html",
  styleUrl: "./task-item.component.scss",
})
export class TaskItemComponent {
  accordionService = inject(AccordionService);
  private elementRef = inject(ElementRef);

  task = input.required<Task>();
  selected = input(false, { transform: booleanAttribute });

  select = output<string>();
  expand = output<string>();
  edit = output<Task>();

  expanded = computed(
    () => this.accordionService.ExpandedItemId() === this.task().id,
  );

  toggleSelect(event: Event) {
    event.stopImmediatePropagation();
    this.select.emit(this.task().id);
  }

  toggleExpand() {
    const taskId = this.task().id;
    if (!this.expanded()) {
      this.accordionService.expandItem(taskId, this.elementRef);
    }
  }

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
