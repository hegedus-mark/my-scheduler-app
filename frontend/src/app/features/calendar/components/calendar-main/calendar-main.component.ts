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
import { CreateModalComponent } from "@shared/components/create-modal/create-modal.component";
import { CreateModalService } from "@shared/components/create-modal/service/create-modal.service";
import { TaskFormComponent } from "@shared/components/create-modal/forms/task-form/task-form.component";
import { EventFormComponent } from "@shared/components/create-modal/forms/event-form/event-form.component";

@Component({
  selector: "app-calendar-main",
  imports: [
    RouterOutlet,
    LucideAngularModule,
    CreateModalComponent,
    TaskFormComponent,
    EventFormComponent,
  ],
  templateUrl: "./calendar-main.component.html",
  styleUrl: "./calendar-main.component.scss",
  providers: [CalendarService],
})
export class CalendarMainComponent {
  //injection
  private calendarService = inject(CalendarService);

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

  //Modal
  modalService = inject(CreateModalService);

  modalType = this.modalService.type;
  isTaskForm = computed(() => this.modalType() === "task");

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
