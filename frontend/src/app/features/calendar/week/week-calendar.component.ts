import { Component, computed, signal } from "@angular/core";
import { CalendarEvent } from "@features/calendar/week/week-calendar.types";
import { DatePipe } from "@angular/common";
import {
  ChevronLast,
  ChevronLeft,
  ChevronRight,
  LucideAngularModule,
  Plus,
  User,
  Users,
} from "lucide-angular";

@Component({
  selector: "app-week-calendar",
  imports: [DatePipe, LucideAngularModule],
  templateUrl: "./week-calendar.component.html",
  styleUrl: "./week-calendar.component.scss",
})
export class WeekCalendarComponent {
  private readonly HOURS_IN_DAY = 24;
  private readonly DAYS_IN_WEEK = 7;
  readonly HOUR_HEIGHT = 60; // pixels per hour

  // Time-related signals
  readonly currentDate = signal(new Date());
  readonly currentWeekDays = computed(() =>
    this.getWeekDays(this.currentDate()),
  );

  // Display computed values
  readonly weekDisplayRange = computed(() => {
    const days = this.currentWeekDays();
    const firstDay = days[0];
    const lastDay = days[days.length - 1];
    const sameMonth = firstDay.getMonth() === lastDay.getMonth();
    const sameYear = firstDay.getFullYear() === lastDay.getFullYear();

    if (sameMonth && sameYear) {
      return `${firstDay.toLocaleDateString("default", { month: "long" })} ${this.formatDateRange(firstDay, lastDay)}, ${firstDay.getFullYear()}`;
    } else if (sameYear) {
      return `${firstDay.toLocaleDateString("default", { month: "short" })} ${firstDay.getDate()} - ${lastDay.toLocaleDateString("default", { month: "short" })} ${lastDay.getDate()}, ${firstDay.getFullYear()}`;
    } else {
      return `${firstDay.toLocaleDateString("default", { month: "short" })} ${firstDay.getDate()}, ${firstDay.getFullYear()} - ${lastDay.toLocaleDateString("default", { month: "short" })} ${lastDay.getDate()}, ${lastDay.getFullYear()}`;
    }
  });

  readonly timeSlots = computed(() => {
    return Array.from({ length: this.HOURS_IN_DAY }, (_, i) => {
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

  // Events signal
  readonly events = signal<CalendarEvent[]>([]);

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

  private formatDateRange(start: Date, end: Date): string {
    return `${start.getDate()}-${end.getDate()}`;
  }

  private isToday(date: Date): boolean {
    const today = new Date();
    return (
      date.getDate() === today.getDate() &&
      date.getMonth() === today.getMonth() &&
      date.getFullYear() === today.getFullYear()
    );
  }

  // Event handling methods
  getEventsForSlot(date: Date, hour: number): CalendarEvent[] {
    return this.events().filter((event) => {
      const slotStart = new Date(new Date(date).setHours(hour, 0, 0, 0));
      const slotEnd = new Date(new Date(date).setHours(hour + 1, 0, 0, 0));
      return event.startTime < slotEnd && event.endTime > slotStart;
    });
  }

  // Navigation methods
  handleWeekChange = (offset: number) => {
    this.currentDate.update((date) => {
      const newDate = new Date(date);
      newDate.setDate(newDate.getDate() + offset * 7);
      return newDate;
    });
  };

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

  protected readonly ChevronRight = ChevronRight;
  protected readonly ChevronLast = ChevronLast;
  protected readonly ChevronLeft = ChevronLeft;
  protected readonly Plus = Plus;
  protected readonly User = User;
  protected readonly Users = Users;
}
