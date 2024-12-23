import { Component } from "@angular/core";
import {
  ChevronLeft,
  ChevronRight,
  LucideAngularModule,
  Plus,
  Users,
} from "lucide-angular";
import { DatePipe } from "@angular/common";

interface CalendarEvent {
  id: number;
  title: string;
  date: Date;
  type: "task" | "meeting";
  status: "pending" | "completed";
}

@Component({
  selector: "app-calendar",
  imports: [LucideAngularModule, DatePipe],
  templateUrl: "./calendar.component.html",
  styleUrl: "./calendar.component.scss",
})
export class CalendarComponent {
  readonly ChevronLeft = ChevronLeft;
  readonly ChevronRight = ChevronRight;
  readonly Plus = Plus;

  currentMonth = "December";
  currentYear = 2024;

  daysOfWeek = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];

  // Mock data - In real app, this would come from a service
  calendarDays = Array.from({ length: 35 }, (_, i) => ({
    date: i + 1,
    isCurrentMonth: i < 31,
    hasEvents: Math.random() > 0.7,
    events: [
      {
        id: i,
        title: `Event ${i}`,
        type: Math.random() > 0.5 ? "task" : "meeting",
      },
    ].filter(() => Math.random() > 0.7),
  }));

  upcomingEvents: CalendarEvent[] = [
    {
      id: 1,
      title: "Team Meeting",
      date: new Date(),
      type: "meeting",
      status: "pending",
    },
    {
      id: 2,
      title: "Project Review",
      date: new Date(Date.now() + 86400000),
      type: "task",
      status: "completed",
    },
  ];

  getEventClass(event: CalendarEvent): string {
    return event.type === "task" ? "event-task" : "event-meeting";
  }

  getEventTypeClass(event: CalendarEvent): string {
    return event.type === "task" ? "text-blue-500" : "text-purple-500";
  }

  getEventStatusClass(event: CalendarEvent): string {
    return event.status === "pending" ? "status-pending" : "status-completed";
  }

  protected readonly Users = Users;
}
