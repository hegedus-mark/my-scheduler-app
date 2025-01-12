export interface TaskForm {
  name: string;
  deadline: Date;
  estimatedHours: number;
  priority: "low" | "medium" | "high";
}

export interface EventForm {
  name: string;
  date: Date;
  length: number; // in hours
  color: string;
}

export type ModalType = "task" | "event";
