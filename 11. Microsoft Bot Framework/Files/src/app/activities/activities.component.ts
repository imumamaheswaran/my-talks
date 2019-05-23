import { Component, OnInit } from '@angular/core';

import { IActivity } from './models/activity.model';
import { IStage } from './models/stage.model';
import { ActivityStatus } from './models/activity-status.model';
import { IMessageArchive } from './models/message-archive.model';
import { ISearchActivityCriteria } from './models/activity-criteria.model';

import { ActivitiesService } from './activities.service';

declare var moment: any;

@Component({
    selector: 'activities',
    templateUrl: '../activities/activities.component.html'
})
export class ActivitiesComponent implements OnInit {
    public title: any = {
        description: ""
    }
    public activities: IActivity[] = [];

    public frequency: Frequency;

    public criteria: ISearchActivityCriteria;

    public frequencyType = Frequency;

    public interval: any = 10;

    private _selectedStage: IStage;

    public selectedActivity: IActivity;

    public relatedActivities: IActivity[];

    public get selectedStage() {
        return this._selectedStage;
    }

    public set selectedStage(selectedStage: IStage) {
        this._selectedStage = selectedStage;
        this.frequency = Frequency.Day;
        this.relatedActivities = [];

        this.activitiesService
            .getRelatedActivities(this._selectedStage.id)
            .then(activities => {
                this.relatedActivities = activities;
            });

        this.activitiesService
            .getMessageArchives(this._selectedStage.id)
            .then(archives => {
                this.selectedStage.archives = archives;
            });

        this.activitiesService
            .getExceptionInfo(this._selectedStage.id)
            .then(exceptions => {
                this._selectedStage.exceptions = exceptions;

                if (exceptions && exceptions.length > 0) {
                    this._selectedStage.status = ActivityStatus.Error;
                    this.selectedActivity.status = ActivityStatus.Error;
                }
            })

    }

    constructor(private activitiesService: ActivitiesService) {
        this.frequency = Frequency.Minute;
        this.criteria = {
            startTime: new Date(),
            filter: ''
        };
    }

    ngOnInit(): void {
        let date = new Date();
        let criteria: ISearchActivityCriteria = {
            startTime: moment().subtract('minutes', this.interval).toDate(),
            filter: ''
        };
        this.getActivities(criteria);
    }

    search() {
        let date = new Date();
        if (this.frequency == Frequency.Second) {
            date = moment().subtract("seconds", this.interval).toDate();
        }
        else if (this.frequency == Frequency.Minute) {
            date = moment().subtract("minutes", this.interval).toDate();
        }
        else if (this.frequency == Frequency.Hour) {
            date = moment().subtract("hours", this.interval).toDate();
        }
        else if (this.frequency == Frequency.Day) {
            date = moment().subtract("days", this.interval).toDate();
        }
        else {
            date = moment().subtract("minutes", this.interval).toDate();
        }

        let criteria: ISearchActivityCriteria = {
            startTime: date,
            filter: this.criteria.filter
        }
        this.getActivities(criteria);
    }

    getActivities(criteria: ISearchActivityCriteria) {
        this.activitiesService.getActivites(criteria).then(activities => {
            if (activities && activities.length > 0) {
                this.activities = activities.map(activity => {
                    if (activity.stages && activity.stages.length > 0) {
                        var allStages = activity.stages.map(stage => stage.status);

                        if (allStages.filter(item => item === ActivityStatus.Success).length === allStages.length)
                            activity.status = ActivityStatus.Success;
                        else if (allStages.find(item => item === ActivityStatus.Error))
                            activity.status = ActivityStatus.Error;
                        else if (!allStages.find(item => item === ActivityStatus.Error) && allStages.find(item => item === ActivityStatus.InProgress))
                            activity.status = ActivityStatus.InProgress;
                        else
                            activity.status = ActivityStatus.Unknown;
                    }
                    else {
                        activity.status = ActivityStatus.Unknown;
                    }

                    return activity;
                });

                this.selectedActivity = activities[0];
                this.selectedStage = this.selectedActivity.stages[0];
            }
        });
    }
}

enum Frequency {
    Second = 0,
    Minute = 1,
    Hour = 2,
    Day = 3
}