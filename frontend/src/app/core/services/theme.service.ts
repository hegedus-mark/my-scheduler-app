import { Injectable } from '@angular/core';
import {BehaviorSubject} from "rxjs";
import {THEMES_CONFIG} from "../config/themes.config";

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  private readonly THEME_KEY = 'selected-theme';

  private currentThemeSubject = new BehaviorSubject<string>(this.getStoredTheme());
  currentTheme$ = this.currentThemeSubject.asObservable();

  constructor() {
    this.applyTheme(this.getStoredTheme());
  }

  private getStoredTheme(): string{
    return localStorage.getItem(this.THEME_KEY) || 'light';
  }

  setTheme(themeId:string){
    if (THEMES_CONFIG.some(theme => theme.id === themeId)) {
      localStorage.setItem(this.THEME_KEY, themeId);
      this.applyTheme(themeId);
      this.currentThemeSubject.next(themeId);
    }
  }

  private applyTheme(themeId: string): void {
    document.documentElement.setAttribute('data-theme', themeId);
  }
}
