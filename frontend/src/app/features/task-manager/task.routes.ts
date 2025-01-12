import { Routes } from "@angular/router";
import { TaskListComponent } from "./components/task-list/task-list.component";

export const TASK_ROUTES: Routes = [
  {
    path: "",
    component: TaskListComponent,
    children: [],
  },
];
