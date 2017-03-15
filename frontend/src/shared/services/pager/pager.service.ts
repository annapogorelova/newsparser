import {Injectable} from '@angular/core';
import {AppSettings} from '../../../app/app.settings';

/**
 * Class contains paging functionality for lists
 */
@Injectable()
export class PagerService {
    private items: Array<any> = [];
    private page: number;
    private pageSize: number;
    private offset: number;

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
     * @param {number} offset - Data array start index
     */
    reset = (page: number = 1, offset: number = 0) => {
        this.page = page;
        this.offset = offset;
        this.items = [];
    };

    /**
     * Appends new page of items to the list
     * @param {Array<any>} newItems - array of new items
     */
    appendItems = (newItems: Array<any>) => {
        this.items = this.items.concat(newItems);
        this.offset = this.items.length;
        this.setPage(this.calculatePage());
        return this.items;
    };

    /**
     * Get the start index for the next page of list
     * @returns {number} The start index for items to be fetched
     */
    getOffset = () => {
        return this.offset;
    };

    /**
     * Get the pageSize of list's page
     * @returns {number} The pageSize of list's page
     */
    getPageSize = () => {
        return this.pageSize;
    };

    /**
     * Get the items array
     * @returns {Array<any>} Items array
     */
    getItems = () => {
        return this.items;
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
     * Get the page number (1, 2 etc.)
     * @returns {number} Page number
     */
    calculatePage = () => {
        return Math.ceil(this.items.length/this.pageSize);
    };

    /**
     * Calculate the page pageSize based on the page specified
     */
    calculatePageSize = (page: number = 1) => {
        return page * this.pageSize;
    };
}