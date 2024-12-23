import { Component } from "@angular/core";
import { ThemeService } from "@core/services/theme.service";
import { AsyncPipe } from "@angular/common";
import { THEMES_CONFIG } from "@core/config/themes.config";

@Component({
  selector: "app-theme-switcher",
  imports: [AsyncPipe],
  templateUrl: "./theme-switcher.component.html",
  styleUrl: "./theme-switcher.component.scss",
})
export class ThemeSwitcherComponent {
  currentTheme$ = this.themeService.currentTheme$;

  constructor(private themeService: ThemeService) {}

  setTheme(themeId: string): void {
    this.themeService.setTheme(themeId);
  }

  protected readonly THEMES_CONFIG = THEMES_CONFIG;
}
