import {booleanAttribute, Component, EventEmitter, Input, Output} from '@angular/core';
import {ButtonProps, ButtonSize, ButtonVariant} from './button.types';

@Component({
  selector: 'app-button',
  imports: [],
  templateUrl: './button.component.html',
  styleUrl: './button.component.scss'
})
export class ButtonComponent implements ButtonProps{

  @Input() variant: ButtonVariant = 'primary';
  @Input() size: ButtonSize = 'md';
  @Input({transform: booleanAttribute}) disabled = false;
  @Input({transform: booleanAttribute}) loading = false;
  @Input() fullWidth = false;
  @Input() icon?: string;
  @Input() type: 'button' | 'submit' | 'reset' = 'button';

  @Output() clicked = new EventEmitter<void>();

  get classes(): string {
    return `
      inline-flex items-center justify-center font-medium transition-colors
      focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-offset-2
      disabled:pointer-events-none disabled:opacity-50
      ${this.getVariantClasses()}
      ${this.getSizeClasses()}
      ${this.fullWidth ? 'w-full' : ''}
    `;
  }

  private getVariantClasses(): string {
    const variants = {
      primary: 'bg-blue-600 text-white hover:bg-blue-700 focus-visible:ring-blue-600',
      secondary: 'bg-gray-200 text-gray-900 hover:bg-gray-300 focus-visible:ring-gray-500',
      outline: 'border-2 border-gray-200 hover:bg-gray-100 focus-visible:ring-gray-500',
      danger: 'bg-red-600 text-white hover:bg-red-700 focus-visible:ring-red-600',
      ghost: 'hover:bg-gray-100 focus-visible:ring-gray-500',
    };
    return variants[this.variant];
  }

  private getSizeClasses(): string {
    const sizes = {
      sm: 'h-8 px-3 text-sm',
      md: 'h-10 px-4',
      lg: 'h-12 px-6 text-lg',
    };
    return sizes[this.size];
  }

}
