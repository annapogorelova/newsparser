import {Component, Input} from '@angular/core';
import {AppSettings} from '../../../app/app.settings';

@Component({
    selector: 'search',
    templateUrl: 'search.component.html',
    styles: [require('./search.component.css').toString()]
})

/**
 * Component for handling the new sources search
 */
export class SearchComponent {
    public search: string = null;

    constructor(){}

    @Input()
    private searchCallback: any = null;

    @Input()
    private placeholderCaption: string = AppSettings.DEFAULT_SEARCH_PLACEHOLDER_TEXT;

    onKeyUp = (event: any) => {
        // Prevent action triggering when user hits functional buttons
        if(event.keyCode > 8 && event.keyCode < 48){
            return;
        }

        if(this.searchCallback){
            this.searchCallback(this.search);
        }
    };
}