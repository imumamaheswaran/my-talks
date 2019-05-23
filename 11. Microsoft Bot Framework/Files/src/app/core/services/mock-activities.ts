import { Injectable } from '@angular/core';
import 'rxjs/add/operator/toPromise';

@Injectable()
export class MockService {
    getActivies(): Promise<any> {
        var activities = [{
            name: "activity1"
        }]
        return Promise.resolve(activities);
    }
}