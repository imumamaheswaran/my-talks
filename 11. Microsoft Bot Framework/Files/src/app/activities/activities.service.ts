import { Injectable } from '@angular/core';
import { IActivity } from './models/activity.model';
import { IStage } from './models/stage.model';
import { ActivityStatus } from './models/activity-status.model';
import { IMessageArchive } from './models/message-archive.model';
import { ISearchActivityCriteria } from './models/activity-criteria.model';
import { IExceptionInfo } from './models/activity-exception-info.model';
import { Headers, Http } from '@angular/http';

import 'rxjs/add/operator/toPromise';

@Injectable()
export class ActivitiesService {


    private url = "";
    constructor(private http: Http) {

    }

    getActivites(criteria: ISearchActivityCriteria): Promise<IActivity[]> {
        if (!criteria.filter) {
            criteria.filter = "[environmentId]: \"\"";
        }
        return this.http.get(`http://localhost:53054/api/activities?startTime=${criteria.startTime.toISOString()}&filter=${criteria.filter}`)
            .toPromise()
            .then(response => response.json().data as IActivity[])
            .catch(this.handleError);
    }

    getStageActivites(mainActivityId: string): Promise<IStage[]> {

        return this.http.get(`http://localhost:53054/api/activities/${mainActivityId}/stages`)
            .toPromise()
            .then(response => response.json().data as IStage[])
            .catch(this.handleError);
    }

    getRelatedActivities(stageActivityId: string): Promise<IActivity[]> {
        return this.http.get(`http://localhost:53054/api/activities/${stageActivityId}/related`)
            .toPromise().then(response => response.json().data as IActivity[])
            .catch(this.handleError)
    }

    getMessageArchives(stageActivityId: string): Promise<IMessageArchive[]> {
        return this.http.get(`http://localhost:53054/api/activities/archives/${stageActivityId}`)
            .toPromise().then(response => response.json() as IMessageArchive[])
            .catch(this.handleError)
    }

    getExceptionInfo(stageActivityId: string): Promise<IExceptionInfo[]> {
        return this.http.get(`http://localhost:53054/api/activities/exceptions?stageActivityId=${stageActivityId}`)
            .toPromise().then(response => response.json() as IExceptionInfo[])
            .catch(this.handleError)
    }

    handleError(error: any): Promise<any> {
        console.error("An error occurred", error);
        return Promise.reject(error.message || error);
    }
}