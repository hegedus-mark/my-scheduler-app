import { Component, computed, inject, signal } from "@angular/core";
import { CalendarEvent } from "@features/calendar/components/week-calendar/week-calendar.types";
import { DatePipe } from "@angular/common";
import { LucideAngularModule, Plus } from "lucide-angular";
import { CalendarService } from "@features/calendar/services/calendar.service";
import {
  DAYS_IN_WEEK,
  HOUR_HEIGHT,
  HOURS_IN_DAY,
} from "@features/calendar/constants/calendar.constants";

@Component({
  selector: "app-week-calendar-calendar",
  imports: [DatePipe, LucideAngularModule],
  templateUrl: "./week-calendar.component.html",
  styleUrl: "./week-calendar.component.scss",
})
export class WeekCalendarComponent {
  //DI
  private readonly calendarService = inject(CalendarService);

  // Time-related signals
  readonly currentDate = this.calendarService.currentDate;
  readonly currentWeekDays = computed(() =>
    this.getWeekDays(this.currentDate()),
  );
  readonly timeSlots = computed(() => {
    return Array.from({ length: HOURS_IN_DAY }, (_, i) => {
      const hour = i;
      return {
        hour,
        display: this.formatHour(hour),
      };
    });
  });

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

    for (let i = 0; i < DAYS_IN_WEEK; i++) {
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
    return hour * HOUR_HEIGHT;
  }

  calculateEventPosition(startTime: Date, endTime: Date) {
    const startHour = startTime.getHours() + startTime.getMinutes() / 60;
    const endHour = endTime.getHours() + endTime.getMinutes() / 60;

    return {
      top: this.getTopPosition(startHour),
      height: (endHour - startHour) * HOUR_HEIGHT,
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
