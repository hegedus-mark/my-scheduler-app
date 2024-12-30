import { TestBed } from "@angular/core/testing";

import { CreateModalService } from "./create-modal.service";

describe("CreateModalService", () => {
  let service: CreateModalService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CreateModalService);
  });

  it("should be created", () => {
    expect(service).toBeTruthy();
  });
});
