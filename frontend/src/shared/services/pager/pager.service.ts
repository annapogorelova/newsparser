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
    reset = (pageNumber?: number, pageSize?: number) => {
        this.pageNumber = pageNumber || 0;
        this.pageSize = pageSize || AppSettings.DEFAULT_PAGE_SIZE;
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
        return Math.ceil(this.items.length/this.pageSize);
    };
}