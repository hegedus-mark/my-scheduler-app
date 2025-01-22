import { Component, computed, effect, input, signal } from "@angular/core";
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from "@angular/forms";
import { DatePipe } from "@angular/common";
import { MonthCalendarCell } from "@shared/models/month-calendar.model";
import { generateMonthViewGrid } from "@shared/utils/month-calendar.utils";

@Component({
  selector: "app-date-picker",
  imports: [DatePipe],
  templateUrl: "./date-picker.component.html",
  styleUrl: "./date-picker.component.scss",
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: DatePickerComponent,
      multi: true,
    },
  ],
})
export class DatePickerComponent implements ControlValueAccessor {
  placeholder = input("Select Date");
  minDate = input<null | Date>(null);
  maxDate = input<null | Date>(null);

  showCalendar = signal<boolean>(false);
  selectedDate = signal<Date | null>(null);
  currentMonth = signal<Date>(new Date());
  calendarDays = signal<MonthCalendarCell[]>([]);
  touched = signal<boolean>(false);
  disabled = signal<boolean>(false);

  constructor() {
    effect(() => {
      this.generateCalendar();
    });
  }

  private onChange: (value: Date) => void = () => {
    return;
  };

  private onTouched: () => void = () => {
    return;
  };

  formattedSelectedDate = computed(() => {
    if (this.selectedDate() === null) {
      return "";
    } else {
      return this.formatDate(this.selectedDate()!);
    }
  });

  // ControlValueAccessor implementation
  writeValue(value: Date): void {
    this.selectedDate.set(value);
  }

  registerOnChange(fn: (value: Date) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  selectDate(value: Date) {
    this.markAsTouched();
    this.selectedDate.set(value);
    this.onChange(value);
  }

  markAsTouched() {
    if (!this.touched()) {
      this.onTouched();
      this.touched.set(true);
    }
  }

  setDisabledState(isDisabled: boolean) {
    this.disabled.set(isDisabled);
  }

  formatDate(date?: Date): string {
    if (!date) {
      throw new Error("date can't be undefined");
    }
    return date.toLocaleDateString();
  }

  // UI handlers
  toggleCalendar(): void {
    if (!this.disabled()) {
      this.showCalendar.update((value) => !value);
    }
  }

  previousMonth(): void {
    this.currentMonth.update((current) => {
      const newDate = new Date(current);
      newDate.setMonth(current.getMonth() - 1);
      return newDate;
    });
  }

  nextMonth(): void {
    this.currentMonth.update((current) => {
      const newDate = new Date(current);
      newDate.setMonth(current.getMonth() + 1);
      return newDate;
    });
  }

  generateCalendar(): void {
    this.calendarDays.set(generateMonthViewGrid(this.currentMonth()));
  }

  isSelected(date: Date): boolean {
    return this.selectedDate()?.toDateString() === date.toDateString();
  }

  isDisabled(date: Date): boolean {
    if (this.disabled()) return true;
    if (this.minDate() !== null && date < this.minDate()!) return true;
    if (this.maxDate() !== null && date > this.maxDate()!) return true;
    return false;
  }
}
