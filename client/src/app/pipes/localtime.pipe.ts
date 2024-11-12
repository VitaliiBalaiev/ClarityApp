import { Pipe, PipeTransform } from '@angular/core';
import dayjs from 'dayjs';
import utc from 'dayjs/plugin/utc';
import timezone from 'dayjs/plugin/timezone';

dayjs.extend(utc);
dayjs.extend(timezone);

@Pipe({
  standalone: true,
  name: 'localTime'
})
export class LocalTimePipe implements PipeTransform {
  transform(value: string | Date, format: string = 'HH:mm'): string {
    return dayjs.utc(value).local().format(format);
  }
}
