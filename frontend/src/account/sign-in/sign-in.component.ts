import {Component, Inject} from '@angular/core';
import {Router} from '@angular/router';
import { LocalStorageService } from 'angular-2-local-storage';
import {AuthService} from '../../shared/services/auth/auth.service';

@Component({
    templateUrl: 'sign-in.component.html',
})
export class SignInComponent  {
    public email = '';
    public password = '';

    constructor(@Inject(Router) private router: Router, private authService: AuthService,
                private localStorageService: LocalStorageService){

    }

    register = () => {
        this.router.navigate(['/register']);
    };

    private handleAuth = (auth: any) => {
        this.localStorageService.set('auth', auth.access_token)
        this.router.navigate(['/news']);
    };

    signIn = () => {
        this.authService.signIn(this.email, this.password).then(auth => this.handleAuth(auth));
    };
}
