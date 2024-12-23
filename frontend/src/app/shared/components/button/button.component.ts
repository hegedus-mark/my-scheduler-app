import {
  booleanAttribute,
  ChangeDetectionStrategy,
  Component,
  input,
  output,
} from "@angular/core";

@Component({
  selector: "app-button",
  imports: [],
  templateUrl: "./button.component.html",
  styleUrl: "./button.component.scss",
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ButtonComponent {
  disabled = input(false, { transform: booleanAttribute });
  loading = input(false, { transform: booleanAttribute });
  type = input<"button" | "submit" | "reset">("button");
  clicked = output<void>();
}
