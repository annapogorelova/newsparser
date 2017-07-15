import {Component, Input, Output, EventEmitter} from '@angular/core';

@Component({
    selector: 'channel-item',
    templateUrl: './channel-item.component.html',
    styleUrls: ['./channel-item.component.css']
})
export class ChannelItemComponent {
    @Input() channel:any;
    @Input() isSelected:boolean;

    @Output() onClick:EventEmitter<any> = new EventEmitter<any>();

    handleClick() {
        this.onClick.emit({channel: this.channel});
    };
}