import {Component, Output, EventEmitter, Inject, Input, OnInit} from '@angular/core';
import {ISelectList, AbstractDataProviderService, PagerServiceProvider} from '../../../../shared';
import {BaseChannelsListComponent} from '../base-channels-list';

@Component({
    selector: 'channels-multiselect-list',
    templateUrl: './channels-multiselect-list.component.html',
    styleUrls: ['./channels-multiselect-list.component.css']
})
export class ChannelsMultiSelectList
    extends BaseChannelsListComponent
    implements ISelectList, OnInit {

    protected apiRoute:string = 'channels';
    protected onlySubscribed:boolean;

    selectedChannelsIds:Array<any> = [];

    @Input() subscribed:boolean;

    @Output() onSelect:EventEmitter<any> = new EventEmitter<any>();
    @Output() onDeselect:EventEmitter<any> = new EventEmitter<any>();

    constructor(protected dataProvider:AbstractDataProviderService,
                @Inject(PagerServiceProvider) pagerProvider:PagerServiceProvider) {
        super(dataProvider, pagerProvider);
    }

    ngOnInit() {
        this.onlySubscribed = this.subscribed;
        super.resetPage().then(() => this.handleLoadedChannels());
    };

    handleLoadedChannels() {
        this.selectedChannelsIds = this.selectedChannelsIds.concat(this.initiallySelectedChannelsIds);
    };

    handleSelection(source:any) {
        if (this.isSelected(source)) {
            this.deselect(source);
        } else {
            this.select(source);
        }
    };

    select(channel:any):void {
        this.selectedChannelsIds.push(channel.id);
        this.onSelect.emit({channel: channel});
    };

    deselect(channel:any):void {
        this.selectedChannelsIds = this.selectedChannelsIds.filter(function (item) {
            return item !== channel.id;
        });
        this.onDeselect.emit({channel: channel});
    };

    isSelected(channel:any):boolean {
        return this.selectedChannelsIds.indexOf(channel.id) !== -1;
    };

    reset():Promise<any> {
        return super.resetPage().then(() => this.onReset());
    };

    onReset() {
        this.selectedChannelsIds = [];
    };

    clearChannels() {
        this.selectedChannelsIds = [];
    };
}