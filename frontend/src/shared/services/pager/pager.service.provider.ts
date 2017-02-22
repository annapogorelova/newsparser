import {Injectable} from '@angular/core';
import {PagerService} from './pager.service';

@Injectable()
export class PagerServiceProvider {
    getInstance = (pageNumber?:number, pageSize?:number):PagerService => {
        return new PagerService(pageNumber, pageSize);
    };
}