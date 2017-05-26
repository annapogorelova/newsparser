import {Component, Input, Output, EventEmitter, ViewChild} from '@angular/core';

@Component({
	selector: 'subscription-item',
	templateUrl: './subscription-item.component.html',
	styleUrls: ['./subscription-item.component.css']
})
export class SubscriptionItemComponent {
	@Input() source: any;
	@Input() popContent: any;
	@Input() isSelected: boolean;

	@Output() onClick: EventEmitter<any> = new EventEmitter<any>();

	@ViewChild("popover") popover: any;
	
	handleClick(){
		this.onClick.emit({source: this.source, popover: this.popover});
	};
}