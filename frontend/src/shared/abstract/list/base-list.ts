import {PagerService, AbstractDataProviderService} from '../../services';

/**
 * Class contains methods to manipulate the lists data,
 * to be extended by specific list components
 */
export abstract class BaseList {
    /**
     * Api route to make requests to
     */
    protected abstract apiRoute:string;
    /**
     * Flag is used to prevent simultaneous data loading
     */
    protected loadingInProgress:boolean;
    /**
     * Flag is used to prevent infinite scroll data loading when there is no data
     * @type {boolean}
     */
    protected hasMoreItems:boolean = true;

    /**
     * List items
     * @type {Array}
     */
    protected items:Array<any> = [];

    constructor(protected dataProvider:AbstractDataProviderService,
                protected pager:PagerService) {
    }

    /**
     * Get paging request params
     * @returns {{pageIndex: number, pageSize: number}}
     */
    private getPagingParams() {
        return {
            pageIndex: this.pager.getOffset(),
            pageSize: this.pager.getPageSize()
        };
    };

    /**
     * Merge paging request params with child component's request params
     * @param params
     * @returns {{pageIndex: number, pageSize: number}}
     */
    private mergeRequestParams(params:any) {
        var baseParams = this.getPagingParams();
        return Object.assign(baseParams, params);
    };

    /**
     * Basic method to call api GET for data
     * @param params
     * @param refresh
     * @returns {Promise<TResult|T>}
     */
    protected loadData(params:any, refresh:boolean = false) {
        this.loadingInProgress = true;
        var mergedParams = this.mergeRequestParams(params);
        return this.dataProvider.get(this.apiRoute, mergedParams, null, refresh)
            .then(data => this.onLoaded(data))
            .catch(error => this.onError(error));
    };

    /**
     * Load next page of data
     * @param params
     * @returns {any}
     */
    protected loadMoreData(params:any, refresh:boolean = false) {
        if (this.loadingInProgress) {
            return Promise.reject({});
        }

        this.pager.setPage(this.pager.getPage() + 1);
        return this.loadData(params, refresh);
    };

    /**
     * Reload data from the first page
     * @param params
     * @param refresh
     * @returns {any}
     */
    protected reloadData(params:any, refresh:boolean = false) {
        if (this.loadingInProgress) {
            return Promise.reject({});
        }
        this.pager.reset();
        this.resetItems();
        this.hasMoreItems = true;
        return this.loadData(params, refresh);
    };

    /**
     * Basic loading success callback
     * @param data
     */
    protected onLoaded(response:any) {
        if (!response.data.length) {
            this.hasMoreItems = false;
        }

        this.items = this.items.concat(response.data);
        if (response.total) {
            this.pager.setTotal(response.total);
        }
        this.loadingInProgress = false;
    };

    /**
     * Basic loading error callback
     * @param error
     */
    protected onError(error:any) {
        this.loadingInProgress = false;
    };

    /**
     * Returns pager item's length
     * @returns {number}
     */
    protected hasItems() {
        return this.items.length;
    };

    protected resetItems() {
        this.items = [];
    };
}