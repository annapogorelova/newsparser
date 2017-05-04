import {Component, Input, Output, EventEmitter} from '@angular/core';

@Component({
    selector: 'tags-list',
    templateUrl: 'tags-list.component.html',
    styleUrls: ['./tags-list.component.css']
})

/**
 * Component for displaying the list of selected tags
 */
export class TagListComponent {
    @Input() tags: Array<any> = [];

    /**
     * Tag deselect event
     * @type {EventEmitter<any>}
     */
    @Output() onDeselect: EventEmitter<any> = new EventEmitter<any>();

    /**
     * Tags list clear event
     * @type {EventEmitter<any>}
     */
    @Output() onClear: EventEmitter<any> = new EventEmitter<any>();

    deselectTag = (tag: any) => {
        this.onDeselect.emit({tag: tag});
    };

    clearTags = () => {
        if(!this.tags.length){
            return;
        }
        this.onClear.emit();
    };
}