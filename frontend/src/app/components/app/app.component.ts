import {Component, HostListener, ViewChild, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {
    AuthService,
    AuthProviderService,
    PageTitleService
} from '../../../shared';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
    isNavbarCollapsed = true;
    minContentHeight:number;
    appName:string = process.env.APP_NAME;

    @ViewChild('appContent') appContent:any;

    constructor(private authService:AuthService,
                private authProvider:AuthProviderService,
                private router:Router,
                private pageTitleService:PageTitleService) {
    }

    ngOnInit() {
        this.authService.loadUser(true);

        this.router.events.subscribe(() => {
            this.isNavbarCollapsed = true;
        });

        this.pageTitleService.setBaseTitle(process.env.APP_NAME);
    };

    signOut() {
        this.authService.signOut().then(() => this.onSignedOut());
    };

    onSignedOut() {
        this.router.navigate(['/sign-in']);
    };
}
