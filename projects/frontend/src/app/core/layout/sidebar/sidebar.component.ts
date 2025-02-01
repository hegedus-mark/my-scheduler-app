import { Component, input } from "@angular/core";
import { RouterLink, RouterLinkActive } from "@angular/router";
import { NgOptimizedImage } from "@angular/common";
import { NavItem } from "@core/config/navigation.config";

@Component({
  selector: "app-sidebar",
  imports: [RouterLink, RouterLinkActive, NgOptimizedImage],
  templateUrl: "./sidebar.component.html",
  styleUrl: "./sidebar.component.scss",
})
export class SidebarComponent {
  navigationItems = input<NavItem[]>([]);
}
