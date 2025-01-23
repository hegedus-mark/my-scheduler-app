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

  static fromString(timeString: string): TimeSpan {
    // Regular expressions for both formats
    const fullFormatRegex = /^(\d+)\.(\d{2}):(\d{2}):(\d{2})$/; // dd.hh:mm:ss
    const shortFormatRegex = /^(\d{2}):(\d{2}):(\d{2})$/; // hh:mm:ss

    let days = 0,
      hours = 0,
      minutes = 0,
      seconds = 0;

    if (fullFormatRegex.test(timeString)) {
      // Parse dd.hh:mm:ss format
      const [, d, h, m, s] = timeString.match(fullFormatRegex)!;
      days = parseInt(d, 10);
      hours = parseInt(h, 10);
      minutes = parseInt(m, 10);
      seconds = parseInt(s, 10);
    } else if (shortFormatRegex.test(timeString)) {
      // Parse hh:mm:ss format
      const [, h, m, s] = timeString.match(shortFormatRegex)!;
      hours = parseInt(h, 10);
      minutes = parseInt(m, 10);
      seconds = parseInt(s, 10);
    } else {
      throw new Error(
        'Invalid time format. Use either "hh:mm:ss" or "dd.hh:mm:ss"',
      );
    }

    // Validate ranges
    if (hours >= 24) throw new Error("Hours must be less than 24");
    if (minutes >= 60) throw new Error("Minutes must be less than 60");
    if (seconds >= 60) throw new Error("Seconds must be less than 60");

    return new TimeSpan({ days, hours, minutes, seconds });
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
    if (this.days > 0) {
      return `${this.days}.${String(this.hours).padStart(2, "0")}:${String(this.minutes).padStart(2, "0")}:${String(this.seconds).padStart(2, "0")}`;
    }
    return `${String(this.hours).padStart(2, "0")}:${String(this.minutes).padStart(2, "0")}:${String(this.seconds).padStart(2, "0")}`;
  }

  toHourMinuteString(): string {
    return `${String(this.hours + 24 * this.days).padStart(2, "0")}:${String(this.minutes).padStart(2, "0")}`;
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
