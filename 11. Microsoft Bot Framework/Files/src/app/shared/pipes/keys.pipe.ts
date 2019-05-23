import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
    name: 'keys',
    pure: false
})
export class KeysPipe implements PipeTransform {
    transform(value: any, ...args: any[]) {
        return Object.keys(value)
    }

}