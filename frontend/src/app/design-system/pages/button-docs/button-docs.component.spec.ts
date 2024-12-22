import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ButtonDocsComponent } from './button-docs.component';

describe('ButtonDocsComponent', () => {
  let component: ButtonDocsComponent;
  let fixture: ComponentFixture<ButtonDocsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ButtonDocsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ButtonDocsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
