import {Routes} from "@angular/router";

export const DESIGN_SYSTEM_ROUTES: Routes = [
  {
    path: 'components',
    children: [
      {path: 'button',
      loadComponent: () => import('./pages/button-docs/button-docs.component')
        .then(m => m.ButtonDocsComponent),}
    ]
  }
]
