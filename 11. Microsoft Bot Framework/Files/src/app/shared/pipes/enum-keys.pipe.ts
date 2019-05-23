import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
    name: 'enumKeys'
})
export class EnumKeys implements PipeTransform {
    transform(value: any, ...args: any[]) {
        let keys = [];
        for (var enumMember in value) {
            if (!isNaN(parseInt(enumMember, 10))) {
                keys.push({ key: enumMember, value: value[enumMember] })
            }
        }
        return keys;
    }
}