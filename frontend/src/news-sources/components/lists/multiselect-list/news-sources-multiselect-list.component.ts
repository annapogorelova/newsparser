import {Component, Output, EventEmitter, Inject, Input} from '@angular/core';
import {ISelectList, AbstractDataProviderService, PagerServiceProvider} from '../../../../shared';
import {BaseNewsSourcesListComponent} from '../base-news-sources-list';

@Component({
	selector: 'news-sources-multiselect-list',
	templateUrl: './news-sources-multiselect-list.component.html',
	styleUrls: ['./news-sources-multiselect-list.component.css']
})
export class NewsSourcesMultiSelectList
	extends BaseNewsSourcesListComponent
	implements ISelectList {

	protected apiRoute: string = 'newsSources';
	protected onlySubscribed: boolean;

	selectedSourcesIds: Array<any> = [];

	@Input() subscribed: boolean;

	@Output() onSelect: EventEmitter<any> = new EventEmitter<any>();
	@Output() onDeselect: EventEmitter<any> = new EventEmitter<any>();

	constructor(protected dataProvider: AbstractDataProviderService,
	            @Inject(PagerServiceProvider) pagerProvider:PagerServiceProvider){
		super(dataProvider, pagerProvider);
	}

	ngOnInit(){
		this.onlySubscribed = this.subscribed;
		super.resetPage().then(() => this.handleLoadedNewsSources());
	};

	handleLoadedNewsSources() {
		this.selectedSourcesIds = this.selectedSourcesIds.concat(this.initiallySelectedSourcesIds);
	};

	handleSelection(source: any){
		if(this.isSelected(source)) {
			this.deselect(source);
		} else {
			this.select(source);
		}
	};

	select(source:any): void {
		this.selectedSourcesIds.push(source.id);
		this.onSelect.emit({source: source});
	};

	deselect(source:any): void {
		this.selectedSourcesIds = this.selectedSourcesIds.filter(function (item) {
			return item !== source.id;
		});
		this.onDeselect.emit({source: source});
	};

	isSelected(source:any): boolean {
		return this.selectedSourcesIds.indexOf(source.id) !== -1;
	};

	reset(): Promise<any> {
		return super.resetPage().then(() => this.onReset());
	};

	onReset() {
		this.selectedSourcesIds = [];
	};

	clearSources() {
		this.selectedSourcesIds = [];
	};
}