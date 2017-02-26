import {Component, HostListener, Inject} from '@angular/core';
import {ApiService} from '../../../shared/services/api/api.service';
import {NavigatorService} from '../../../shared/services/navigator/navigator.service';
import {ActivatedRoute} from '@angular/router';
import {PagerServiceProvider} from '../../../shared/services/pager/pager.service.provider';
import {BaseListComponent} from '../../../shared/components/base-list/base-list.component';

@Component({
    selector: 'news-list',
    templateUrl: './news-list.component.html',
    styles: [require('./news-list.component.css').toString()]
})

/**
 * Component represents a list of news
 */
export class NewsListComponent extends BaseListComponent{
    public refreshInProgress: boolean = false;
    public selectedSourceId: number = null;
    private search: string = null;

    constructor(@Inject(ApiService) apiService: ApiService,
                @Inject(PagerServiceProvider) pagerProvider: PagerServiceProvider,
                private navigator: NavigatorService,
                @Inject(ActivatedRoute) private route:ActivatedRoute){
        super(apiService, pagerProvider.getInstance(), 'news');
    }

    ngOnInit(){
        window.scrollTo(0, 0);
        this.initializeRequestParams();

        var itemsToPreload = this.pager.getNumberOfItemsToPreload();
        if(itemsToPreload){
            this.loadData(this.getRequestParams(), true);
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

        this.route.queryParams
            .map((queryParams) => queryParams['search'])
            .subscribe((search: string) => this.search = search);
    };

    private setInitialPage = () => {
        this.pager.setPage(1);
        this.navigator.setQueryParam('page', 1);
    };

    /**
     * Returns current request params in form of object
     * @param refresh - sets the 'refresh' param
     * @returns {{pageIndex: number, pageSize: number, sourceId: number, refresh: boolean}}
     */
    getRequestParams = (refresh: boolean = false) => {
        return {
            sourceId: this.selectedSourceId,
            search: this.search,
            refresh: refresh
        };
    };

    /**
     * Load the fresh list of news
     * (without launching the RSS sources refresh action on server)
     */
    reload = (refresh: boolean = false) => {
        this.reloadData(this.getRequestParams(), refresh)
            .then(this.setUrlQueryParams)
            .catch(() => console.log('Reload failed'));
    };

    /**
     * Load the fresh list of news (launches the RSS sources refresh on server)
     */
    refresh = () =>{
        if(this.refreshInProgress){
            return;
        }
        this.refreshInProgress = true;
        this.pager.reset();
        this.loadData(this.getRequestParams(true), true)
            .then(this.onRefresh)
            .catch(this.onRefresh);
    };

    onRefresh = () => {
        this.refreshInProgress = false;
    };

    /**
     * Load next page of news
     */
    loadMore = () => {
        this.loadMoreData(this.getRequestParams())
            .then(this.setUrlQueryParams)
            .catch(() => console.log('Loading more failed'));
    };

    setUrlQueryParams = () => {
        this.navigator.setQueryParam('page', this.pager.getPageNumber());
        this.navigator.setQueryParam('source', this.selectedSourceId);
    };

    /**
     * Set selected source
     * @param source - source object
     */
    selectSource = (source: any) => {
        this.selectedSourceId = source.id;
        this.reload();
    };

    searchNews = (search: string) => {
        this.search = search;
        this.navigator.setQueryParam('search', this.search);
        this.reload(true);
    };

    /**
     * Listens to user scrolling the page and loads the next page
     * of data when user reaches the bottom
     */
    @HostListener("window:scroll", [])
    onWindowScroll = () => {
        var userScrollPosition = window.innerHeight + window.scrollY;
        if (userScrollPosition >= document.body.offsetHeight &&
            this.hasMoreItems && !this.loadingInProgress){
            this.loadMore();
        }
    };
}
