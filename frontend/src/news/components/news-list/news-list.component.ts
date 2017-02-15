import {Component, HostListener, Inject} from '@angular/core';
import {ApiService} from '../../../shared/services/api/api.service';
import {PagerService} from '../../../shared/services/pager/pager.service';
import {NavigatorService} from '../../../shared/services/navigator/navigator.service';
import {ActivatedRoute} from '@angular/router';

@Component({
    templateUrl: './news-list.component.html',
    styles: [require('./news-list.component.css').toString()]
})
export class NewsListComponent  {
    public hasMoreItems: boolean = true;
    public loadingInProgress: boolean = false;
    public selectedSourceId: number = null;

    constructor(private apiService: ApiService, private pager: PagerService,
                private navigator: NavigatorService, @Inject(ActivatedRoute) private route:ActivatedRoute,){
    }

    loadNews = (params: any) => {
        this.loadingInProgress = true;
        this.apiService.get('news', params).then(news => this.handleLoadedNews(news));
    };

    ngOnInit(){
        window.scrollTo(0, 0);
        this.initializeRequestParams();

        this.loadNews({
            pageIndex: 0,
            pageSize: this.pager.getNumberOfItemsToPreload(),
            sourceId: this.selectedSourceId
        });
    }

    initializeRequestParams = () => {
        this.route.queryParams
            .map((queryParams) => queryParams['page'])
            .subscribe((page: string) =>
                parseInt(page) ? this.pager.setPage(parseInt(page)) : this.pager.setPage(1));

        this.route.queryParams
            .map((queryParams) => queryParams['source'])
            .subscribe((sourceId: string) =>
                this.selectedSourceId = sourceId ? parseInt(sourceId) : null);
    };

    handleLoadedNews = (data: any) => {
        if(!data.length){
            this.hasMoreItems = false;
        }
        this.pager.appendItems(data);
        this.navigator.setQueryParam('page', this.pager.getPageNumber());
        if(this.selectedSourceId != null){
            this.navigator.setQueryParam('source', this.selectedSourceId);
        }
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
        this.loadNews(this.getRequestParams());
    };

    loadMore = () => {
        if(this.loadingInProgress){
            return;
        }
        this.loadNews(this.getRequestParams());
    };

    selectSource = (source: any) => {
        this.selectedSourceId = source.id;
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
