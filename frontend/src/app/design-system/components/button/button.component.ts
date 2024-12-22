import {
  booleanAttribute,
  ChangeDetectionStrategy,
  Component,
  computed,
  EventEmitter,
  input,
  output
} from '@angular/core';
import {ButtonSize, ButtonType, ButtonVariant} from './button.types';
import {SpinnerComponent} from "../spinner/spinner.component";

@Component({
  selector: 'app-button',
  imports: [
    SpinnerComponent
  ],
  templateUrl: './button.component.html',
  styleUrl: './button.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ButtonComponent {

  variant = input<ButtonVariant>('primary');
  size = input<ButtonSize>('md');
  disabled = input(false, { transform: booleanAttribute });
  loading = input(false, { transform: booleanAttribute });
  fullWidth = input(false);
  icon = input<string | undefined>(undefined);
  type = input<ButtonType>('button');

  // Convert Output to signal
  clicked = output<void>();

  // Convert classes to a computed signal that depends on other signals
  classes = computed(() => `
    btn
    ${this.getVariantClasses()}
    ${this.getSizeClasses()}
    ${this.fullWidth() ? 'btn-block' : ''}
  `);

  private getVariantClasses(): string {
    const variants = {
      primary: 'btn-primary',
      secondary: 'btn-secondary',
      outline: 'btn-outline',
      danger: 'btn-error',
      ghost: 'btn-ghost',
    };
    return variants[this.variant()];
  }

  private getSizeClasses(): string {
    const sizes = {
      sm: 'btn-sm',
      md: 'btn-md',
      lg: 'btn-lg',
    };
    return sizes[this.size()];
  }

}
