import {Component} from '@angular/core';
import {AuthService} from '../../../shared/services/auth/auth.service';
import {AuthProviderService} from '../../../shared/services/auth/auth-provider.service';
import {Router} from "@angular/router";

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {
    public isNavbarCollapsed = true;
    public minContentHeight:number;

    constructor(private authService: AuthService,
                private authProvider: AuthProviderService,
                private router: Router) {
    }

    ngOnInit() {
        this.minContentHeight = window.screen.height;
        this.authService.loadUser(true);
    }

    signOut = () => {
        this.authService.signOut().then(() => this.router.navigate(['/sign-in']));
    };
}
