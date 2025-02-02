import { Routes } from "@angular/router";
import { MainLayoutComponent } from "@core/layout/main-layout/main-layout.component";
import { navigationConfig } from "@core/config/navigation.config";

export const APP_ROUTES: Routes = [
  {
    path: "",
    component: MainLayoutComponent,
    children: [...navigationConfig.map((nav) => nav.route)],
  },
];
