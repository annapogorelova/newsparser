import {
    Component,
    EventEmitter,
    Output,
    Input,
    ViewChild,
    OnInit,
    style,
    state,
    animate,
    transition,
    trigger
} from '@angular/core';
import {AppSettings} from '../../../app/app.settings';

@Component({
    selector: 'sidebar',
    templateUrl: './sidebar.component.html',
    styleUrls: ['./sidebar.component.css'],
    animations: [
        trigger('slideInOut', [
            state('in', style({
                'transform': 'translate3d(0, 0, 0)'
            })),
            state('out', style({
                'transform': 'translate3d(-100%, 0, 0)'
            })),
            transition('in => out', animate('200ms ease-in-out')),
            transition('out => in', animate('200ms ease-in-out'))
        ]),
        trigger('appearInOut', [
            state('in', style({
                'display': 'block',
                'opacity': '0.85',
            })),
            state('out', style({
                'display': 'none',
                'opacity': '0',
            })),
            transition('in => out', animate('200ms ease-in-out')),
            transition('out => in', animate('200ms ease-in-out'))
        ])
    ]
})
export class SidebarComponent implements OnInit {
    protected animationState:string = 'in';
    @ViewChild('channelsList') channelsList:any;

    @Output() onShow:EventEmitter<any> = new EventEmitter<any>();
    @Output() onHide:EventEmitter<any> = new EventEmitter<any>();

    @Output() onChannelSelected:EventEmitter<any> = new EventEmitter<any>();
    @Output() onChannelDeselected:EventEmitter<any> = new EventEmitter<any>();
    @Output() onChannelsCleared:EventEmitter<any> = new EventEmitter<any>();

    @Output() onTagDeselected:EventEmitter<any> = new EventEmitter<any>();
    @Output() onTagsCleared:EventEmitter<any> = new EventEmitter<any>();

    @Input() selectedTags:Array<string> = [];
    @Input() selectedChannelsIds:Array<number> = [];

    defaultWidth:number = AppSettings.SIDEBAR_WIDTH_PX;

    constructor() {
    }

    ngOnInit() {
        if (window.innerWidth <= AppSettings.MAX_DEVICE_WIDTH_PX) {
            this.hide();
        } else {
            this.show();
        }
    }

    show() {
        this.animationState = 'in';
        this.onShow.emit();
    };

    hide() {
        this.animationState = 'out';
        this.onHide.emit();
    };

    isVisible():boolean {
        return this.animationState === 'in';
    };

    toggle() {
        this.isVisible() ? this.hide() : this.show();
    };

    onSelectChannel(event:any) {
        this.onChannelSelected.emit(event);
    };

    onDeselectChannel(event:any) {
        this.onChannelDeselected.emit(event);
    };

    clearChannels(event:any) {
        if (!this.selectedChannelsIds.length) {
            return;
        }
        this.channelsList.clearChannels();
        this.onChannelsCleared.emit(event);
    };

    onDeselectTag = (event:any) => {
        this.onTagDeselected.emit(event);
    };

    onClearTags = () => {
        this.onTagsCleared.emit();
    };
}