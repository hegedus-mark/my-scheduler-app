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

@Component({
  selector: 'app-button',
  imports: [],
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
    inline-flex items-center justify-center font-medium transition-colors
    focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-offset-2
    disabled:pointer-events-none disabled:opacity-50
    ${this.getVariantClasses()}
    ${this.getSizeClasses()}
    ${this.fullWidth() ? 'w-full' : ''}
  `);

  private getVariantClasses(): string {
    const variants = {
      primary: 'bg-blue-600 text-white hover:bg-blue-700 focus-visible:ring-blue-600',
      secondary: 'bg-gray-200 text-gray-900 hover:bg-gray-300 focus-visible:ring-gray-500',
      outline: 'border-2 border-gray-200 hover:bg-gray-100 focus-visible:ring-gray-500',
      danger: 'bg-red-600 text-white hover:bg-red-700 focus-visible:ring-red-600',
      ghost: 'hover:bg-gray-100 focus-visible:ring-gray-500',
    };
    return variants[this.variant()];
  }

  private getSizeClasses(): string {
    const sizes = {
      sm: 'h-8 px-3 text-sm',
      md: 'h-10 px-4',
      lg: 'h-12 px-6 text-lg',
    };
    return sizes[this.size()];
  }

}
