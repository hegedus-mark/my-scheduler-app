export const TIME_CONSTANTS = {
  MILLISECONDS_IN_SECOND: 1000,
  SECONDS_IN_MINUTE: 60,
  MINUTES_IN_HOUR: 60,
  HOURS_IN_DAY: 24,
  DAYS_IN_WEEK: 7,
  MONTHS_IN_YEAR: 12,
} as const;

export const DERIVED_CONSTANTS = {
  MILLISECONDS_IN_MINUTE:
    TIME_CONSTANTS.MILLISECONDS_IN_SECOND * TIME_CONSTANTS.SECONDS_IN_MINUTE,
  MILLISECONDS_IN_HOUR:
    TIME_CONSTANTS.MILLISECONDS_IN_SECOND *
    TIME_CONSTANTS.SECONDS_IN_MINUTE *
    TIME_CONSTANTS.MINUTES_IN_HOUR,
  MILLISECONDS_IN_DAY:
    TIME_CONSTANTS.MILLISECONDS_IN_SECOND *
    TIME_CONSTANTS.SECONDS_IN_MINUTE *
    TIME_CONSTANTS.MINUTES_IN_HOUR *
    TIME_CONSTANTS.HOURS_IN_DAY,
  MILLISECONDS_IN_WEEK:
    TIME_CONSTANTS.MILLISECONDS_IN_SECOND *
    TIME_CONSTANTS.SECONDS_IN_MINUTE *
    TIME_CONSTANTS.MINUTES_IN_HOUR *
    TIME_CONSTANTS.HOURS_IN_DAY *
    TIME_CONSTANTS.DAYS_IN_WEEK,
} as const;

interface TimeComponents {
  days?: number;
  hours?: number;
  minutes?: number;
  seconds?: number;
  milliseconds?: number;
}

export class TimeSpan {
  private _milliseconds = 0;
  private _totalMilliseconds = 0;

  constructor(timeComponents: TimeComponents | number = {}) {
    if (typeof timeComponents === "number") {
      this.setTotalMilliseconds(timeComponents);
    } else {
      this.set(timeComponents);
    }
  }

  // Static factory methods
  static fromDates(date1: Date, date2: Date): TimeSpan {
    if (date1 > date2) {
      throw new Error(`${date1} can't be later than ${date2}`);
    }
    return new TimeSpan(date1.getTime() - date2.getTime());
  }

  static fromSeconds(seconds: number): TimeSpan {
    return new TimeSpan(seconds * TIME_CONSTANTS.MILLISECONDS_IN_SECOND);
  }

  static fromMinutes(minutes: number): TimeSpan {
    return new TimeSpan(minutes * DERIVED_CONSTANTS.MILLISECONDS_IN_MINUTE);
  }

  static fromHours(hours: number): TimeSpan {
    return new TimeSpan(hours * DERIVED_CONSTANTS.MILLISECONDS_IN_HOUR);
  }

  static fromDays(days: number): TimeSpan {
    return new TimeSpan(days * DERIVED_CONSTANTS.MILLISECONDS_IN_DAY);
  }

  static fromWeeks(weeks: number): TimeSpan {
    return new TimeSpan(weeks * DERIVED_CONSTANTS.MILLISECONDS_IN_WEEK);
  }

  // Instance methods
  set({
    days = 0,
    hours = 0,
    minutes = 0,
    seconds = 0,
    milliseconds = 0,
  }: TimeComponents): void {
    this.setTotalMilliseconds(
      days * DERIVED_CONSTANTS.MILLISECONDS_IN_DAY +
        hours * DERIVED_CONSTANTS.MILLISECONDS_IN_HOUR +
        minutes * DERIVED_CONSTANTS.MILLISECONDS_IN_MINUTE +
        seconds * TIME_CONSTANTS.MILLISECONDS_IN_SECOND +
        milliseconds,
    );
  }

  private setTotalMilliseconds(milliseconds: number): void {
    if (!Number.isFinite(milliseconds)) {
      throw new Error("Invalid milliseconds value");
    }
    this._totalMilliseconds = milliseconds;
    this._milliseconds = milliseconds;
    this.normalize();
  }

  private normalize(): void {
    const absMs = Math.abs(this._totalMilliseconds);
    const sign = this._totalMilliseconds < 0 ? -1 : 1;

    this._milliseconds = sign * (absMs % TIME_CONSTANTS.MILLISECONDS_IN_SECOND);
  }

  // Arithmetic operations
  add(timespan: TimeSpan): TimeSpan {
    return new TimeSpan(this._totalMilliseconds + timespan.totalMilliseconds);
  }

  subtract(timespan: TimeSpan): TimeSpan {
    return new TimeSpan(this._totalMilliseconds - timespan.totalMilliseconds);
  }

  multiply(factor: number): TimeSpan {
    return new TimeSpan(this._totalMilliseconds * factor);
  }

  // Date operations
  addToDate(date: Date): Date {
    return new Date(date.getTime() + this._totalMilliseconds);
  }

  subtractFromDate(date: Date): Date {
    return new Date(date.getTime() - this._totalMilliseconds);
  }

  // Getters for components
  get totalMilliseconds(): number {
    return this._totalMilliseconds;
  }

  get totalSeconds(): number {
    return Math.floor(
      this._totalMilliseconds / TIME_CONSTANTS.MILLISECONDS_IN_SECOND,
    );
  }

  get totalMinutes(): number {
    return Math.floor(
      this._totalMilliseconds / DERIVED_CONSTANTS.MILLISECONDS_IN_MINUTE,
    );
  }

  get totalHours(): number {
    return Math.floor(
      this._totalMilliseconds / DERIVED_CONSTANTS.MILLISECONDS_IN_HOUR,
    );
  }

  get totalDays(): number {
    return Math.floor(
      this._totalMilliseconds / DERIVED_CONSTANTS.MILLISECONDS_IN_DAY,
    );
  }

  get days(): number {
    return Math.floor(this.totalDays);
  }

  get hours(): number {
    return Math.floor(
      (this._totalMilliseconds % DERIVED_CONSTANTS.MILLISECONDS_IN_DAY) /
        DERIVED_CONSTANTS.MILLISECONDS_IN_HOUR,
    );
  }

  get minutes(): number {
    return Math.floor(
      (this._totalMilliseconds % DERIVED_CONSTANTS.MILLISECONDS_IN_HOUR) /
        DERIVED_CONSTANTS.MILLISECONDS_IN_MINUTE,
    );
  }

  get seconds(): number {
    return Math.floor(
      (this._totalMilliseconds % DERIVED_CONSTANTS.MILLISECONDS_IN_MINUTE) /
        TIME_CONSTANTS.MILLISECONDS_IN_SECOND,
    );
  }

  get milliseconds(): number {
    return this._milliseconds;
  }

  // Comparison methods
  equals(other: TimeSpan): boolean {
    return this._totalMilliseconds === other.totalMilliseconds;
  }

  compareTo(other: TimeSpan): number {
    return this._totalMilliseconds - other.totalMilliseconds;
  }

  // Utility methods
  toString(): string {
    const parts: string[] = [];
    if (this.days) parts.push(`${this.days}d`);
    if (this.hours) parts.push(`${this.hours}h`);
    if (this.minutes) parts.push(`${this.minutes}m`);
    if (this.seconds) parts.push(`${this.seconds}s`);
    if (this.milliseconds) parts.push(`${this.milliseconds}ms`);
    return parts.length ? parts.join(" ") : "0ms";
  }

  toJSON(): TimeComponents {
    return {
      days: this.days,
      hours: this.hours,
      minutes: this.minutes,
      seconds: this.seconds,
      milliseconds: this.milliseconds,
    };
  }
}
