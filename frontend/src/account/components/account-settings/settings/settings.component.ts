import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {NavigatorService} from '../../../../shared';

@Component({
    templateUrl: 'settings.component.html',
    styleUrls: ['settings.component.css'],
    selector: 'settings'
})

/**
 * Component contains child components for editing the account in its template
 */
export class AccountSettingsComponent implements OnInit {
    selectedTabId:string;
    tabsIds:Array<string> = ['profile', 'password'];

    constructor(private route:ActivatedRoute, private navigator:NavigatorService) {
    }

    ngOnInit() {
        this.route.fragment.subscribe((fragment:string) => this.setActiveTab(fragment));
    };

    setActiveTab(fragment:string) {
        this.selectedTabId = fragment || this.tabsIds[0];
        this.navigator.navigate([], {fragment: this.selectedTabId});
    };
}