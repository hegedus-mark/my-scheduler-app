import { ElementRef, Injectable, signal } from "@angular/core";

@Injectable({
  providedIn: "root",
})
export class AccordionService {
  private expandedItemId = signal<string | null>(null);
  private activeElement = signal<ElementRef | null>(null);
  ExpandedItemId = this.expandedItemId.asReadonly();

  expandItem(id: string, element: ElementRef) {
    this.activeElement.set(element);
    this.expandedItemId.set(id);
  }

  collapse() {
    this.activeElement.set(null);
    this.expandedItemId.set(null);
  }

  handleDocumentClick(event: MouseEvent) {
    if (
      this.activeElement() &&
      !this.activeElement()?.nativeElement.contains(event.target)
    ) {
      this.collapse();
    }
  }
}
