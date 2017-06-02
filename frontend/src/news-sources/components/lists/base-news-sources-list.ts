import {Input} from '@angular/core';
import {AbstractDataProviderService, PagerServiceProvider, PagedList} from '../../../shared';
import {AppSettings} from '../../../app/app.settings';


export abstract class BaseNewsSourcesListComponent extends PagedList {
    protected search:string = null;
    protected abstract apiRoute:string = 'newsSources';
    protected abstract onlySubscribed:boolean;

    @Input() initiallySelectedSourcesIds:Array<any> = [];
    @Input() useSearch:boolean = false;
    @Input() noDataText:string = 'No news sources';

    constructor(dataProvider:AbstractDataProviderService,
                pagerProvider:PagerServiceProvider) {
        super(dataProvider, pagerProvider.getInstance(1, AppSettings.NEWS_SOURCES_PAGE_SIZE));
    }

    nextPage(refresh:boolean = false):Promise<any> {
        return super.nextPage(this.getRequestParams(), refresh);
    };

    firstPage(refresh:boolean = false):Promise<any> {
        return super.firstPage(this.getRequestParams(), refresh);
    };

    prevPage(refresh:boolean = false):Promise<any> {
        return super.prevPage(this.getRequestParams(), refresh);
    };

    lastPage(refresh:boolean = false):Promise<any> {
        return super.lastPage(this.getRequestParams(), refresh);
    };

    resetPage():Promise<any> {
        return super.resetPage(this.getRequestParams());
    };

    searchNewsSource(search:string):Promise<any> {
        this.search = search;
        return this.resetPage();
    };

    getRequestParams() {
        return {
            search: this.search,
            subscribed: this.onlySubscribed
        };
    };
}