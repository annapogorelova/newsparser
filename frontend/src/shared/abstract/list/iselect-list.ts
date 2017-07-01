import {EventEmitter} from '@angular/core';

/**
 * An interface that contains the methods declarations for the feed channel select lists
 */
export interface ISelectList {
    select(source:any):void;
    deselect(source:any):void;
    isSelected(source:any):boolean;

    onSelect:EventEmitter<any>;
    onDeselect:EventEmitter<any>;
}