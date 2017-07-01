import {Component, Input, Output, EventEmitter} from '@angular/core';

@Component({
    selector: 'refresh-button',
    templateUrl: './refresh-button.component.html',
    styleUrls: ['./refresh-button.component.css']
})

/**
 * A refresh button that executes user refresh handler
 */
export class RefreshButtonComponent {
    /**
     * Event fires user defined refresh handler
     * @type {any}
     */
    @Output() onRefresh:EventEmitter<any> = new EventEmitter<any>();

    /**
     * flag to apply the spinning animation
     * @type {boolean}
     */
    @Input() isSpinning:boolean = false;

    /**
     * Function executes custom refresh handler if specified
     */
    refresh() {
        this.onRefresh.emit({});
    };
}