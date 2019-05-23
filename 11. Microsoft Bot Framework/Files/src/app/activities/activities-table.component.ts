import { Component, ViewChild, OnInit, OnChanges, Output, EventEmitter, Input, SimpleChanges, AfterViewInit } from '@angular/core';

import { IActivity } from './models/activity.model';
import { IStage } from './models/stage.model';
import { ActivityStatus } from './models/activity-status.model';
import { IMessageArchive } from './models/message-archive.model';

import { ActivitiesService } from './activities.service';
import { NgSwitch } from '@angular/common';
import { GridDataResult, DataStateChangeEvent, PageChangeEvent, GridComponent } from '@progress/kendo-angular-grid';

import { Observable } from 'rxjs/Rx';
//import { State } from '@progress/kendo-angular-grid';

@Component({
    selector: 'activities-table',
    templateUrl: '../activities/activities-table.component.html'
})
export class ActivitiesTableComponent implements OnInit, OnChanges, AfterViewInit {
    @Input()
    public activities: IActivity[];

    @Input()
    selectedStage: IStage;

    @Output()
    private selectedStageChange = new EventEmitter<IStage>();

    @Input()
    selectedActivity: IActivity;

    @Output()
    private selectedActivityChange = new EventEmitter<IActivity>();

    @ViewChild(GridComponent)
    grid: GridComponent;

    public gridDataResult: GridDataResult;
    public pageSize: number = 15;
    public skip: number = 0;

    constructor(private activitiesService: ActivitiesService) {

    }

    ngOnInit(): void {
        console.log(this.activities);
    }

    ngOnChanges(changes: SimpleChanges): void {
        if (changes.activities && changes.activities.currentValue && changes.activities.currentValue.length > 0) {
            this.activities = changes.activities.currentValue as IActivity[];
            this.loadItems();
        }
    }

    ngAfterViewInit(): void {

    }

    private loadItems(): void {
        if (this.activities.length <= 0)
            return;

        this.gridDataResult = {
            data: this.activities.slice(this.skip, this.skip + this.pageSize),
            total: this.activities.length
        }
    }

    protected pageChange(event: PageChangeEvent): void {
        this.skip = event.skip;
        this.loadItems();
    }

    protected setSelectedStage(activity: IActivity, stage: IStage) {
        this.selectedActivityChange.emit(activity);
        this.selectedStageChange.emit(stage);
    }
}