import {Component, Input, Output, EventEmitter} from '@angular/core';

@Component({
    selector: 'news-source-item',
    templateUrl: './news-source-item.component.html'
})
export class NewsSourceListItemComponent {
    @Input() source:any;
    @Input() isSelected:boolean;

    @Output() onClick:EventEmitter<any> = new EventEmitter<any>();

    handleClick() {
        this.onClick.emit({source: this.source});
    };
}