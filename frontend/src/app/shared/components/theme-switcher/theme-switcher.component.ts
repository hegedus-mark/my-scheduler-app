import {Component, OnInit} from '@angular/core';
import {ThemeService} from "@core/services/theme.service";
import {AsyncPipe} from "@angular/common";
import {THEMES_CONFIG} from "@core/config/themes.config";

@Component({
  selector: 'app-theme-switcher',
  imports: [
    AsyncPipe
  ],
  templateUrl: './theme-switcher.component.html',
  styleUrl: './theme-switcher.component.scss'
})
export class ThemeSwitcherComponent implements OnInit {
  currentTheme$ = this.themeService.currentTheme$;

  constructor(private themeService: ThemeService) {}

  ngOnInit(): void {}

  setTheme(themeId: string): void {
    this.themeService.setTheme(themeId);
  }

  protected readonly THEMES_CONFIG = THEMES_CONFIG;
}
