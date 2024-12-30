import { Component } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { TaskForm } from "@features/calendar/types/calendar.types";
import { TitleCasePipe } from "@angular/common";

@Component({
  selector: "app-task-form",
  imports: [FormsModule, TitleCasePipe],
  templateUrl: "./task-form.component.html",
  styleUrl: "./task-form.component.scss",
})
export class TaskFormComponent {
  readonly priorities = ["low", "medium", "high"];

  onSubmit() {
    console.log("Task submitted:", this.taskForm);
  }

  taskForm: TaskForm = {
    name: "",
    deadline: new Date(),
    estimatedHours: 1,
    priority: "medium",
  };
}
