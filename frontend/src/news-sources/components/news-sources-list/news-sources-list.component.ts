import {Component, Inject, Input, Output, EventEmitter} from '@angular/core';
import {ApiService} from '../../../shared/services/api/api.service';
import {PagerServiceProvider} from '../../../shared/services/pager/pager.service.provider';
import {AppSettings} from '../../../app/app.settings';
import {BaseList} from '../../../shared/components/base-list/base-list';

@Component({
    selector: 'news-sources',
    templateUrl: 'news-sources-list.component.html',
    styleUrls: ['./news-sources-list.component.css']
})

/**
 * Component for displaying the list of news sources
 */
export class NewsSourcesListComponent extends BaseList {
    protected apiRoute: string = 'newsSources';
    private search: string = null;
    public selectedSourcesIds: Array<any> = [];

    @Input() initiallySelectedSourcesIds: Array<any> = [];
    @Input() useSearch: boolean = false;
    @Input() onlySubscribed: boolean = false;
    @Input() noDataText: string = 'No news sources';

    @Output() onSelect: EventEmitter<any> = new EventEmitter<any>();
    @Output() onDeselect: EventEmitter<any> = new EventEmitter<any>();

    constructor(@Inject(ApiService) apiService: ApiService,
                @Inject(PagerServiceProvider) pagerProvider:PagerServiceProvider){
        super(apiService, pagerProvider.getInstance(1, AppSettings.NEWS_SOURCES_PAGE_SIZE));
    }

    ngOnInit(){
        this.loadData(this.getRequestParams(), true).then(() => this.handleLoadedNewsSources());
    }

    handleLoadedNewsSources = () => {
        this.selectedSourcesIds = this.selectedSourcesIds.concat(this.initiallySelectedSourcesIds);
    };

    selectNewsSource = (source: any) => {
        this.selectedSourcesIds.push(source.id);
        this.onSelect.emit({source: source});
    };

    deselectNewsSource = (source: any) => {
        this.selectedSourcesIds = this.selectedSourcesIds.filter(function (item) {
            return item !== source.id;
        });
        this.onDeselect.emit({source: source});
    };

    isSelected = (source: any) => {
        return this.selectedSourcesIds.indexOf(source.id) !== -1;
    };

    isSubscribed = (source: any) => {
        if(!this.onlySubscribed && source.isSubscribed){
            return true;
        }
    };

    reload = () => {
        return this.reloadData(this.getRequestParams(), true).then(() => this.onReload());
    };

    onReload = () => {
        this.selectedSourcesIds = [];
    };

    loadMore = () => {
        return this.loadMoreData(this.getRequestParams());
    };

    searchNewsSource = (search: string) => {
        this.search = search;
        this.reload();
    };

    getRequestParams = () => {
        return {
            search: this.search,
            subscribed: this.onlySubscribed
        };
    };
}