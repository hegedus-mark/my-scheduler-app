import { Routes } from "@angular/router";
import { CalendarMainComponent } from "@features/calendar/calendar-main/calendar-main.component";

export const CALENDAR_ROUTES: Routes = [
  {
    path: "",
    component: CalendarMainComponent,
    children: [
      {
        path: "",
        redirectTo: "week",
        pathMatch: "full",
      },
      {
        path: "month",
        loadComponent: () =>
          import("./month/month-calendar.component").then(
            (m) => m.MonthCalendarComponent,
          ),
      },
      {
        path: "week",
        loadComponent: () =>
          import("./week/week-calendar.component").then(
            (m) => m.WeekCalendarComponent,
          ),
      },
    ],
  },
];
