import {BaseList} from './base-list';
import {ApiService} from '../../services/api/api.service';
import {PagerService} from '../../services/pager/pager.service';

/**
 * Base list class with pagination
 */
export abstract class PagedList extends BaseList {
    constructor(apiService: ApiService, pager: PagerService, protected apiRoute: string){
        super(apiService, pager);
    }

    loadPage = (params: any) => {
        this.resetItems();
        return this.loadData(params);
    };

    reloadPage(params: any){
        this.resetItems();
        return this.reloadData(params, true);
    };

    nextPage(params: any){
        this.resetItems();
        this.pager.setPage(this.pager.getPage() + 1);
        this.loadPage(params);
    };

    prevPage(params: any) {
        this.resetItems();
        this.pager.setPage(this.pager.getPage() - 1);
        this.loadPage(params);
    };

    isFirstPage = () => {
        return this.pager.getPage() === 1;
    };

    isLastPage = () => {
        return this.pager.getPage() === this.pager.getPagesAmount();
    };
}