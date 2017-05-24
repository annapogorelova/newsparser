import {Component, Input, Output, EventEmitter} from '@angular/core';
import {AppSettings} from '../../../app/app.settings';

@Component({
    selector: 'search',
    templateUrl: 'search.component.html',
    styleUrls: ['./search.component.css']
})

/**
 * Component for handling the new sources search
 */
export class SearchComponent {
    constructor(){}

    @Input()
    private search: string = null;

    @Input()
    private placeholderCaption: string = AppSettings.DEFAULT_SEARCH_PLACEHOLDER_TEXT;

    @Output() onSearch: EventEmitter<any> = new EventEmitter<any>();
    @Output() onClear: EventEmitter<any> = new EventEmitter<any>();

    onKeyUp(event: any) {
        // Prevent action triggering when user hits functional buttons
        if(event.keyCode > 8 && event.keyCode < 48){
            return;
        }

        this.onSearch.emit({inputValue: this.search});
    };

    clear(){
        this.search = null;
        this.onClear.emit();
    };
}