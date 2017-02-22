import {Component, HostListener, Inject} from '@angular/core';
import {ApiService} from '../../../shared/services/api/api.service';
import {PagerService} from '../../../shared/services/pager/pager.service';
import {NavigatorService} from '../../../shared/services/navigator/navigator.service';
import {ActivatedRoute} from '@angular/router';
import {PagerServiceProvider} from '../../../shared/services/pager/pager.service.provider';

@Component({
    selector: 'news-list',
    templateUrl: './news-list.component.html',
    styles: [require('./news-list.component.css').toString()]
})

/**
 * Component represents a list of news
 */
export class NewsListComponent  {
    public hasMoreItems: boolean = true;
    public loadingInProgress: boolean = false;
    public refreshInProgress: boolean = false;
    public selectedSourceId: number = null;
    public pager: PagerService = null;

    constructor(private apiService: ApiService, private pagerProvider: PagerServiceProvider,
                private navigator: NavigatorService, @Inject(ActivatedRoute) private route:ActivatedRoute){
        this.pager = this.pagerProvider.getInstance();
    }

    /**
     * Calls the api GET method to get the list of news
     * @param params
     */
    loadNews = (params: any) => {
        this.loadingInProgress = true;
        this.apiService.get('news', params).then(news => this.handleLoadedNews(news));
    };

    ngOnInit(){
        window.scrollTo(0, 0);
        this.initializeRequestParams();

        var itemsToPreload = this.pager.getNumberOfItemsToPreload();
        if(itemsToPreload){
            this.loadNews({
                pageIndex: 0,
                pageSize: itemsToPreload,
                sourceId: this.selectedSourceId
            });
        }
    }

    /**
     * Initializes the pager and source params from url query params
     */
    initializeRequestParams = () => {
        this.route.queryParams
            .map((queryParams) => queryParams['page'])
            .subscribe((page: string) =>
                !isNaN(parseInt(page)) ? this.pager.setPage(parseInt(page)) : this.setInitialPage());

        this.route.queryParams
            .map((queryParams) => queryParams['source'])
            .subscribe((sourceId: string) =>
                this.selectedSourceId = sourceId ? parseInt(sourceId) : null);


    };

    private setInitialPage = () => {
        this.pager.setPage(1)
        this.navigator.setQueryParam('page', 1);
    };

    /**
     * Updates pager and navigation according to loaded data
     * @param data
     */
    handleLoadedNews = (data: any) => {
        if(!data.length){
            this.hasMoreItems = false;
        }
        this.pager.appendItems(data);
        this.navigator.setQueryParam('page', this.pager.getPageNumber());
        this.navigator.setQueryParam('source', this.selectedSourceId);
        this.loadingInProgress = false;
        this.refreshInProgress = false;
    };

    /**
     * Returns current request params in form of object
     * @param refresh - sets the 'refresh' param
     * @returns {{pageIndex: number, pageSize: number, sourceId: number, refresh: boolean}}
     */
    getRequestParams = (refresh: boolean = false) => {
        return {
            pageIndex: this.pager.getNextPageStartIndex(),
            pageSize: this.pager.getPageSize(),
            sourceId: this.selectedSourceId,
            refresh: refresh
        };
    };

    /**
     * Load the fresh list of news
     * (without launching the RSS sources refresh action on server)
     */
    reload = () => {
        this.pager.reset();
        this.loadNews(this.getRequestParams());
    };

    /**
     * Load the fresh list of news (launches the RSS sources refresh on server)
     */
    refresh = () =>{
        if(this.refreshInProgress){
            return;
        }
        this.refreshInProgress = true;
        this.pager.reset(false);
        this.loadNews(this.getRequestParams(true));
    };

    /**
     * Load next page of news
     */
    loadMore = () => {
        if(this.loadingInProgress){
            return;
        }
        this.loadNews(this.getRequestParams());
    };

    /**
     * Set selected source
     * @param source - source object
     */
    selectSource = (source: any) => {
        this.selectedSourceId = source.id;
        this.reload();
    };

    hasItems = () => {
        return this.pager.getItems().length;
    };

    /**
     * Listens to user scrolling the page and loads the next page
     * of data when user reaches the bottom
     */
    @HostListener("window:scroll", [])
    onWindowScroll = () => {
        var userScrollPosition = window.innerHeight + window.scrollY;
        if (userScrollPosition >= document.body.offsetHeight && this.hasMoreItems){
            this.loadMore();
        }
    };
}
