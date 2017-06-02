import {Component, ViewChild, OnInit, AfterViewInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {ApiService, NoticesService, NavigatorService} from '../../../shared';

@Component({
    selector: 'subscriptions-settings',
    templateUrl: 'subscriptions-settings.component.html',
    styleUrls: ['./subscriptions-settings.component.css']
})

/**
 * Component contains methods for subscribing to and unsubscribing from news sources
 */
export class SubscriptionsSettingsComponent implements OnInit, AfterViewInit {
    selectedTabId:string = 'subscribed';
    submitSucceeded:boolean;
    submitFailed:boolean;
    submitCompleted:boolean;
    submitInProgress:boolean;

    @ViewChild('subscribedSourcesList') subscribedSourcesList:any;
    @ViewChild('allSourcesList') allSourcesList:any;

    tabsComponentsMap:any;
    tabsIds:Array<any> = ['subscribed', 'all', 'add'];

    constructor(private apiService:ApiService,
                private navigator:NavigatorService,
                private route:ActivatedRoute,
                private notices:NoticesService) {
    }

    ngOnInit() {
        this.resetForm();
    };

    setActiveTab(fragment:string) {
        this.selectedTabId = fragment || this.tabsIds[0];
        this.navigator.navigate([], {fragment: this.selectedTabId});
    };

    ngAfterViewInit() {
        this.tabsComponentsMap = {
            'subscribed': this.subscribedSourcesList,
            'all': this.allSourcesList
        };

        this.route.fragment.subscribe((fragment:string) => this.setActiveTab(fragment));
    };

    onTabChange(event:any) {
        this.selectedTabId = event.nextId;
        this.navigator.navigate([], {fragment: this.selectedTabId});
        this.resetForm();
        var activeComponent = this.tabsComponentsMap[this.selectedTabId];
        if (activeComponent && typeof activeComponent['resetPage'] === 'function') {
            activeComponent.resetPage();
        }
    };

    resetForm() {
        this.submitSucceeded = false;
        this.submitFailed = false;
        this.submitCompleted = false;
        this.submitInProgress = false;
    };

    setRequestCompleted(succeeded:boolean, message:string) {
        this.submitInProgress = false;
        this.submitCompleted = true;
        this.submitSucceeded = succeeded;
        this.submitFailed = !this.submitSucceeded;
        this.submitSucceeded ? this.notices.success(message) : this.notices.error(message);
    };

    /**
     * Performs subscription
     * @param event
     */
    subscribe(event:any) {
        this.resetForm();
        this.submitInProgress = true;
        this.apiService.post(`subscription/${event.source.id}`, null)
            .then(response => this.onSubscribeSucceeded(response, event.source.id))
            .catch(error => this.onSubscribeFailed(error));
    };

    /**
     * Successful subscription callback
     */
    onSubscribeSucceeded(response:any, subscriptionId:number) {
        this.setRequestCompleted(true, response.message);
        this.updateSubscriptionInfo(subscriptionId, true);
    };

    /**
     * Failed subscription callback
     * @param error
     */
    onSubscribeFailed(error:any) {
        this.setRequestCompleted(false, error.message);
    };

    /**
     * Performs unsubscription
     * @param event
     */
    unsubscribe(event:any) {
        this.apiService.delete(`subscription/${event.source.id}`)
            .then(response => this.onUnsubscribeSucceeded(response, event.source.id))
            .catch(error => this.onUnsubscribeFailed(error));
    };

    /**
     * Successful unsubscription callback
     */
    onUnsubscribeSucceeded(response:any, subscriptionId:number) {
        this.setRequestCompleted(true, response.message);
        this.updateSubscriptionInfo(subscriptionId, false);
    };

    /**
     * Failed unsubscription callback
     * @param error
     */
    onUnsubscribeFailed(error:any) {
        this.setRequestCompleted(false, error.message);
    };

    private updateSubscriptionInfo(sourceId:number, isSubscribed:boolean) {
        this.tabsComponentsMap[this.selectedTabId].updateSubscriptionState(sourceId, isSubscribed);
        this.tabsComponentsMap[this.selectedTabId].hideSubscriptionInfo();
    };
}