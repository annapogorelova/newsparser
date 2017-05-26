import {BaseList} from './base-list';
import {PagerService, AbstractDataProviderService} from '../../services';

/**
 * Base list class with pagination
 */
export abstract class PagedList extends BaseList {
    constructor(protected dataProvider: AbstractDataProviderService,
                pager: PagerService, protected apiRoute: string){
        super(dataProvider, pager);
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