import {Component} from '@angular/core';
import {ApiService} from '../../../shared/services/api/api.service';
import {PagerServiceProvider} from '../../../shared/services/pager/pager.service.provider';
import {PagerService} from '../../../shared/services/pager/pager.service';

@Component({
    selector: 'user-subscriptions',
    templateUrl: 'user-subscriptions.component.html',
    styles: [require('./user-subscriptions.component.css').toString()]
})

/**
 * Component for editing the list of news sources
 */
export class UserSubscriptionsComponent {
    public loadingInProgress: boolean = false;
    public unsubscribeInProgress: boolean = false;
    public pager: PagerService = null;
    public hasMoreItems: boolean = true;

    constructor(private apiService: ApiService, private pagerProvider: PagerServiceProvider){
        this.pager = this.pagerProvider.getInstance(0, 30);
    }

    ngOnInit() {
        this.loadNewsSources({pageIndex: 0, pageSize: this.pager.getPageSize()});
    }

    loadNewsSources = (params: any) => {
        this.loadingInProgress = true;
        this.apiService.get('subscription', params)
            .then(newsSources => this.handleLoadedNewsSources(newsSources))
            .catch(error => this.handleErrorResponse(error));
    };

    hasItems = () => {
        return this.pager.getItems().length;
    };

    handleLoadedNewsSources = (data: any) => {
        if(!data.length){
            this.hasMoreItems = false;
        }
        this.loadingInProgress = false;
        this.pager.appendItems(data);
    };

    handleErrorResponse = (error: any) => {
        this.loadingInProgress = false;
    };

    unsubscribe = (newsSource: any) => {
        this.unsubscribeInProgress = true;
        this.apiService.delete('subscription', newsSource.id)
            .then(response => this.handleSubscriptionDelete(response))
            .catch(error => this.handleSubscriptionDeleteError(error));
    };

    handleSubscriptionDelete = (response: any) => {
        this.unsubscribeInProgress = false;
    };

    handleSubscriptionDeleteError = (error: any) => {
        this.unsubscribeInProgress = false;
    };

    getRequestParams = () => {
        return {
            pageIndex: this.pager.getNextPageStartIndex(),
            pageSize: this.pager.getPageSize()
        };
    };

    loadMore = () => {
        if(this.loadingInProgress){
            return;
        }
        this.loadNewsSources(this.getRequestParams());
    };
}