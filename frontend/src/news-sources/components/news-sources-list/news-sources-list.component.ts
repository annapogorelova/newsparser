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
    public selectedSourceId: number;
    protected apiRoute: string;
    private search: string = null;

    @Input() initialSelectedSourceId: any = null;
    @Input() useSearch: boolean = false;
    @Input() subscribed: boolean = false;

    @Output() onSelect: EventEmitter<any> = new EventEmitter<any>();

    constructor(@Inject(ApiService) apiService: ApiService,
                @Inject(PagerServiceProvider) pagerProvider:PagerServiceProvider){
        super(apiService, pagerProvider.getInstance(1, AppSettings.NEWS_SOURCES_PAGE_SIZE));
    }

    ngOnInit(){
        this.apiRoute = this.subscribed ? 'subscription' : 'newssources';
        this.loadData({}, true).then(this.handleLoadedNewsSources);
    }

    handleLoadedNewsSources = () => {
        this.selectedSourceId = this.initialSelectedSourceId;
    };

    selectNewsSource = (source: any) => {
        this.selectedSourceId = source.id;
        this.onSelect.emit({source: source});
    };

    reload = () => {
        return this.reloadData(this.getRequestParams(), true);
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