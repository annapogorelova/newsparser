import {Component, Input} from '@angular/core';

@Component({
    selector: 'refresh-button',
    templateUrl: './refresh-button.component.html',
    styles: [require('./refresh-button.component.css').toString()]
})

/**
 * A refresh button that executes user refresh handler
 */
export class RefreshButtonComponent {
    @Input() refreshHandler: any = null;

    /**
     * Function executes custom refresh handler if specified
     */
    refresh = () => {
        if(this.refreshHandler){
            this.refreshHandler();
        }
    };
}