export type CalendarView = "month" | "week";

export interface MonthCalendarCell {
  currentMonth: boolean;
  date: Date;
}

export interface TimeSlot {
  hour: number;
  display: string;
}

export interface DayColumn {
  date: Date;
  isToday: boolean;
  slots: WeekCalendarCell[];
}

export interface WeekCalendarCell {
  hour: number;
  date: Date;
  events: CalendarEvent[];
}

export interface CalendarEvent {
  id: string;
  title: string;
  description?: string;
  startTime: Date;
  endTime: Date;
  color?: string;
}

export interface WeekGridDay {
  date: Date;
  isToday: boolean;
  slots: WeekCalendarCell[];
}
