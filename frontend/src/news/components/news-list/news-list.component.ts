import {
    Component,
    HostListener,
    Inject,
    OnInit,
    AfterViewInit,
    Input,
    Output,
    EventEmitter
} from '@angular/core';
import {
    AbstractDataProviderService,
    BaseList,
    PagerServiceProvider,
    RequestLockerService
} from '../../../shared';

@Component({
    selector: 'news-list',
    templateUrl: './news-list.component.html',
    styleUrls: ['./news-list.component.css']
})

/**
 * Component represents a list of news
 */
export class NewsListComponent extends BaseList implements OnInit, AfterViewInit {
    protected apiRoute:string = 'news';
    private search:string = null;

    refreshInProgress:boolean = false;

    @Input() selectedSourcesIds:Array<number> = [];
    @Input() selectedTags:Array<string> = [];
    @Input() initialPage:number;
    @Input() initialSearch:string;

    @Output() onTagSelected:EventEmitter<any> = new EventEmitter<any>();
    @Output() onPageChanged:EventEmitter<any> = new EventEmitter<any>();
    @Output() onSearch:EventEmitter<any> = new EventEmitter<any>();

    constructor(protected dataProvider:AbstractDataProviderService,
                @Inject(PagerServiceProvider) pagerProvider:PagerServiceProvider,
                public requestLocker:RequestLockerService) {
        super(dataProvider, pagerProvider.getInstance());
    }

    ngOnInit() {
        window.scrollTo(0, 0);
        this.search = this.initialSearch;
        this.pager.setPage(this.initialPage);
    };

    ngAfterViewInit() {
        var preloadPageParams = {
            pageIndex: 0,
            pageSize: this.pager.getPage() * this.pager.getPageSize()
        };
        var requestParams = Object.assign(preloadPageParams, this.getRequestParams());
        this.requestLocker.lock(() => this.loadData(requestParams, true));
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
    reload(refresh:boolean = false, sourcesIds:Array<number> = [], tags:Array<string> = []) {
        window.scrollTo(0, 0);
        this.selectedSourcesIds = sourcesIds;
        this.selectedTags = tags;
        return this.requestLocker.lock(() => this.reloadData(this.getRequestParams(), refresh))
            .then(() => this.setPageUrlQueryParam())
            .catch((error:any) => this.onReloadFailed(error));
    };

    onReloadFailed(error:any) {
        console.log(error);
    };

    refresh() {
        this.reload(true, this.selectedSourcesIds, this.selectedTags);
    };

    loadMore() {
        this.requestLocker.lock(() => this.loadMoreData(this.getRequestParams()))
            .then(() => this.setPageUrlQueryParam())
            .catch(() => console.log('Loading more failed'));
    };

    setPageUrlQueryParam() {
        this.onPageChanged.emit({page: this.pager.getPage()});
    };

    selectTag(tag:string) {
        if (this.isTagSelected(tag)) {
            return;
        }
        this.onTagSelected.emit({tag: tag});
    };

    isTagSelected(tag:string) {
        return this.selectedTags.indexOf(tag) !== -1;
    };

    searchNews(search:string) {
        this.search = search;
        this.onSearch.emit({search: this.search});
        this.reload(true, this.selectedSourcesIds, this.selectedTags);
    };

    getOtherSources(newsItem:any) {
        return newsItem.sources.slice(1, newsItem.sources.length);
    };

    /**
     * Listens to user scrolling the page and loads the next page
     * of data when user reaches the bottom
     */
    @HostListener('window:scroll')
    onWindowScroll() {
        var userScrollPosition = window.innerHeight + window.scrollY;
        if (userScrollPosition >= document.body.offsetHeight &&
            this.hasMoreItems && !this.loadingInProgress) {
            this.loadMore();
        }
    };
}
