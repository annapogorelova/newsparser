import {Injectable} from '@angular/core';
import {AppSettings} from '../../../app/app.settings';

/**
 * Class contains paging functionality for lists
 */
@Injectable()
export class PagerService {
    private page: number;
    private pageSize: number;
    private total?: number;

    /**
     * Initializes a pager
     * @param {number} pageNumber - Initial page number for list
     * @param {number} pageSize - pageSize of the list's page
     */
    constructor(page?: number, pageSize?: number){
        this.reset(page);
        this.pageSize = pageSize || AppSettings.DEFAULT_PAGE_SIZE;
    };

    /**
     * Resets a pager
     * @param {number} page - Initial page index for list
     */
    reset = (page: number = 1) => {
        this.page = page;
    };

    /**
     * Get total amount of list items
     * @returns {number}
     */
    getTotal = () => {
        return this.total;
    };

    /**
     * Set total amount of list items
     * @param total
     */
    setTotal = (total: number) => {
        this.total = total;
    };

    /**
     * Get the list offset for the next page
     * @returns {number} The start index for items to be fetched
     */
    getOffset = () => {
        return this.page === 1 ? 0 : (this.page - 1) * this.pageSize;
    };

    /**
     * Get the pageSize of list's page
     * @returns {number} The pageSize of list's page
     */
    getPageSize = () => {
        return this.pageSize;
    };

    /**
     * Sets the page number
     */
    setPage = (page: number) => {
        this.page = page;
    };

    /**
     * Returns the page number
     * @returns {number}
     */
    getPage = () => {
        return this.page;
    };

    /**
     * Get the number of pages available (only if total is present)
     * @returns {number}
     */
    getPagesAmount = () => {
        if(this.total !== undefined){
            return Math.ceil(this.total/this.pageSize);
        }
    };
}