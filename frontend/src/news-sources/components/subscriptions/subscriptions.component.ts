import {Component, ViewChild, Inject} from '@angular/core';
import {ApiService} from '../../../shared/services/api/api.service';

@Component({
    selector: 'news-sources-settings',
    templateUrl: 'subscriptions.component.html',
    styles: [require('./subscriptions.component.css').toString()]
})

/**
 * Component contains methods for subscribing to and unsubscribing from news sources
 */
export class SubscriptionsComponent {
    @ViewChild('subscriptionsList') subscriptionsList: any;

    constructor(@Inject(ApiService) private apiService: ApiService) {}

    /**
     * Performs subscription
     * @param event
     */
    subscribe = (event: any) => {
        this.apiService.post('subscription', {sourceId: event.source.id})
            .then(response => this.onSubscribed())
            .catch(error => this.onSubscribedError(error));
    };

    /**
     * Successful subscription callback
     */
    onSubscribed = () => {
        this.subscriptionsList.reload();
    };

    /**
     * Failed subscription callback
     * @param error
     */
    onSubscribedError = (error: any) => {
        alert('Failed to subscribe');
    };

    /**
     * Performs unsubscription
     * @param event
     */
    unsubscribe = (event: any) => {
        this.apiService.delete('subscription', event.source.id)
            .then(response => this.handleUbsubscribed())
            .catch(error => this.handleUbsubscribedError(error));
    };

    /**
     * Successful unsubscription callback
     */
    handleUbsubscribed = () => {
        this.subscriptionsList.reload();
    };

    /**
     * Failed unsubscription callback
     * @param error
     */
    handleUbsubscribedError = (error: any) => {
    };
}