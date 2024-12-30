import { Component, computed, inject } from "@angular/core";
import { RouterOutlet } from "@angular/router";
import {
  LucideAngularModule,
  ChevronLeft,
  ChevronRight,
  Plus,
  Users,
  CalendarDays,
  Calendar,
} from "lucide-angular";
import { CalendarService } from "@features/calendar/services/calendar-service/calendar.service";
import { CalendarView } from "@features/calendar/types/calendar.types";
import { formatHeader } from "@features/calendar/utils/header.utils";
import { CreateModalComponent } from "@features/calendar/components/create-modal/create-modal.component";
import { CreateModalService } from "@features/calendar/services/create-modal/create-modal.service";

@Component({
  selector: "app-calendar-main",
  imports: [RouterOutlet, LucideAngularModule, CreateModalComponent],
  templateUrl: "./calendar-main.component.html",
  styleUrl: "./calendar-main.component.scss",
  providers: [CalendarService],
})
export class CalendarMainComponent {
  //injection
  private calendarService = inject(CalendarService);
  private modalService = inject(CreateModalService);

  // View management
  readonly currentView = this.calendarService.currentView;
  readonly currentDate = this.calendarService.currentDate;

  readonly headerDisplay = computed(() => {
    return formatHeader(this.currentDate(), this.currentView());
  });

  switchView(view: CalendarView) {
    this.calendarService.switchView(view);
  }

  handleDateChange = (offset: number) => {
    this.calendarService.handleDateChange(offset);
  };

  openModal(): void {
    this.modalService.open("task");
  }

  //icons
  readonly ChevronLeft = ChevronLeft;
  readonly ChevronRight = ChevronRight;
  readonly Plus = Plus;
  readonly Users = Users;
  readonly CalendarDays = CalendarDays;
  readonly CalendarIcon = Calendar;
}
