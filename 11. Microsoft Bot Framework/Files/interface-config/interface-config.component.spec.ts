import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InterfaceConfigComponent } from './interface-config.component';

describe('InterfaceConfigComponent', () => {
  let component: InterfaceConfigComponent;
  let fixture: ComponentFixture<InterfaceConfigComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InterfaceConfigComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InterfaceConfigComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
