import { TaskItemDto } from "@myschedulerapp/api-client/src";
import {
  BaseTask,
  DraftTask,
  ScheduledTask,
  Task,
  UnscheduledTask,
} from "./task.model";
import { TimeSpan } from "@shared/models/timespan.model";

export class TaskAdapter {
  /**
   * Converts a TaskItemDto to Task
   */
  static toTask(dto: TaskItemDto): Task {
    if (!dto.id) {
      throw new Error("Task id is missing");
    }

    const baseTask: BaseTask = {
      id: dto.id,
      name: dto.name ?? "",
      dueDate: new Date(dto.dueDate),
      duration: new TimeSpan(dto.duration),
      priority: dto.priorityLevel,
    };

    switch (dto.taskItemStatus) {
      case "Draft":
        return {
          ...baseTask,
          status: "Draft",
        } as DraftTask;

      case "Scheduled":
        if (!dto.startDate || !dto.endDate) {
          throw new Error("Scheduled task must have start and end dates");
        }
        return {
          ...baseTask,
          status: "Scheduled",
          scheduledStartDate: new Date(dto.startDate),
          scheduledEndDate: new Date(dto.endDate),
        } as ScheduledTask;

      case "Unscheduled":
        return {
          ...baseTask,
          status: "Unscheduled",
          failureReason: dto.failureReason ?? undefined,
        } as UnscheduledTask;

      default:
        throw new Error(`Unknown task status: ${dto.taskItemStatus}`);
    }
  }

  /**
   * Converts a Task to TaskItemDto
   */
  public static toDto(task: Task): TaskItemDto {
    const dto: TaskItemDto = {
      id: task.id,
      name: task.name,
      dueDate: task.dueDate.toISOString(),
      duration: task.duration,
      priorityLevel: task.priority,
      taskItemStatus: task.status,
      startDate: null,
      endDate: null,
      failureReason: null,
    };

    switch (task.status) {
      case "Scheduled":
        dto.startDate = task.scheduledStartDate.toISOString();
        dto.endDate = task.scheduledEndDate.toISOString();
        break;

      case "Unscheduled":
        if (task.failureReason) {
          dto.failureReason = task.failureReason;
        }
        break;
    }

    return dto;
  }
}
