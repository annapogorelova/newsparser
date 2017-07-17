import {Component, Input} from '@angular/core';

@Component({
    selector: 'loading',
    templateUrl: './loading.component.html'
})

/**
 * A loading animation component
 */
export class LoadingComponent {

    /**
     * Size in px of the loading icon
     */
    @Input() sizePx:number = 50;
}