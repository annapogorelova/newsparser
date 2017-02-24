import {Component, Inject} from '@angular/core';
import {ApiService} from '../../../shared/services/api/api.service';
import {PagerServiceProvider} from '../../../shared/services/pager/pager.service.provider';
import {BaseListComponent} from '../../../shared/components/base-list/base-list.component';

/**
 * Component for listing the news sources available for user
 */
@Component({
    selector: 'available-news-sources',
    templateUrl: './available-news-sources.component.html',
    styles: [require('./available-news-sources.component.css').toString()]
})

export class AvailableNewsSourcesComponent extends BaseListComponent{
    public search: string = null;
    public subscriptionInProgress: boolean = false;

    constructor(@Inject(ApiService) apiService: ApiService,
                @Inject(PagerServiceProvider) pagerProvider: PagerServiceProvider){
        super(apiService, pagerProvider.getInstance(1, 30), 'newsSources');
    }

    ngOnInit(){
        this.loadMoreData(this.getRequestParams());
    }

    reload = () => {
        return this.reloadData(this.getRequestParams());
    };

    searchNewsSource = (event: any) => {
        if(event.keyCode > 8 && event.keyCode < 48){
            return;
        }

        this.reload();
    };

    subscribe = (newsSource: any) => {
        this.subscriptionInProgress = true;
        this.apiService.post('subscription', {sourceId: newsSource.id})
            .then(response => this.onSubscribed())
            .catch(error => this.onSubscribedError(error));
    };

    onSubscribed = () => {
        this.subscriptionInProgress = false;
        alert('Subscribed');
    };

    getRequestParams = () => {
        return {
            search: this.search
        };
    };

    loadMore = () => {
        return this.loadMoreData(this.getRequestParams());
    };

    onSubscribedError = (error: any) => {
        this.subscriptionInProgress = false;
        alert('Failed to subscribe');
    };
}