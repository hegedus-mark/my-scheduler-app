import {DashboardComponent} from "@features/dashboard/dashboard.component";
import {Routes} from "@angular/router";

export const DASHBOARD_ROUTES: Routes = [
  {
    path: '',
    component: DashboardComponent,
    children: [
      // Any child routes for dashboard
    ]
  }
];
