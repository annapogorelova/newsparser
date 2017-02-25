import {Component, Inject, Input} from '@angular/core';
import {ApiService} from '../../../shared/services/api/api.service';
import {BaseListComponent} from '../../../shared/components/base-list/base-list.component';
import {PagerServiceProvider} from '../../../shared/services/pager/pager.service.provider';
import {AppSettings} from '../../../app/app.settings';

@Component({
    selector: 'news-sources',
    templateUrl: './news-sources-list.component.html',
    styles: [require('./news-sources-list.component.css').toString()]
})

/**
 * Component for displaying the list of news sources
 */
export class NewsSourcesListComponent extends BaseListComponent {
    public selectedSourceId: number;

    @Input() selectHandler: any = null;
    @Input() initialSelectedSourceId: any = null;

    constructor(@Inject(ApiService) apiService: ApiService,
                @Inject(PagerServiceProvider) pagerProvider:PagerServiceProvider){
        super(apiService, pagerProvider.getInstance(1, AppSettings.NEWS_SOURCES_PAGE_SIZE), 'subscription');
    }

    ngOnInit(){
        this.pager.appendItems([{name: 'All', id: null}]);
        this.loadData({}, true).then(this.handleLoadedNewsSources);
    }

    handleLoadedNewsSources = () => {
        this.selectedSourceId = this.initialSelectedSourceId;
    };

    onSelect = (source: any) => {
        this.selectedSourceId = source.id;

        if(this.selectHandler){
            this.selectHandler(source);
        }
    };
}