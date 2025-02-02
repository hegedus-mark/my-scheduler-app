import { Component } from "@angular/core";
import { RouterOutlet } from "@angular/router";
import { SidebarComponent } from "@core/layout/sidebar/sidebar.component";
import { navigationConfig } from "@core/config/navigation.config";

@Component({
  selector: "app-main-layout",
  imports: [RouterOutlet, SidebarComponent],
  templateUrl: "./main-layout.component.html",
  styleUrl: "./main-layout.component.scss",
})
export class MainLayoutComponent {
  protected readonly navigationConfig = navigationConfig;
}
