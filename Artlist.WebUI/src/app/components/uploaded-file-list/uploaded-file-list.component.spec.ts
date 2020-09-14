import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadedFileListComponent } from './uploaded-file-list.component';

describe('UploadedFileListComponent', () => {
  let component: UploadedFileListComponent;
  let fixture: ComponentFixture<UploadedFileListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UploadedFileListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UploadedFileListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
