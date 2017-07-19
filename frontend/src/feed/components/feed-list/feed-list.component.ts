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
    selector: 'feed-list',
    templateUrl: './feed-list.component.html',
    styleUrls: ['./feed-list.component.css']
})

/**
 * Component represents a list of feed
 */
export class FeedListComponent extends BaseList implements OnInit, AfterViewInit {
    protected apiRoute:string = 'feed';
    private search:string = null;

    refreshInProgress:boolean = false;

    @Input() selectedChannelsIds:Array<number> = [];
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
     * @returns {{channels: Array<number>, search: string, tags: Array<string>}}
     */
    getRequestParams = () => {
        return {
            channels: this.selectedChannelsIds,
            search: this.search,
            tags: this.selectedTags
        };
    };

    /**
     * Load the fresh list of feed
     */
    reload(refresh:boolean = false, channelsIds:Array<number> = [], tags:Array<string> = []) {
        window.scrollTo(0, 0);
        this.selectedChannelsIds = channelsIds;
        this.selectedTags = tags;
        return this.requestLocker.lock(() => this.reloadData(this.getRequestParams(), refresh))
            .then(() => this.setPageUrlQueryParam());
    };

    refresh() {
        this.reload(true, this.selectedChannelsIds, this.selectedTags);
    };

    loadMore() {
        this.requestLocker.lock(() => this.loadMoreData(this.getRequestParams()))
            .then(() => this.onLoadMoreSucceeded());
    };

    onLoadMoreSucceeded() {
        this.setPageUrlQueryParam();
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
        this.reload(true, this.selectedChannelsIds, this.selectedTags);
    };

    getOtherChannels(feedItem:any) {
        return feedItem.channels.slice(1, feedItem.channels.length);
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
