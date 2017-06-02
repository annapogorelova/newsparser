import {Component, Input, Output, EventEmitter} from '@angular/core';

@Component({
	selector: 'pagination',
	templateUrl: './pagination.component.html',
	styleUrls: ['./pagination.component.css']
})

export class PaginationComponent {
	@Input() isLastPage: boolean;
	@Input() isFirstPage: boolean;

	@Output() onFirstPage: EventEmitter<any> = new EventEmitter<any>();
	@Output() onNextPage: EventEmitter<any> = new EventEmitter<any>();
	@Output() onLastPage: EventEmitter<any> = new EventEmitter<any>();
	@Output() onPrevPage: EventEmitter<any> = new EventEmitter<any>();

	firstPage(){
		this.onFirstPage.emit();
	};

	nextPage(){
		this.onNextPage.emit();
	};

	lastPage(){
		this.onLastPage.emit();
	};

	prevPage(){
		this.onPrevPage.emit();
	};
}
