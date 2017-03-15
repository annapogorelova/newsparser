import {Component, HostListener, Inject} from '@angular/core';
import {ApiService} from '../../../shared/services/api/api.service';
import {NavigatorService} from '../../../shared/services/navigator/navigator.service';
import {ActivatedRoute} from '@angular/router';
import {PagerServiceProvider} from '../../../shared/services/pager/pager.service.provider';
import {BaseListComponent} from '../../../shared/components/base-list/base-list';

@Component({
    selector: 'news-list',
    templateUrl: './news-list.component.html',
    styles: [require('./news-list.component.css').toString()]
})

/**
 * Component represents a list of news
 */
export class NewsListComponent extends BaseListComponent{
    protected apiRoute: string = 'news';
    public refreshInProgress: boolean = false;
    public selectedSourcesIds: Array<number> = [];
    public selectedTags: Array<string> = [];
    private search: string = null;

    constructor(@Inject(ApiService) apiService: ApiService,
                @Inject(PagerServiceProvider) pagerProvider: PagerServiceProvider,
                private navigator: NavigatorService,
                @Inject(ActivatedRoute) private route:ActivatedRoute){
        super(apiService, pagerProvider.getInstance());
    }

    ngOnInit(){
        window.scrollTo(0, 0);
        this.initializeRequestParams();
    }

    ngAfterViewInit() {
        var preloadPageParams = {
            pageSize: this.pager.calculatePageSize(this.pager.getPage())
        };
        var requestParams = Object.assign(preloadPageParams, this.getRequestParams());
        this.loadData(requestParams, true);
    }

    /**
     * Initializes the pager and source params from url query params
     */
    initializeRequestParams = () => {
        this.route.queryParams
            .map((queryParams) => queryParams['page'])
            .subscribe((page: string) =>
                !isNaN(parseInt(page)) ? this.pager.setPage(parseInt(page)) : this.pager.setPage(1));

        this.route.queryParams
            .map((queryParams) => queryParams['sources'])
            .subscribe((sources: string) =>
                this.selectedSourcesIds = sources ? sources.split(',').map(id => parseInt(id)) : []);

        this.route.queryParams
            .map((queryParams) => queryParams['search'])
            .subscribe((search: string) => this.search = search);

        this.route.queryParams
            .map((queryParams) => queryParams['tags'])
            .subscribe((tags: string) => this.selectedTags = tags ? tags.split(',') : []);
    };

    /**
     * Returns current request params in form of object
     * @returns {{pageIndex: number, pageSize: number, sourceId: number, refresh: boolean}}
     */
    getRequestParams = () => {
        return {
            sources: this.selectedSourcesIds,
            search: this.search,
            tags: this.selectedTags
        };
    };

    /**
     * Load the fresh list of news
     * (without launching the RSS sources refresh action on server)
     */
    reload = (refresh: boolean = false) => {
        window.scrollTo(0, 0);
        this.reloadData(this.getRequestParams(), refresh)
            .then(() => this.setPageUrlQueryParam())
            .catch(() => console.log('Reload failed'));
    };

    refresh = (event: any) => {
        this.reload(true);
    };

    loadMore = () => {
        this.loadMoreData(this.getRequestParams())
            .then(() => this.setPageUrlQueryParam())
            .catch(() => console.log('Loading more failed'));
    };

    setPageUrlQueryParam = () => {
        this.navigator.setQueryParam('page', this.pager.getPage());
    };

    onSelectSource = (event: any) => {
        this.selectedSourcesIds.push(event.source.id);
        this.navigator.setQueryParam('sources', this.selectedSourcesIds.join(','));
        this.reload();
    };

    onDeselectSource = (event: any) => {
        this.selectedSourcesIds = this.selectedSourcesIds.filter(function (item) {
            return item !== event.source.id;
        });
        this.navigator.setQueryParam('sources', this.selectedSourcesIds.join(','));
        this.reload();
    };

    selectTag = (tag: any) => {
        this.selectedTags.push(tag.name);
        this.navigator.setQueryParam('tags', this.selectedTags.join(','));
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
