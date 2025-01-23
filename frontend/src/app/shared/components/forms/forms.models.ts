import { PriorityLevel } from "@myschedulerapp/api-client";
import { TimeSpan } from "@shared/models/timespan.model";

export interface TaskForm {
  name: string;
  dueDate: Date;
  duration: TimeSpan;
  priority: PriorityLevel;
}

export interface EventForm {
  name: string;
  date: Date;
  length: number; // in hours
  color: string;
}
