import { Component } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { EventForm } from "@features/calendar/types/calendar.types";

@Component({
  selector: "app-event-form",
  imports: [FormsModule],
  templateUrl: "./event-form.component.html",
  styleUrl: "./event-form.component.scss",
})
export class EventFormComponent {
  onSubmit() {
    console.log("Event submitted:", this.eventForm);
  }

  eventForm: EventForm = {
    name: "",
    date: new Date(),
    length: 1,
    color: "#3b82f6", // default blue
  };

  readonly colors = [
    { name: "Blue", value: "#3b82f6" },
    { name: "Red", value: "#ef4444" },
    { name: "Green", value: "#22c55e" },
    { name: "Purple", value: "#a855f7" },
    { name: "Yellow", value: "#eab308" },
  ];
}
