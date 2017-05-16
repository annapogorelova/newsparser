import {Component, Input, Output, EventEmitter} from '@angular/core';
import {AuthService} from '../../../shared/services/auth/auth.service';
import {ExternalAuthService} from '../../../shared/modules/external-auth/external-auth.service';
import {IForm} from '../../../shared/abstract/base-form/base-form';

@Component({
    templateUrl: 'external-sign-in.component.html',
    styleUrls: ['external-sign-in.component.css'],
    selector: 'external-sign-in'
})

/**
 * Component for signing in via external auth providers
 */
export class ExternalSignInComponent implements IForm{
    submitInProgress: boolean;
    submitCompleted: boolean;
    submitFailed: boolean;
    submitSucceeded: boolean;
    responseMessage: string;
    validationErrors: Array<string>;
    formData: any;

    @Input() provider: string;
    @Input() buttonIcon: string;
    @Input() buttonClass: string;
    @Input() inputDisabled: boolean;

    @Output() onSignInSucceeded: EventEmitter<any> = new EventEmitter<any>();
    @Output() onSignInFailed: EventEmitter<any> = new EventEmitter<any>();
    @Output() onSignInInProgress: EventEmitter<any> = new EventEmitter<any>();

    constructor(private authService: AuthService,
                public externalAuthService: ExternalAuthService){
    }

    submit(isValid: boolean): Promise<any> {
        this.onSignInInProgress.emit();
        return this.externalAuthService.login(this.provider)
            .then((data: any) => this.onExternalAuthSucceeded(data))
            .catch((error: any) => this.onExternalAuthFailed(error));
    }

    onSubmitSucceeded(response: any): Promise<any> {
        this.submitInProgress = false;
        this.submitFailed = false;
        this.responseMessage = '';
        this.onSignInSucceeded.emit(response);
        return Promise.resolve(response);
    }

    onSubmitFailed(errorResponse: any): Promise<any> {
        var error = errorResponse || {};
        if(!error.message){
            error.message = 'Failed to sign in the external user';
        }
        this.submitInProgress = false;
        this.submitFailed = true;
        this.responseMessage = error.message;
        this.onSignInFailed.emit(error);
        return Promise.reject(error);
    }

    reset(): void {
    }

    private onExternalAuthSucceeded = (data: any) => {
        this.submitInProgress = true;
        this.authService.externalSignIn(data['token'], this.provider)
            .then((response: any) => this.onSubmitSucceeded(response))
            .catch((error: any) => this.onSubmitFailed(error));
    };

    private onExternalAuthFailed = (error: any) => {
        this.onSubmitFailed(error);
    };
}
