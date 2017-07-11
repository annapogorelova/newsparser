import {Component, HostListener, ViewChild} from '@angular/core';
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
export class AppComponent {
    isNavbarCollapsed = true;
    minContentHeight:number;
    @ViewChild('appContent') appContent:any;

    constructor(private authService:AuthService,
                private authProvider:AuthProviderService,
                private router:Router,
                private pageTitleService:PageTitleService) {
    }

    ngOnInit() {
        this.setAppMinContentHeight();
        this.authService.loadUser(true);

        this.router.events.subscribe(() => {
            this.isNavbarCollapsed = true;
        });

        this.pageTitleService.setBaseTitle(process.env.APP_NAME);
    };

    @HostListener('window:resize')
    onWindowResize() {
        this.setAppMinContentHeight();
    };

    calculateMinContentHeight() {
        // Height of top and bottom navbars is 38, padding - 8 top and bottom
        // 39 + 16*2 = 71
        return window.innerHeight - 71;
    };

    setAppMinContentHeight() {
        this.minContentHeight = this.calculateMinContentHeight();
        this.appContent.nativeElement.style['min-height'] = this.minContentHeight + 'px';
    };

    signOut() {
        this.authService.signOut().then(() => this.router.navigate(['/sign-in']));
    };
}
