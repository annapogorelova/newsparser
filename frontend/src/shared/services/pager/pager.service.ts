import {Injectable} from '@angular/core';
import {AppSettings} from '../../../app/app.settings';

/**
 * Class contains paging functionality for lists
 */
@Injectable()
export class PagerService {
    private items: Array<any> = [];
    private pageNumber: number;
    private pageSize: number;

    /**
     * Initializes a pager
     * @param {number} pageNumber - Initial page number for list
     * @param {number} pageSize - Size of the list's page
     */
    constructor(pageNumber?: number, pageSize?: number){
        this.reset(pageNumber, pageSize);
    };

    /**
     * Resets a pager
     * @param {number} pageNumber - Initial page index for list
     * @param {number} pageSize - Size of the list's page
     */
    reset = (pageNumber: number = 1, pageSize: number = AppSettings.DEFAULT_PAGE_SIZE) => {
        this.pageNumber = pageNumber;
        this.pageSize = pageSize;
        this.items = [];
    };

    /**
     * Appends new page of items to the list
     * @param {Array<any>} newItems - array of new items
     */
    appendItems = (newItems: Array<any>) => {
        this.items = this.items.concat(newItems);
        this.pageNumber = this.getPageNumber();
    };

    /**
     * Get the start index for the next page of list
     * @returns {number} The start index for items to be fetched
     */
    getNextPageStartIndex = () => {
        return this.pageNumber * this.pageSize;
    };

    /**
     * Get the number of items to preload based on current page and current number of items
     * Ex.: User requests page 5, but items.length is 10, page size =>
     * we need to preload 5*10 - 10 = 40 items to reach the 5th page
     * @returns {number}
     */
    getNumberOfItemsToPreload = () => {
        return this.pageNumber * this.pageSize - this.items.length;
    };

    /**
     * Get the size of list's page
     * @returns {number} The size of list's page
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
     * Get the page number (1, 2 etc.)
     * @returns {number} Page number
     */
    getPageNumber = () => {
        return this.items.length ? Math.ceil(this.items.length/this.pageSize) : 1;
    };

    /**
     * Sets the page number
     */
    setPage = (page: number) => {
        this.pageNumber = page;
    };
}