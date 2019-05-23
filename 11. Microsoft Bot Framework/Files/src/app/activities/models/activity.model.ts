import { IStage } from './stage.model';
import { ActivityStatus } from './activity-status.model';

export interface IActivity {
    activityId: string;
    environmentId: string;
    startTime: Date;
    interfaceName: string;
    messageType: string;
    messageId: string;
    batchId: string;
    senderId: string;
    receiverId: string;
    trackedData: string;
    status: ActivityStatus;
    stages: IStage[];
}