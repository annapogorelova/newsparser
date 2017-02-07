import {Component, HostListener} from '@angular/core';
import {ApiService} from '../../../shared/services/api/api.service';
import {PagerService} from '../../../shared/services/pager/pager.service';

@Component({
    templateUrl: './news-list.component.html',
    styles: [require('./news-list.component.css').toString()]
})
export class NewsListComponent  {
    public hasMoreItems: boolean = true;
    public loadingInProgress: boolean = false;

    constructor(private apiService: ApiService, private pager: PagerService){
        this.loadingInProgress = true;
        var requestParams = { startIndex: this.pager.getNextPageStartIndex(), numResults: this.pager.getPageSize() };
        this.apiService.get('news', requestParams).then(news => this.handleLoadedNews(news));
    }

    handleLoadedNews = (data: any) => {
        if(!data.length){
            this.hasMoreItems = false;
        }
        this.pager.appendItems(data);
        this.loadingInProgress = false;
    };

    loadMore = () => {
        if(this.loadingInProgress){
            return;
        }
        this.loadingInProgress = true;
        var requestParams = { startIndex: this.pager.getNextPageStartIndex(), numResults: this.pager.getPageSize() };
        this.apiService.get('news', requestParams).then(news => this.handleLoadedNews(news));
    };

    hasItems = () => {
        return this.pager.getItems().length;
    };

    @HostListener("window:scroll", [])
    onWindowScroll = () => {
        // TODO: refactor
        if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight){
            this.loadMore();
        }
    };
}
