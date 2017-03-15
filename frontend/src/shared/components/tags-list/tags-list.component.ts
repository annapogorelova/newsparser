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

    @Output() onDeselect: EventEmitter<any> = new EventEmitter<any>();

    deselectTag = (tag: any) => {
        this.onDeselect.emit({tag: tag});
    };
}