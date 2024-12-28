import {
  Component,
  computed,
  DestroyRef,
  inject,
  OnInit,
  signal,
} from "@angular/core";
import { ActivatedRoute, Router, RouterOutlet } from "@angular/router";
import {
  LucideAngularModule,
  ChevronLeft,
  ChevronRight,
  Plus,
  Users,
  CalendarDays,
  Calendar,
} from "lucide-angular";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";

type CalendarView = "month" | "week";

@Component({
  selector: "app-calendar-main",
  imports: [RouterOutlet, LucideAngularModule],
  templateUrl: "./calendar-main.component.html",
  styleUrl: "./calendar-main.component.scss",
})
export class CalendarMainComponent implements OnInit {
  //injection
  private destroyRef = inject(DestroyRef);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  // View management
  readonly currentView = signal<CalendarView>("month");
  readonly currentDate = signal(new Date());

  readonly monthYearDisplay = computed(() => {
    return this.currentDate().toLocaleDateString("default", {
      month: "long",
      year: "numeric",
    });
  });

  ngOnInit() {
    console.log("Initial URL:", this.router.url); // Debug log
    console.log("Route snapshot:", this.route.snapshot); // Debug log

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

  switchView(view: CalendarView) {
    this.router.navigate([`/calendar/${view}`], {
      queryParams: { date: this.currentDate().toISOString() },
      queryParamsHandling: "merge",
    });
    this.currentView.set(view);
  }

  handleDateChange = (offset: number) => {
    this.currentDate.update((date) => {
      const newDate = new Date(date);
      if (this.currentView() === "month") {
        newDate.setMonth(date.getMonth() + offset);
      } else {
        newDate.setDate(date.getDate() + offset * 7);
      }
      return newDate;
    });
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { date: this.currentDate().toISOString() },
      queryParamsHandling: "merge",
    });
  };

  //icons
  readonly ChevronLeft = ChevronLeft;
  readonly ChevronRight = ChevronRight;
  readonly Plus = Plus;
  readonly Users = Users;
  readonly CalendarDays = CalendarDays;
  readonly CalendarIcon = Calendar;
}
