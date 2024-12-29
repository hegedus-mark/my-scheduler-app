import { Component, computed, inject } from "@angular/core";
import { LucideAngularModule, Plus } from "lucide-angular";
import { MonthCalendarCell } from "@features/calendar/components/month-calendar/month-calendar.types";
import {
  DAY_CELLS_IN_MONTH,
  DAYS_OF_WEEK,
} from "@features/calendar/constants/calendar.constants";
import { CalendarService } from "@features/calendar/services/calendar.service";

@Component({
  selector: "app-month-calendar-calendar",
  imports: [LucideAngularModule],
  templateUrl: "./month-calendar.component.html",
  styleUrl: "./month-calendar.component.scss",
})
export class MonthCalendarComponent {
  private readonly calendarService = inject(CalendarService);

  readonly currentDate = this.calendarService.currentDate;
  readonly currentDaysInMonth = computed(() =>
    this.getDaysInMonth(this.currentDate()),
  );

  private getDaysInMonth = (date: Date) => {
    const year = date.getFullYear();
    const month = date.getMonth();
    const firstDay = new Date(year, month, 1);
    const lastDay = new Date(year, month + 1, 0);
    const startingDay = firstDay.getDay();
    const daysInMonth = lastDay.getDate();

    const result = new Array<MonthCalendarCell>(DAY_CELLS_IN_MONTH);

    //prev month-calendar
    const prevMonth = new Date(year, month - 1);
    const daysInPrevMonth = new Date(year, month, 0).getDate();
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

    //current month-calendar
    for (let i = 0; i < daysInMonth; i++) {
      result[startingDay + i] = {
        currentMonth: true,
        date: new Date(year, month, i + 1),
      };
    }

    //next month-calendar
    const nextMonth = new Date(year, month + 1);
    let nextMonthDay = 1;
    for (let i = startingDay + daysInMonth; i < DAY_CELLS_IN_MONTH; i++) {
      result[i] = {
        currentMonth: false,
        date: new Date(
          nextMonth.getFullYear(),
          nextMonth.getMonth(),
          nextMonthDay++,
        ),
      };
    }

    return result;
  };

  readonly Plus = Plus;
  readonly DAYS_OF_WEEK = DAYS_OF_WEEK;
}
