import { Route } from '@angular/router';

//extending the base route offered by angular to avoid
//lot of custom code for handling routes as we do in BizTalk360
export interface ApplicationRoute extends Route {
    name: string,
    isMain?: boolean,
    children?: ApplicationRoute[]
}

export declare type ApplicationRouterConfig = ApplicationRoute[];