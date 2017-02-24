import {Component} from '@angular/core';
import {ApiService} from '../../services/api/api.service';
import {PagerService} from '../../services/pager/pager.service';

@Component({
    templateUrl: './base-list.component.html'
})

/**
 * Component contains methods to manipulate the lists data,
 * to be extended by specific list components
 */
export class BaseListComponent {
    /**
     * Flag is used to prevent simultaneous data loading
     */
    protected loadingInProgress: boolean;
    /**
     * Flag is used to prevent infinite scroll data loading when there is no data
     * @type {boolean}
     */
    protected hasMoreItems: boolean = true;

    constructor(protected apiService: ApiService,
                protected pager: PagerService,
                private apiRoute: string){
    }

    /**
     * Get paging request params
     * @returns {{pageIndex: number, pageSize: number}}
     */
    private getBaseRequestParams = () => {
        return {
            pageIndex: this.pager.getNextPageStartIndex(),
            pageSize: this.pager.getPageSize()
        };
    };

    /**
     * Merge paging request params with child component's request params
     * @param params
     * @returns {{pageIndex: number, pageSize: number}&U}
     */
    private mergeRequestParams = (params: any) => {
        var baseParams = this.getBaseRequestParams();
        return Object.assign(baseParams, params);
    };

    /**
     * Basic method to call api GET for data
     * @param params
     * @returns {Promise<TResult|T>}
     */
    public loadData = (params: any) => {
        this.loadingInProgress = true;
        var mergedParams = this.mergeRequestParams(params);
        return this.apiService.get(this.apiRoute, mergedParams)
            .then(data => this.onLoaded(data))
            .catch(error => this.onError(error));
    };

    /**
     * Load next page of data
     * @param params
     * @returns {any}
     */
    protected loadMoreData = (params: any) => {
        if(this.loadingInProgress){
            return Promise.reject({});
        }

        return this.loadData(params);
    };

    /**
     * Reload data from first page
     * @param params
     * @returns {any}
     */
    protected reloadData = (params: any) => {
        if(this.loadingInProgress){
            return Promise.reject({});
        }
        this.pager.reset();
        return this.loadData(params);
    };

    /**
     * Basic loading success callback
     * @param data
     */
    protected onLoaded = (data: any) => {
        if(!data.length){
            this.hasMoreItems = false;
        }

        this.pager.appendItems(data);
        this.loadingInProgress = false;
    };

    /**
     * Basic loading error callback
     * @param error
     */
    protected onError = (error: any) => {
        this.loadingInProgress = false;
    };

    /**
     * Returns pager item's length
     * @returns {number}
     */
    protected hasItems = () => {
        return this.pager.getItems().length;
    };
}