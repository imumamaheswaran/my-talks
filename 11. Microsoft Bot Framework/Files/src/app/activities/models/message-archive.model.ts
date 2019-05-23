export interface IMessageArchive {
    archiveId: string;
    messageBody: string;
    messageContext: string,
    messageDirection: boolean;
    archiveTime: Date;
}

