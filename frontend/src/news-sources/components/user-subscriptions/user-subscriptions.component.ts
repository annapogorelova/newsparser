import {Component, Inject} from '@angular/core';
import {ApiService} from '../../../shared/services/api/api.service';
import {PagerServiceProvider} from '../../../shared/services/pager/pager.service.provider';
import {BaseListComponent} from '../../../shared/components/base-list/base-list.component';
import {AppSettings} from '../../../app/app.settings';

@Component({
    selector: 'user-subscriptions',
    templateUrl: 'user-subscriptions.component.html',
    styles: [require('./user-subscriptions.component.css').toString()]
})

/**
 * Component for editing the list of news sources
 */
export class UserSubscriptionsComponent extends BaseListComponent{
    public unsubscribeInProgress: boolean = false;

    constructor(@Inject(ApiService) apiService: ApiService,
                @Inject(PagerServiceProvider) pagerProvider: PagerServiceProvider){
        super(apiService, pagerProvider.getInstance(1, AppSettings.NEWS_SOURCES_PAGE_SIZE), 'subscription')
    }

    ngOnInit() {
        this.loadData({});
    }

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
}