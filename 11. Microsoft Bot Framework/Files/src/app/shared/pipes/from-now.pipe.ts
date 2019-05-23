import { Pipe, PipeTransform } from '@angular/core';
declare var moment: any;

@Pipe({
    name: 'fromNow'
})
export class FromNowPipe implements PipeTransform {
    transform = (value: Date) => moment(value.toString()).fromNow();
}