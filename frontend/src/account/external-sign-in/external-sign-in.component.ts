import {Component, Input, Output, EventEmitter} from '@angular/core';
import {AuthService} from '../../shared/services/auth/auth.service';
import * as SocialAuthService from "angular2-social-login";

@Component({
    templateUrl: './external-sign-in.component.html',
    styleUrls: ['./external-sign-in.component.css'],
    selector: 'external-sign-in'
})

/**
 * Component for signing in via external auth providers
 */
export class ExternalSignInComponent  {
    @Input() provider: string;
    @Input() buttonIcon: string;
    @Input() buttonClass: string;

    @Output() onSignedIn: EventEmitter<any> = new EventEmitter<any>();

    constructor(private authService: AuthService,
                public socialAuthService: SocialAuthService.AuthService){
    }

    signIn = () => {
        this.socialAuthService.login(this.provider).subscribe(data => this.handleExternalAuth(data));
    };

    private handleExternalAuth = (data: any) => {
        this.authService.externalSignIn(data['token'], this.provider).then(auth => this.handleAuth(auth));
    };

    private handleAuth = (auth: any) => {
        this.onSignedIn.emit(auth);
    };
}
