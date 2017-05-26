import {Component, ViewChild, OnInit, AfterViewInit} from '@angular/core';
import {NavigatorService, ApiService} from '../../../../shared';
import {ActivatedRoute} from '@angular/router';

@Component({
    selector: 'subscriptions',
    templateUrl: 'subscriptions.component.html',
    styleUrls: ['./subscriptions.component.css']
})

/**
 * Component contains methods for subscribing to and unsubscribing from news sources
 */
export class SubscriptionsComponent implements OnInit, AfterViewInit {
	selectedTabId: string = 'subscribedSources';
	responseMessage: string;
	submitSucceeded: boolean;
	submitFailed: boolean;
	submitCompleted: boolean;
	submitInProgress: boolean;

    @ViewChild('subscribedSourcesList') subscribedSourcesList: any;
    @ViewChild('allSourcesList') allSourcesList: any;

	tabsComponentsMap: any;

    constructor(private apiService: ApiService,
                private navigator: NavigatorService,
                private route: ActivatedRoute) {}

    ngOnInit() {
        this.navigator.navigate([], {fragment: 'subscriptions'});
	    this.resetForm();
    };

	ngAfterViewInit() {
		this.tabsComponentsMap = {
			'subscribedSources': this.subscribedSourcesList,
			'allSources': this.allSourcesList
		};
	};

	onTabChange(event: any){
		this.selectedTabId = event.nextId;
		this.resetForm();
		this.tabsComponentsMap[this.selectedTabId].reload();
	};

	resetForm(){
		this.submitSucceeded = false;
		this.submitFailed = false;
		this.submitCompleted = false;
		this.submitInProgress = false;
		this.responseMessage = null;
	};

	setRequestCompleted(succeeded: boolean, message: string){
		this.submitInProgress = false;
		this.submitCompleted = true;
		this.submitSucceeded = succeeded;
		this.submitFailed = !this.submitSucceeded;
		this.responseMessage = message;
	};

    /**
     * Performs subscription
     * @param event
     */
    subscribe = (event: any) => {
	    this.resetForm();
	    this.submitInProgress = true;
        this.apiService.post(`subscription/${event.source.id}`, null)
            .then(response => this.onSubscribeSucceeded(response, event.source.id))
            .catch(error => this.onSubscribeFailed(error));
    };

    /**
     * Successful subscription callback
     */
    onSubscribeSucceeded = (response: any, subscriptionId: number) => {
	    this.setRequestCompleted(true, response.message);
	    this.updateSubscriptionInfo(subscriptionId, true);
    };

    /**
     * Failed subscription callback
     * @param error
     */
    onSubscribeFailed = (error: any) => {
	    this.setRequestCompleted(false, error.message);
    };

    /**
     * Performs unsubscription
     * @param event
     */
    unsubscribe = (event: any) => {
        this.apiService.delete(`subscription/${event.source.id}`)
            .then(response => this.onUnsubscribeSucceeded(response, event.source.id))
            .catch(error => this.onUnsubscribeFailed(error));
    };

    /**
     * Successful unsubscription callback
     */
    onUnsubscribeSucceeded(response: any, subscriptionId: number) {
	    this.setRequestCompleted(true, response.message);
	    this.updateSubscriptionInfo(subscriptionId, false);
    };

    /**
     * Failed unsubscription callback
     * @param error
     */
    onUnsubscribeFailed = (error: any) => {
	    this.setRequestCompleted(false, error.message);
    };

	private refreshList(){
		if(this.selectedTabId === 'subscribedSources'){
			this.subscribedSourcesList.reload();
		} else if(this.selectedTabId === 'allSources'){
			this.allSourcesList.reload();
		}
	};

	private updateSubscriptionInfo(sourceId: number, isSubscribed: boolean){
		this.tabsComponentsMap[this.selectedTabId].updateSubscriptionState(sourceId, isSubscribed);
		this.tabsComponentsMap[this.selectedTabId].hideSubscriptionInfo();
	};
}