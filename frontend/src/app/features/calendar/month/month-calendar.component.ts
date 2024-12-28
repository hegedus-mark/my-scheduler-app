import { Component, computed, OnInit, signal } from "@angular/core";
import {
  ChevronLeft,
  ChevronRight,
  LucideAngularModule,
  Plus,
  Users,
} from "lucide-angular";
import { MonthCalendarCell } from "@features/calendar/month/month-calendar.types";
import { ActivatedRoute, Router } from "@angular/router";

@Component({
  selector: "app-month-calendar",
  imports: [LucideAngularModule],
  templateUrl: "./month-calendar.component.html",
  styleUrl: "./month-calendar.component.scss",
})
export class MonthCalendarComponent implements OnInit {
  readonly ChevronLeft = ChevronLeft;
  readonly ChevronRight = ChevronRight;
  readonly Plus = Plus;
  readonly Users = Users;
  readonly DaysOfWeek = [
    "Sun",
    "Mon",
    "Tue",
    "Wed",
    "Thu",
    "Fri",
    "Sat",
  ] as const;
  private readonly CELLS = 42; // 6 * 7

  readonly currentDate = signal(new Date());
  readonly currentDaysInMonth = computed(() =>
    this.getDaysInMonth(this.currentDate()),
  );

  constructor(
    private route: ActivatedRoute,
    private router: Router,
  ) {}

  ngOnInit() {
    // Subscribe to query params to get the date
    this.route.queryParams.subscribe((params) => {
      if (params["date"]) {
        this.currentDate.set(new Date(params["date"]));
      }
    });
  }

  private getDaysInMonth = (date: Date) => {
    const year = date.getFullYear();
    const month = date.getMonth();
    const firstDay = new Date(year, month, 1);
    const lastDay = new Date(year, month + 1, 0);
    const startingDay = firstDay.getDay();
    const daysInMonth = lastDay.getDate();

    const result = new Array<MonthCalendarCell>(this.CELLS);

    //prev month
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

    //current month
    for (let i = 0; i < daysInMonth; i++) {
      result[startingDay + i] = {
        currentMonth: true,
        date: new Date(year, month, i + 1),
      };
    }

    //next month
    const nextMonth = new Date(year, month + 1);
    let nextMonthDay = 1;
    for (let i = startingDay + daysInMonth; i < this.CELLS; i++) {
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

  // Update navigation methods to use routing
  handleMonthChange = (offset: number) => {
    const newDate = new Date(this.currentDate());
    newDate.setMonth(newDate.getMonth() + offset);

    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { date: newDate.toISOString() },
      queryParamsHandling: "merge",
    });
  };
}
