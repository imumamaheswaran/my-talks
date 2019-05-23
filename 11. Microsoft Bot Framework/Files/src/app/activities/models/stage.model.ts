import { ActivityStatus } from './activity-status.model';
import { IMessageArchive } from './message-archive.model';
import { IExceptionInfo } from './activity-exception-info.model';
export interface IStage {
    id: string;
    activityId: string;
    currentStage: string;
    previousStage: string;
    senderId: string;
    receiverId: string;
    messageType: string;
    startTime: Date;
    endTime: Date;
    archiveId: string;
    status: ActivityStatus;
    trackedData: string;
    archives: IMessageArchive[];
    exceptions: IExceptionInfo[];
}