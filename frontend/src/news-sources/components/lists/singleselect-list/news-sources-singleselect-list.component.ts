import {Component, Output, EventEmitter, Inject} from '@angular/core';
import {BaseNewsSourcesListComponent} from '../base-news-sources-list';
import {AbstractDataProviderService, PagerServiceProvider, ISelectList} from '../../../../shared';

@Component({
	selector: 'news-sources-singleselect-list',
	templateUrl: './news-sources-singleselect-list.component.html',
	styleUrls: ['./news-sources-singleselect-list.component.css']
})
export class NewsSourcesSingleSelectList
	extends BaseNewsSourcesListComponent
	implements ISelectList {

	protected apiRoute: string = 'newsSources';
	selectedSource: any = null;
	currentPopover: any;

	@Output() onSelect: EventEmitter<any> = new EventEmitter<any>();
	@Output() onDeselect: EventEmitter<any> = new EventEmitter<any>();

	constructor(protected dataProvider: AbstractDataProviderService,
	            @Inject(PagerServiceProvider) pagerProvider:PagerServiceProvider){
		super(dataProvider, pagerProvider);
	}

	ngOnInit(){
		this.loadData(super.getRequestParams(), true);
	};

	select(source:any): void {
		this.selectedSource = source;
	};

	deselect(source:any): void {
		this.selectedSource = source;
	};

	handleSubscription() {
		this.onSelect.emit({source: this.selectedSource});
	};

	handleUnsubscription() {
		this.onDeselect.emit({source: this.selectedSource});
	};

	isSelected(source:any): boolean {
		return this.selectedSource && this.selectedSource.id === source.id;
	};

	reload(): Promise<any> {
		return super.reload().then(() => this.onReload());
	};

	onReload = () => {
		this.selectedSource = null;
	};

	setCurrentSource(event: any){
		// if the source is selected first time or the selected source changed
		// close the existing popup
		if(!this.selectedSource || event.source.id !== this.selectedSource.id){
			this.selectedSource = event.source;
			if(this.currentPopover && this.currentPopover.isOpen()){
				this.currentPopover.close();
			}
		} else if(this.selectedSource && event.source.id === this.selectedSource.id){
			this.selectedSource = null;
		}

		this.currentPopover = event.popover;
		this.currentPopover.isOpen() ? this.currentPopover.close() : this.currentPopover.open();
	};
	
	hideSubscriptionInfo(){
		if(this.selectedSource){
			this.selectedSource = null;
		}
		
		if(this.currentPopover){
			this.currentPopover.isOpen() && this.currentPopover.close();
			this.currentPopover = null;
		}	
	};
}