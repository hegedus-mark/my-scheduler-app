import { TestBed } from "@angular/core/testing";

import { TaskFilterProvider } from "./task-filter-provider.service";

describe("TaskFilterService", () => {
  let service: TaskFilterProvider;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TaskFilterProvider);
  });

  it("should be created", () => {
    expect(service).toBeTruthy();
  });
});
