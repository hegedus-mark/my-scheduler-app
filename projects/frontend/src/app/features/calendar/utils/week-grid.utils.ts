import { TimeSlot, WeekGridDay } from "@features/calendar/types/calendar.types";
import { isToday } from "@features/calendar/utils/date.utils";

export function generateWeekGrid(
  days: Date[],
  hours: TimeSlot[],
): WeekGridDay[] {
  return days.map((day) => ({
    date: day,
    isToday: isToday(day),
    slots: hours.map((hour) => ({
      hour: hour.hour,
      date: day,
      events: [],
    })),
  }));
}
