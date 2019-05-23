import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { ActivitiesComponent } from './activities.component'
import { ActivitiesTableComponent } from './activities-table.component'
import { EventChartDirective } from './event-chart'
import { ActivitiesService } from './activities.service';

describe('Activities Component (inline template)', () => {
    let comp: ActivitiesComponent;
    let fixture: ComponentFixture<ActivitiesComponent>;
    let de: DebugElement;
    let el: HTMLElement;

    beforeEach(() => {
        TestBed.configureTestingModule({
            declarations: [ActivitiesComponent],
            providers: [{
                provide: ActivitiesService
            }]
        });

        fixture = TestBed.createComponent(ActivitiesComponent);

        comp = fixture.componentInstance;

        de = fixture.debugElement.query(By.css('#activity-id'));
        el = de.nativeElement;
    })

    it('should display original title', () => {
        fixture.detectChanges();
        expect(el.textContent).toContain(comp.selectedActivity.id);
    });

})
