import { Component, computed, inject } from "@angular/core";
import { LucideAngularModule, Plus } from "lucide-angular";
import { DAYS_OF_WEEK } from "@features/calendar/constants/calendar.constants";
import { CalendarService } from "@features/calendar/services/calendar-service/calendar.service";
import { generateMonthViewGrid } from "@features/calendar/utils/month-grid.utils";
import { CreateModalService } from "@features/calendar/services/create-modal/create-modal.service";

@Component({
  selector: "app-month-calendar-calendar",
  imports: [LucideAngularModule],
  templateUrl: "./month-calendar.component.html",
  styleUrl: "./month-calendar.component.scss",
})
export class MonthCalendarComponent {
  private readonly calendarService = inject(CalendarService);
  private readonly modalService = inject(CreateModalService);

  readonly currentDate = this.calendarService.currentDate;
  readonly currentDaysInMonth = computed(() =>
    generateMonthViewGrid(this.currentDate()),
  );

  openModal(): void {
    this.modalService.open("event");
  }

  readonly Plus = Plus;
  readonly DAYS_OF_WEEK = DAYS_OF_WEEK;
}
