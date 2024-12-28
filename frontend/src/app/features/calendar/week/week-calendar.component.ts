import {
  Component,
  computed,
  DestroyRef,
  inject,
  OnInit,
  signal,
} from "@angular/core";
import { CalendarEvent } from "@features/calendar/week/week-calendar.types";
import { DatePipe } from "@angular/common";
import { LucideAngularModule, Plus } from "lucide-angular";
import { ActivatedRoute } from "@angular/router";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";

@Component({
  selector: "app-week-calendar",
  imports: [DatePipe, LucideAngularModule],
  templateUrl: "./week-calendar.component.html",
  styleUrl: "./week-calendar.component.scss",
})
export class WeekCalendarComponent implements OnInit {
  //DI
  private route = inject(ActivatedRoute);
  private destroyRef = inject(DestroyRef);

  //constants
  private readonly HOURS_IN_DAY = 24;
  private readonly DAYS_IN_WEEK = 7;
  readonly HOUR_HEIGHT = 60; // pixels per hour

  // Time-related signals
  readonly currentDate = signal(new Date());
  readonly currentWeekDays = computed(() =>
    this.getWeekDays(this.currentDate()),
  );
  readonly timeSlots = computed(() => {
    return Array.from({ length: this.HOURS_IN_DAY }, (_, i) => {
      const hour = i;
      return {
        hour,
        display: this.formatHour(hour),
      };
    });
  });

  ngOnInit() {
    this.route.queryParams
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((params) => {
        if (params["date"]) {
          this.currentDate.set(new Date(params["date"]));
        }
      });
  }

  readonly weekGrid = computed(() => {
    const days = this.currentWeekDays();
    const hours = this.timeSlots();

    return days.map((day) => ({
      date: day,
      isToday: this.isToday(day),
      slots: hours.map((hour) => ({
        hour: hour.hour,
        date: day,
        events: [], // To be populated with events later
      })),
    }));
  });

  // Helper methods
  private getWeekDays(date: Date): Date[] {
    const current = new Date(date);
    const week = [];
    // Start with Sunday or Monday based on preference
    current.setDate(current.getDate() - current.getDay());

    for (let i = 0; i < this.DAYS_IN_WEEK; i++) {
      week.push(new Date(current));
      current.setDate(current.getDate() + 1);
    }

    return week;
  }

  private formatHour(hour: number): string {
    if (hour === 0) return "12 AM";
    if (hour === 12) return "12 PM";
    return hour > 12 ? `${hour - 12} PM` : `${hour} AM`;
  }

  private isToday(date: Date): boolean {
    const today = new Date();
    return (
      date.getDate() === today.getDate() &&
      date.getMonth() === today.getMonth() &&
      date.getFullYear() === today.getFullYear()
    );
  }

  // Events signal
  readonly events = signal<CalendarEvent[]>([]);

  // Event handling methods
  getEventsForSlot(date: Date, hour: number): CalendarEvent[] {
    return this.events().filter((event) => {
      const slotStart = new Date(new Date(date).setHours(hour, 0, 0, 0));
      const slotEnd = new Date(new Date(date).setHours(hour + 1, 0, 0, 0));
      return event.startTime < slotEnd && event.endTime > slotStart;
    });
  }

  // Time calculation helpers
  getTopPosition(hour: number): number {
    return hour * this.HOUR_HEIGHT;
  }

  calculateEventPosition(startTime: Date, endTime: Date) {
    const startHour = startTime.getHours() + startTime.getMinutes() / 60;
    const endHour = endTime.getHours() + endTime.getMinutes() / 60;

    return {
      top: this.getTopPosition(startHour),
      height: (endHour - startHour) * this.HOUR_HEIGHT,
    };
  }

  // Current time indicator
  readonly currentTimePosition = computed(() => {
    const now = new Date();
    const hour = now.getHours();
    const minutes = now.getMinutes();
    return this.getTopPosition(hour + minutes / 60);
  });

  // Event interaction methods
  handleEventClick(event: CalendarEvent): void {
    console.log("Event clicked:", event);
  }

  handleAddEvent(date: Date, hour: number): void {
    // Handle adding new event at specific time slot
    const startTime = new Date(new Date(date).setHours(hour, 0, 0, 0));
    const endTime = new Date(new Date(date).setHours(hour + 1, 0, 0, 0));
    console.log("Add event at:", { startTime, endTime });
  }

  readonly Plus = Plus;
}
