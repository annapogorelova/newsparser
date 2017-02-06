import {Component, Inject, HostListener, Input} from '@angular/core';
import { DOCUMENT } from '@angular/platform-browser';
import {AppSettings} from '../../../app/app.settings';

@Component({
    selector: 'go-top-button',
    templateUrl: './go-top-button.component.html',
    styles: [require('./go-top-button.component.css').toString()]
})

export class GoTopComponent {
    @Input() scrollYDistance: number = AppSettings.DEFAULT_GO_TOP_HEIGHT;

    public displayGoTopButton: boolean = false;

    constructor(@Inject(DOCUMENT) private document: Document){}

    @HostListener("window:scroll", [])
    onWindowScroll = () => {
        let number = this.document.body.scrollTop;
        if (number > this.scrollYDistance) {
            this.displayGoTopButton = true;
        } else if(this.displayGoTopButton){
            this.displayGoTopButton = false;
        }
    }

    scrollTop = (event: any) =>{
        event.preventDefault();
        window.scrollTo(0, 0);
    }
}