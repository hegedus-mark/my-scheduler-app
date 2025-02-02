import { getWeekDays } from "@features/calendar/utils/date.utils";
import { CalendarView } from "@features/calendar/types/calendar.types";

export const formatHeader = (date: Date, view: CalendarView): string => {
  if (view == "month") {
    return date.toLocaleDateString("default", {
      month: "long",
      year: "numeric",
    });
  } else if (view == "week") {
    const week = getWeekDays(date);
    const firstDay = week[0];
    const lastDay = week[week.length - 1];

    const firstMonth = firstDay.toLocaleDateString("default", {
      month: "short",
    });
    const lastMonth = lastDay.toLocaleDateString("default", { month: "short" });
    const firstYear = firstDay.getFullYear();
    const lastYear = lastDay.getFullYear();

    // Different years
    if (firstYear !== lastYear) {
      return `${firstMonth} ${firstYear} - ${lastMonth} ${lastYear}`;
    }

    // Same year, different months
    if (firstMonth !== lastMonth) {
      return `${firstMonth} - ${lastMonth} ${firstYear}`;
    }

    // Same month and year
    return firstDay.toLocaleDateString("default", {
      month: "long",
      year: "numeric",
    });
  }

  return "";
};
