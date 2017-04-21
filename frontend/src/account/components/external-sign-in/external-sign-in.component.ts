import {Component, Input, Output, EventEmitter} from '@angular/core';
import {AuthService} from '../../../shared/services/auth/auth.service';
import {ExternalAuthService} from '../../../shared/modules/external-auth/external-auth.service';

@Component({
    templateUrl: 'external-sign-in.component.html',
    styleUrls: ['external-sign-in.component.css'],
    selector: 'external-sign-in'
})

/**
 * Component for signing in via external auth providers
 */
export class ExternalSignInComponent  {
    public signinInProgress: boolean;
    public signInFailed: boolean;
    public errorMessage: string;

    @Input() provider: string;
    @Input() buttonIcon: string;
    @Input() buttonClass: string;

    @Output() onSignedIn: EventEmitter<any> = new EventEmitter<any>();

    constructor(private authService: AuthService,
                public externalAuthService: ExternalAuthService){
    }

    signIn = () => {
        this.externalAuthService.login(this.provider)
            .then(data => this.handleExternalAuth(data));
    };

    private handleExternalAuth = (data: any) => {
        this.signinInProgress = true;
        this.authService.externalSignIn(data['token'], this.provider)
            .then(auth => this.handleAuth(auth))
            .catch(error => this.handleFailedAuth(error));
    };

    private handleFailedAuth = (error: any) => {
        this.signinInProgress = false;
        this.signInFailed = true;
        this.errorMessage = error.message;
    };

    private handleAuth = (auth: any) => {
        this.signinInProgress = false;
        this.signInFailed = false;
        this.errorMessage = '';
        this.onSignedIn.emit(auth);
    };
}
