import { Component } from '@angular/core';
import {ApiService} from '../../../shared/services/api/api.service';

@Component({
    templateUrl: './news-list.component.html',
    styles: [require('./news-list.component.css').toString()]
})
export class NewsListComponent  {
    public news: any = [];
    constructor(private apiService: ApiService){
        this.apiService.get('news').then(news => this.news = news);
    }
}
