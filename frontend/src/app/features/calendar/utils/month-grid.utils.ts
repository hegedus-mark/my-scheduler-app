import { DAY_CELLS_IN_MONTH } from "@features/calendar/constants/calendar.constants";
import { MonthCalendarCell } from "@features/calendar/types/calendar.types";

function getPreviousMonthDays(
  year: number,
  month: number,
  startingDay: number,
): MonthCalendarCell[] {
  const prevMonth = new Date(year, month - 1);
  const daysInPrevMonth = new Date(year, month, 0).getDate();
  const result = new Array<MonthCalendarCell>(startingDay);

  for (let i = 0; i < startingDay; i++) {
    result[i] = {
      currentMonth: false,
      date: new Date(
        prevMonth.getFullYear(),
        prevMonth.getMonth(),
        daysInPrevMonth - (startingDay - i - 1),
      ),
    };
  }

  return result;
}

function getCurrentMonthDays(
  year: number,
  month: number,
  daysInMonth: number,
): MonthCalendarCell[] {
  const result = new Array<MonthCalendarCell>(daysInMonth);

  for (let i = 0; i < daysInMonth; i++) {
    result[i] = {
      currentMonth: true,
      date: new Date(year, month, i + 1),
    };
  }

  return result;
}

function getNextMonthDays(
  year: number,
  month: number,
  remainingCells: number,
): MonthCalendarCell[] {
  const nextMonth = new Date(year, month + 1);
  const result = new Array<MonthCalendarCell>(remainingCells);

  for (let i = 0; i < remainingCells; i++) {
    result[i] = {
      currentMonth: false,
      date: new Date(nextMonth.getFullYear(), nextMonth.getMonth(), i + 1),
    };
  }

  return result;
}

function getMonthInfo(date: Date) {
  const year = date.getFullYear();
  const month = date.getMonth();
  const firstDay = new Date(year, month, 1);
  const lastDay = new Date(year, month + 1, 0);

  return {
    year,
    month,
    startingDay: firstDay.getDay(),
    daysInMonth: lastDay.getDate(),
  };
}

export function generateMonthViewGrid(date: Date): MonthCalendarCell[] {
  const { year, month, startingDay, daysInMonth } = getMonthInfo(date);

  const previousMonthDays = getPreviousMonthDays(year, month, startingDay);
  const currentMonthDays = getCurrentMonthDays(year, month, daysInMonth);
  const remainingCells = DAY_CELLS_IN_MONTH - (startingDay + daysInMonth);
  const nextMonthDays = getNextMonthDays(year, month, remainingCells);

  return [...previousMonthDays, ...currentMonthDays, ...nextMonthDays];
}
