import {Component, Input, Output, EventEmitter, ViewChild} from '@angular/core';

@Component({
    selector: 'subscription-item',
    templateUrl: './subscription-item.component.html',
    styleUrls: ['./subscription-item.component.css']
})
export class SubscriptionItemComponent {
    @Input() channel:any;
    @Input() popContent:any;
    @Input() isSelected:boolean;

    @Output() onClick:EventEmitter<any> = new EventEmitter<any>();

    @ViewChild('popover') popover:any;

    handleClick() {
        this.onClick.emit({channel: this.channel, popover: this.popover});
    };
    
    getSubscribersCountTitle(count:number) {
        if(count === 0) {
            return 'No subscribers';
        }

        return count === 1 ? '1 subscriber' : `${count} subscribers`;
    };
}