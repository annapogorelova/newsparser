import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";

@Component({
    templateUrl: 'settings.component.html',
    styleUrls: ['settings.component.css'],
    selector: 'account-settings'
})

/**
 * Component contains child components for editing the account in its template
 */
export class AccountSettingsComponent implements OnInit{
    selectedTabId: string;

    constructor(private route: ActivatedRoute){}

    ngOnInit(){
        this.route.fragment
            .subscribe((fragment: string) => this.selectedTabId = fragment);
    }
}