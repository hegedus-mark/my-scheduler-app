import { Task } from "@core/task/task.model";
import { PriorityLevel, TaskItemStatus } from "@myschedulerapp/api-client";

export interface TaskFilters {
  priorities: PriorityLevel[];
  statuses: TaskItemStatus[];
  searchQuery: string;
}

export interface TaskListItem {
  task: Task;
  selected: boolean;
}
