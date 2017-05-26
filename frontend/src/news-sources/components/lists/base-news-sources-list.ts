import {Input} from '@angular/core';
import {AbstractDataProviderService, PagerServiceProvider, BaseList} from '../../../shared';
import {AppSettings} from '../../../app/app.settings';

export abstract class BaseNewsSourcesListComponent extends BaseList {
	protected search: string = null;
	protected abstract apiRoute: string = 'newsSources';
	
	@Input() initiallySelectedSourcesIds: Array<any> = [];
	@Input() useSearch: boolean = false;
	@Input() onlySubscribed: boolean = false;
	@Input() noDataText: string = 'No news sources';

	constructor(dataProvider: AbstractDataProviderService,
	            pagerProvider:PagerServiceProvider){
		super(dataProvider, pagerProvider.getInstance(1, AppSettings.NEWS_SOURCES_PAGE_SIZE));
	}

	reload(): Promise<any> {
		return this.reloadData(this.getRequestParams(), true);
	};

	loadMore(): Promise<any> {
		return this.loadMoreData(this.getRequestParams());
	};

	searchNewsSource(search: string): Promise<any> {
		this.search = search;
		return this.reload();
	};

	getRequestParams() {
		return {
			search: this.search,
			subscribed: this.onlySubscribed
		};
	};
}