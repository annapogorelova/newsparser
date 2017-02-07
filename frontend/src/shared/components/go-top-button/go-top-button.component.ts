import {Component, HostListener, Input, trigger, state, style, transition, animate} from '@angular/core';
import {AppSettings} from '../../../app/app.settings';

@Component({
    selector: 'go-top-button',
    templateUrl: './go-top-button.component.html',
    styles: [require('./go-top-button.component.css').toString()],
    animations: [
        trigger('appearInOut', [
            state('in', style({
                'display': 'block',
                'opacity': '0.85'
            })),
            state('out', style({
                'display': 'none',
                'opacity': '0'
            })),
            transition('in => out', animate('400ms ease-in-out')),
            transition('out => in', animate('400ms ease-in-out'))
        ]),
    ],
})

export class GoTopComponent {
    private animationState: string = 'out';

    @Input() scrollYDistance: number = AppSettings.DEFAULT_GO_TOP_HEIGHT;

    @HostListener("window:scroll", [])
    onWindowScroll = () => {
        this.animationState = document.body.scrollTop > this.scrollYDistance ? 'in' : 'out';
    };

    scrollTop = (event: any) => {
        event.preventDefault();
        window.scrollTo(0, 0);
    };
}