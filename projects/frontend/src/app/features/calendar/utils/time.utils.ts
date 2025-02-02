import { HOUR_HEIGHT, HOURS_IN_DAY } from "../constants/calendar.constants";
import { TimeSlot } from "@features/calendar/types/calendar.types";

export function formatHour(hour: number): string {
  if (hour === 0) return "12 AM";
  if (hour === 12) return "12 PM";
  return hour > 12 ? `${hour - 12} PM` : `${hour} AM`;
}

export function generateTimeSlots(): TimeSlot[] {
  return Array.from({ length: HOURS_IN_DAY }, (_, i) => ({
    hour: i,
    display: formatHour(i),
  }));
}

export function calculateTopPosition(hour: number): number {
  return hour * HOUR_HEIGHT;
}

export function getCurrentTimePosition(): number {
  const now = new Date();
  const hour = now.getHours();
  const minutes = now.getMinutes();
  return calculateTopPosition(hour + minutes / 60);
}
