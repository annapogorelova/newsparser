import {Component, Inject, Input, Output, EventEmitter} from '@angular/core';
import {BaseListComponent} from '../../../shared/components/base-list/base-list';
import {ApiService} from '../../../shared/services/api/api.service';
import {PagerServiceProvider} from '../../../shared/services/pager/pager.service.provider';
import {AppSettings} from '../../../app/app.settings';

@Component({
    selector: 'news-sources',
    templateUrl: 'news-sources-list.component.html',
    styles: [require('./news-sources-list.component.css').toString()]
})

/**
 * Component for displaying the list of news sources
 */
export class NewsSourcesListComponent extends BaseListComponent{
    protected apiRoute: string;
    private search: string = null;
    public selectedSourcesIds: Array<any> = [];

    @Input() initiallySelectedSourcesIds: Array<any> = [];
    @Input() useSearch: boolean = false;
    @Input() subscribed: boolean = false;

    @Output() onSelect: EventEmitter<any> = new EventEmitter<any>();
    @Output() onDeselect: EventEmitter<any> = new EventEmitter<any>();

    constructor(@Inject(ApiService) apiService: ApiService,
                @Inject(PagerServiceProvider) pagerProvider:PagerServiceProvider){
        super(apiService, pagerProvider.getInstance(1, AppSettings.NEWS_SOURCES_PAGE_SIZE));
    }

    ngOnInit(){
        this.apiRoute = this.subscribed ? 'subscription' : 'newssources';
        this.loadData({}, true).then(() => this.handleLoadedNewsSources());
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

    isSourceSelected = (source: any) => {
        return this.selectedSourcesIds.indexOf(source.id) !== -1;
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
            search: this.search
        };
    };
}