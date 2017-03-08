import {ApiService} from '../../services/api/api.service';
import {PagerService} from '../../services/pager/pager.service';

/**
 * Component contains methods to manipulate the lists data,
 * to be extended by specific list components
 */
export abstract class BaseListComponent {
    /**
     * Api route to make requests to
     */
    protected abstract apiRoute: string;
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
                protected pager: PagerService){
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
     * @param refresh
     * @returns {Promise<TResult|T>}
     */
    protected loadData = (params: any, refresh: boolean = false) => {
        this.loadingInProgress = true;
        var mergedParams = this.mergeRequestParams(params);
        return this.apiService.get(this.apiRoute, mergedParams, null, refresh)
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
     * Reload data from the first page
     * @param params
     * @param refresh
     * @returns {any}
     */
    protected reloadData = (params: any, refresh: boolean = false) => {
        if(this.loadingInProgress){
            return Promise.reject({});
        }
        this.pager.reset();
        return this.loadData(params, refresh);
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