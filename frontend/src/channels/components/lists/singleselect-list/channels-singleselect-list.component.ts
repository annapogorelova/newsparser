import {Component, Output, Input, EventEmitter, Inject, OnInit} from '@angular/core';
import {BaseChannelsListComponent} from '../base-channels-list';
import {AbstractDataProviderService, PagerServiceProvider, ISelectList} from '../../../../shared';

@Component({
    selector: 'channels-singleselect-list',
    templateUrl: './channels-singleselect-list.component.html',
    styleUrls: ['./channels-singleselect-list.component.css']
})
export class ChannelsSingleSelectList
    extends BaseChannelsListComponent
    implements ISelectList, OnInit {

    protected apiRoute:string = 'channels';
    protected onlySubscribed:boolean;

    selectedChannel:any = null;
    currentPopover:any;

    @Input() subscribed:boolean;

    @Output() onSelect:EventEmitter<any> = new EventEmitter<any>();
    @Output() onDeselect:EventEmitter<any> = new EventEmitter<any>();

    constructor(protected dataProvider:AbstractDataProviderService,
                @Inject(PagerServiceProvider) pagerProvider:PagerServiceProvider) {
        super(dataProvider, pagerProvider);
    }

    ngOnInit() {
        this.onlySubscribed = this.subscribed;
        super.resetPage();
    };

    select(channel:any):void {
        this.selectedChannel = channel;
    };

    deselect(channel:any):void {
        this.selectedChannel = channel;
    };

    handleSubscription() {
        this.onSelect.emit({channel: this.selectedChannel});
    };

    handleUnsubscription() {
        this.onDeselect.emit({channel: this.selectedChannel});
    };

    isSelected(channel:any):boolean {
        return this.selectedChannel && this.selectedChannel.id === channel.id;
    };

    setCurrentChannel(event:any) {
        // if the channel is selected first time or the selected channel changed
        // close the existing popup
        if (!this.selectedChannel || event.channel.id !== this.selectedChannel.id) {
            this.selectedChannel = event.channel;
            if (this.currentPopover && this.currentPopover.isOpen()) {
                this.currentPopover.close();
            }
        } else if (this.selectedChannel && event.channel.id === this.selectedChannel.id) {
            this.selectedChannel = null;
        }

        this.currentPopover = event.popover;
        this.currentPopover.isOpen() ? this.currentPopover.close() : this.currentPopover.open();
    };

    hideSubscriptionInfo() {
        if (this.selectedChannel) {
            this.selectedChannel = null;
        }

        if (this.currentPopover) {
            this.currentPopover.isOpen() && this.currentPopover.close();
            this.currentPopover = null;
        }
    };

    updateSubscriptionState(channelId:number, isSubscribed:boolean) {
        var channels = this.items.filter(function (c) {
            return c.id === channelId;
        });

        if (channels.length) {
            channels[0].isSubscribed = isSubscribed;
        }
    };

    nextPage():Promise<any> {
        return super.nextPage(true);
    };

    prevPage():Promise<any> {
        return super.prevPage(true);
    };

    firstPage():Promise<any> {
        return super.firstPage(true);
    };

    lastPage():Promise<any> {
        return super.lastPage(true);
    };
}