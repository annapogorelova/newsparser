import {Component, Inject} from '@angular/core';
import {Router} from '@angular/router';
import {AuthService} from '../../../shared/services/auth/auth.service';
import {CacheService} from '../../../shared/services/cache/cache.service';

@Component({
    templateUrl: 'sign-in.component.html',
    styleUrls: ['./sign-in.component.css'],
    selector: 'sign-in'
})
export class SignInComponent  {
    public email: string;
    public password: string;
    public signinInProgress: boolean;
    public signinFailed: boolean;
    public errorMessage: string;

    constructor(@Inject(Router) private router: Router,
                private authService: AuthService,
                private cacheService: CacheService){

    }

    handleAuth = (auth: any) => {
        this.signinInProgress = false;
        this.cacheService.set('auth', auth.access_token);
        this.router.navigate(['/news']);
    };

    handleFailedAuth = (error: Error) => {
        this.signinInProgress = false;
        this.signinFailed = true;
        this.errorMessage = error.message;
    };

    signIn = () => {
        this.signinInProgress = true;
        this.authService.signIn(this.email, this.password)
            .then(auth => this.handleAuth(auth))
            .catch(error => this.handleFailedAuth(error));
    };
}
