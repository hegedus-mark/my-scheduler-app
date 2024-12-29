import { DestroyRef, inject, Injectable, signal } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { CalendarView } from "@features/calendar/types/calendar.types";

@Injectable()
export class CalendarService {
  private readonly destroyRef = inject(DestroyRef);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute, {
    skipSelf: true,
  });

  readonly currentView = signal<CalendarView>("month");
  readonly currentDate = signal(new Date());

  constructor() {
    this.setupSubscription();
  }

  switchView(view: CalendarView) {
    this.router.navigate([`/calendar/${view}`], {
      queryParams: { date: this.currentDate().toISOString() },
      queryParamsHandling: "merge",
    });
    this.currentView.set(view);
  }

  navigateToDate(date: Date) {
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { date: date.toISOString() },
      queryParamsHandling: "merge",
    });
  }

  handleDateChange(offset: number) {
    const newDate = new Date(this.currentDate());
    if (this.currentView() === "month") {
      newDate.setMonth(newDate.getMonth() + offset);
    } else {
      newDate.setDate(newDate.getDate() + offset * 7);
    }
    this.currentDate.set(newDate);
    this.navigateToDate(newDate);
  }

  private setupSubscription() {
    this.route.firstChild?.url
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((url) => {
        const view = url[0]?.path as CalendarView;
        if (view) {
          this.currentView.set(view);
        }
      });

    this.route.queryParams
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((params) => {
        if (params["date"]) {
          this.currentDate.set(new Date(params["date"]));
        }
      });
  }
}
