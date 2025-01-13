import { Task } from "@core/task/task.model";
import { PriorityLevel, TaskItemStatus } from "@myschedulerapp/api-client";
import { TimeSpan } from "@shared/models/timespan.model";

export interface TaskFilters {
  dueDates?: Date[];
  durations?: TimeSpan[];
  priorities?: PriorityLevel[];
  statuses?: TaskItemStatus[];
}

export interface TaskListItem {
  task: Task;
  selected: boolean;
  expanded: boolean;
}
