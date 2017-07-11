import {Injectable} from '@angular/core';
import {Title} from '@angular/platform-browser';

@Injectable()
export class PageTitleService {
    private baseTitle:string;

    constructor(private titleService:Title) {

    }

    setBaseTitle(title:string) {
        this.baseTitle = title;
        this.titleService.setTitle(this.baseTitle);
    };

    appendTitle(newTitle:string) {
        this.titleService.setTitle(`${this.baseTitle} / ${newTitle}`);
    };
}