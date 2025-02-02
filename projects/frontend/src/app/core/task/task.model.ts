import { TimeSpan } from "@shared/models/timespan.model";
import { PriorityLevel } from "@myschedulerapp/api-client/src";

export interface BaseTask {
  id: string;
  name: string;
  description?: string;
  dueDate: Date;
  duration: TimeSpan;
  priority: PriorityLevel;
}

export interface DraftTask extends BaseTask {
  status: "Draft";
}

export interface ScheduledTask extends BaseTask {
  status: "Scheduled";
  scheduledStartDate: Date;
  scheduledEndDate: Date;
}

export interface UnscheduledTask extends BaseTask {
  status: "Unscheduled";
  failureReason?: string;
}

export type Task = DraftTask | ScheduledTask | UnscheduledTask;
