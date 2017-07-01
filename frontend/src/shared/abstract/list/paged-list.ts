import {BaseList} from './base-list';
import {PagerService, AbstractDataProviderService} from '../../services';

/**
 * Base list class with pagination
 */
export abstract class PagedList extends BaseList {
    protected abstract apiRoute:string;

    constructor(protected dataProvider:AbstractDataProviderService,
                pager:PagerService) {
        super(dataProvider, pager);
    }

    loadPage = (params:any, refresh:boolean = false) => {
        this.resetItems();
        return this.loadData(params, refresh);
    };

    resetPage(params:any):Promise<any> {
        this.resetItems();
        return this.reloadData(params, true);
    };

    nextPage(params:any, refresh:boolean = false):Promise<any> {
        this.resetItems();
        this.pager.setPage(this.pager.getPage() + 1);
        return this.loadPage(params, refresh);
    };

    prevPage(params:any, refresh:boolean = false):Promise<any> {
        this.resetItems();
        this.pager.setPage(this.pager.getPage() - 1);
        return this.loadPage(params, refresh);
    };

    firstPage(params:any, refresh:boolean = false):Promise<any> {
        this.resetItems();
        this.pager.setPage(1);
        return this.loadPage(params, refresh);
    };

    lastPage(params:any, refresh:boolean = false):Promise<any> {
        this.resetItems();
        this.pager.setPage(this.pager.getPagesAmount());
        return this.loadPage(params, refresh);
    };

    isFirstPage() {
        return this.pager.getPage() === 1;
    };

    isLastPage() {
        return this.pager.getPage() === this.pager.getPagesAmount();
    };
}