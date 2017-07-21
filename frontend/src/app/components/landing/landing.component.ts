import {Component, OnInit} from '@angular/core';
import {AuthProviderService, NavigatorService} from '../../../shared';

@Component({
    templateUrl: './landing.component.html',
    styleUrls: ['./landing.component.css']
})
export class LandingComponent implements OnInit {
    constructor(private authProvider:AuthProviderService,
                private navigator:NavigatorService) {
    }

    ngOnInit() {
        if(this.authProvider.hasAuth()) {
            this.navigator.navigate(['/feed']);
        }
    }
}