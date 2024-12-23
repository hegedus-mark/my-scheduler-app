import { Route } from "@angular/router";

export interface NavItem {
  name: string;
  path: string;
  icon?: string;
  route: Route;
}

export const navigationConfig: NavItem[] = [
  {
    name: "Dashboard",
    path: "/dashboard",
    icon: "dashboard-icon",
    route: {
      path: "dashboard",
      loadChildren: () =>
        import("@features/dashboard/dashboard.routes").then(
          (m) => m.DASHBOARD_ROUTES,
        ),
    },
  },
  {
    name: "Calendar",
    path: "/calendar",
    icon: "calendar-icon",
    route: {
      path: "calendar",
      loadChildren: () =>
        import("@features/calendar/calendar.routes").then(
          (m) => m.CALENDAR_ROUTES,
        ),
    },
  },
];
