import {Component, HostListener} from '@angular/core';
import {ApiService} from '../../../shared/services/api/api.service';

@Component({
    templateUrl: './news-list.component.html',
    styles: [require('./news-list.component.css').toString()]
})
export class NewsListComponent  {
    public news: any = [];
    public hasMoreItems: boolean = true;
    public loadingInProgress: boolean = false;

    private params: any = { startIndex: 0, numResults: 10};

    constructor(private apiService: ApiService){
        this.loadingInProgress = true;
        this.apiService.get('news', this.params).then(news => this.handleLoadedNews(news));
    }

    handleLoadedNews = (data: any) => {
        if(!data.length){
            this.hasMoreItems = false;
        }
        this.news = this.news.concat(data);
        this.params.startIndex = this.news.length;
        this.loadingInProgress = false;
    };

    loadMore = () => {
        if(this.loadingInProgress){
            return;
        }
        this.loadingInProgress = true;
        this.apiService.get('news', this.params).then(news => this.handleLoadedNews(news));
    };

    @HostListener("window:scroll", [])
    onWindowScroll = () => {
        // TODO: refactor
        if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight){
            this.loadMore();
        }
    };
}
