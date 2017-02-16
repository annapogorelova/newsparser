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
    /**
     * user defined refresh handler
     * @type {any}
     */
    @Input() refreshHandler: any = null;

    /**
     * flag to apply the spinning animation
     * @type {boolean}
     */
    @Input() isSpinning: boolean = false;

    /**
     * Function executes custom refresh handler if specified
     */
    refresh = () => {
        if(this.refreshHandler){
            this.refreshHandler();
        }
    };
}