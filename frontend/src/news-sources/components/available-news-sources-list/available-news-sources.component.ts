import {Component} from '@angular/core';
import {ApiService} from '../../../shared/services/api/api.service';
import {PagerService} from '../../../shared/services/pager/pager.service';
import {PagerServiceProvider} from '../../../shared/services/pager/pager.service.provider';

/**
 * Component for listing the news sources available for user
 */
@Component({
    selector: 'available-news-sources',
    templateUrl: './available-news-sources.component.html',
    styles: [require('./available-news-sources.component.css').toString()]
})

export class AvailableNewsSourcesComponent {
    public loadingInProgress: boolean = false
    public hasMoreItems: boolean = true;
    public search: string = null;
    public subscriptionInProgress: boolean = false;
    public pager: PagerService = null;

    constructor(private apiService: ApiService, private pagerProvider: PagerServiceProvider){
        this.pager = this.pagerProvider.getInstance();
    }

    ngOnInit(){
        this.loadNewsSources({pageIndex: 0, pageSize: this.pager.getPageSize(), search: null});
    }

    reload = () => {
        this.pager.reset();
        this.loadNewsSources({pageIndex: 0, pageSize: this.pager.getPageSize(), search: this.search});
    };

    searchNewsSource = (event: any) => {
        if(event.keyCode > 8 && event.keyCode < 48){
            return;
        }

        this.reload();
    };

    hasItems = () => {
        return this.pager.getItems().length;
    };

    loadNewsSources = (params: any) => {
        this.loadingInProgress = true;
        this.apiService.get('newssources', params)
            .then(data => this.handleLoadedNewsSources(data))
            .catch(error => this.handleLoadingError(error));
    };

    handleLoadedNewsSources = (data: any) => {
        if(!data.length){
            this.hasMoreItems = false;
        }
        this.pager.appendItems(data);
        this.loadingInProgress = false;
    };

    handleLoadingError = (error: any) => {
        this.loadingInProgress = false;
    };

    subscribe = (newsSource: any) => {
        this.subscriptionInProgress = true;
        this.apiService.post('subscription', {sourceId: newsSource.id})
            .then(response => this.onSubscribed())
            .catch(error => this.onSubscribedError(error));
    };

    onSubscribed = () => {
        this.subscriptionInProgress = false;
    };

    getRequestParams = () => {
        return {
            pageIndex: this.pager.getNextPageStartIndex(),
            pageSize: this.pager.getPageSize(),
            search: this.search
        };
    };

    loadMore = () => {
        if(this.loadingInProgress){
            return;
        }
        this.loadNewsSources(this.getRequestParams());
    };

    onSubscribedError = (error: any) => {
        this.subscriptionInProgress = false;
        alert('Failed to subscribe');
    };
}