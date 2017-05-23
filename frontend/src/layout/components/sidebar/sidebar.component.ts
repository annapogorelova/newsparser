import {
	Component,
	EventEmitter,
	Output,
	style,
	state,
	animate,
	transition,
	trigger,
	Input, ViewChild
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
export class SidebarComponent {
    protected animationState: string = 'in';
	@ViewChild('newsSourcesList') newsSourcesList: any;

    @Output() onShow: EventEmitter<any> = new EventEmitter<any>();
    @Output() onHide: EventEmitter<any> = new EventEmitter<any>();
    
    @Output() onSourceSelected: EventEmitter<any> = new EventEmitter<any>();
    @Output() onSourceDeselected: EventEmitter<any> = new EventEmitter<any>();
	@Output() onSourcesCleared: EventEmitter<any> = new EventEmitter<any>();

    @Output() onTagDeselected: EventEmitter<any> = new EventEmitter<any>();
    @Output() onTagsCleared: EventEmitter<any> = new EventEmitter<any>();
    
    @Input() selectedTags: Array<string> = [];
    @Input() selectedSourcesIds: Array<number> = [];
    
    defaultWidth: number = AppSettings.SIDEBAR_WIDTH_PX;

    constructor(){
    }

    show(){
        this.animationState = 'in';
        this.onShow.emit();
    };

    hide(){
        this.animationState = 'out';
        this.onHide.emit();
    };

    isVisible(): boolean {
        return this.animationState === 'in';
    };

    toggle(){
        this.isVisible() ? this.hide() : this.show();
    };
    
    onSelectSource(event: any){
        this.onSourceSelected.emit(event);
    };
    
    onDeselectSource(event: any){
        this.onSourceDeselected.emit(event);
    };

	clearSources(event: any){
		if(!this.selectedSourcesIds.length){
			return;
		}
		this.newsSourcesList.clearSources();
		this.onSourcesCleared.emit(event);
	};

    onDeselectTag = (event: any) => {
        this.onTagDeselected.emit(event);
    };
    
    onClearTags = () => {
        this.onTagsCleared.emit();
    };
}