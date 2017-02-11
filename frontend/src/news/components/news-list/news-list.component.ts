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
    public selectedSourceId: number = null;

    constructor(private apiService: ApiService, private pager: PagerService){
        this.loadingInProgress = true;
        var requestParams = { pageIndex: this.pager.getNextPageStartIndex(), pageSize: this.pager.getPageSize() };
        this.apiService.get('news', requestParams).then(news => this.handleLoadedNews(news));
    }

    handleLoadedNews = (data: any) => {
        if(!data.length){
            this.hasMoreItems = false;
        }
        this.pager.appendItems(data);
        this.loadingInProgress = false;
    };

    getRequestParams = () => {
        return {
            pageIndex: this.pager.getNextPageStartIndex(),
            pageSize: this.pager.getPageSize(),
            sourceId: this.selectedSourceId
        };
    };

    reload = () => {
        this.pager.reset();
        this.loadingInProgress = true;
        this.apiService.get('news', this.getRequestParams()).then(news => this.handleLoadedNews(news));
    };

    loadMore = () => {
        if(this.loadingInProgress){
            return;
        }
        this.loadingInProgress = true;
        this.apiService.get('news', this.getRequestParams()).then(news => this.handleLoadedNews(news));
    };

    selectSource = (sourceId: number) => {
        this.selectedSourceId = sourceId;
        this.reload();
    };

    hasItems = () => {
        return this.pager.getItems().length;
    };

    @HostListener("window:scroll", [])
    onWindowScroll = () => {
        var userScrollPosition = window.innerHeight + window.scrollY;
        if (userScrollPosition >= document.body.offsetHeight && this.hasMoreItems){
            this.loadMore();
        }
    };
}
