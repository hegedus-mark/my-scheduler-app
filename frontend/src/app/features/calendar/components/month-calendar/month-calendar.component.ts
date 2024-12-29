import { Component, computed, inject } from "@angular/core";
import { LucideAngularModule, Plus } from "lucide-angular";
import { DAYS_OF_WEEK } from "@features/calendar/constants/calendar.constants";
import { CalendarService } from "@features/calendar/services/calendar.service";
import { generateMonthViewGrid } from "@features/calendar/utils/month-grid.utils";

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
    generateMonthViewGrid(this.currentDate()),
  );

  readonly Plus = Plus;
  readonly DAYS_OF_WEEK = DAYS_OF_WEEK;
}
