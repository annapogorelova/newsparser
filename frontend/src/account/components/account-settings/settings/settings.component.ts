import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {
    NavigatorService,
    AuthService,
    PageTitleService
} from '../../../../shared';

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
    user:any;

    constructor(private route:ActivatedRoute,
                private authService:AuthService,
                private navigator:NavigatorService,
                private pageTitleService:PageTitleService) {
    }

    ngOnInit() {
        this.route.fragment.subscribe((fragment:string) => this.setActiveTab(fragment));
        this.loadUser(true);
        this.pageTitleService.appendTitle('Account');
    };

    setActiveTab(fragment:string) {
        this.selectedTabId = fragment || this.tabsIds[0];
        this.navigator.navigate([], {fragment: this.selectedTabId});
    };

    onTabChange(event:any) {
        this.loadUser(true);
        this.selectedTabId = event.nextId;
        this.navigator.navigate([], {fragment: this.selectedTabId});
    };

    loadUser(refresh:boolean = false) {
        this.authService.loadUser(refresh).then((data:any) => this.initializeFormData(data));
    };

    initializeFormData(data:any) {
        this.user = data;
    };
}