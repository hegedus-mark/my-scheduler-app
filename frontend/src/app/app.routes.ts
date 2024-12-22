import {Routes} from '@angular/router';
import {DESIGN_SYSTEM_ROUTES} from "./design-system/routes";

export const routes: Routes = [
  {
    path: 'design-system',
    children: DESIGN_SYSTEM_ROUTES,
  }
];
