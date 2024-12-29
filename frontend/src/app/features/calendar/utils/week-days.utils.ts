import { DAYS_IN_WEEK } from "@features/calendar/constants/calendar.constants";

export function getWeekDays(date: Date): Date[] {
  const current = new Date(date);
  const week = [];
  current.setDate(current.getDate() - current.getDay());

  for (let i = 0; i < DAYS_IN_WEEK; i++) {
    week.push(new Date(current));
    current.setDate(current.getDate() + 1);
  }

  return week;
}
