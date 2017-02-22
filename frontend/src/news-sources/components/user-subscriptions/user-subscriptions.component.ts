import {Component} from '@angular/core';
import {ApiService} from '../../../shared/services/api/api.service';

@Component({
    selector: 'user-subscriptions',
    templateUrl: 'user-subscriptions.component.html',
    styles: [require('./user-subscriptions.component.css').toString()]
})

/**
 * Component for editing the list of news sources
 */
export class UserSubscriptionsComponent {
    public newsSources: any = [];
    public loadingInProgress: boolean = false;
    public unsubscribeInProgress: boolean = false;

    constructor(private apiService: ApiService){
        this.loadNewsSources();
    }

    loadNewsSources = () => {
        this.loadingInProgress = true;
        this.apiService.get('subscription')
            .then(newsSources => this.handleLoadedNewsSources(newsSources))
            .catch(error => this.handleErrorResponse(error));
    };

    hasItems = () => {
        return this.newsSources.length;
    };

    handleLoadedNewsSources = (data: any) => {
        this.loadingInProgress = false;
        this.newsSources = this.newsSources.concat(data);
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
}