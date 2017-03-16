import {Component, Input, Output, EventEmitter} from '@angular/core';

@Component({
    selector: 'tags-list',
    templateUrl: 'tags-list.component.html',
    styles: [require('./tags-list.component.css').toString()]
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
     * Tag text input event
     * @type {EventEmitter<any>}
     */
    @Output() onTagAdded: EventEmitter<any> = new EventEmitter<any>();

    /**
     * Input tag name
     * @type {any}
     */
    public tagName: string = null;

    deselectTag = (tag: any) => {
        this.onDeselect.emit({tag: tag});
    };

    onTagEnter = () => {
        this.onTagAdded.emit(this.tagName);
        this.tagName = null;
    };
}