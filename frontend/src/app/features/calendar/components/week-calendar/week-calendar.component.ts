import { Component, computed, inject } from "@angular/core";
import { DatePipe } from "@angular/common";
import { LucideAngularModule, Plus } from "lucide-angular";
import { CalendarService } from "@features/calendar/services/calendar.service";
import { getWeekDays } from "@features/calendar/utils/week-days.utils";
import {
  calculateTopPosition,
  generateTimeSlots,
  getCurrentTimePosition,
} from "@features/calendar/utils/time.utils";
import { generateWeekGrid } from "@features/calendar/utils/week-grid.utils";

@Component({
  selector: "app-week-calendar-calendar",
  imports: [DatePipe, LucideAngularModule],
  templateUrl: "./week-calendar.component.html",
  styleUrl: "./week-calendar.component.scss",
})
export class WeekCalendarComponent {
  //DI
  private readonly calendarService = inject(CalendarService);

  readonly currentDate = this.calendarService.currentDate;
  readonly currentWeekDays = computed(() => getWeekDays(this.currentDate()));

  readonly timeSlots = computed(() => generateTimeSlots());

  readonly weekGrid = computed(() =>
    generateWeekGrid(this.currentWeekDays(), this.timeSlots()),
  );

  readonly currentTimePosition = computed(() => getCurrentTimePosition());

  getTopPosition = calculateTopPosition;
  readonly Plus = Plus;
}
