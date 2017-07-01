import {Component, Input, Output, EventEmitter} from '@angular/core';
import {AuthService, ExternalAuthService, IForm, NoticesService} from '../../../shared';

@Component({
    templateUrl: 'external-sign-in.component.html',
    styleUrls: ['external-sign-in.component.css'],
    selector: 'external-sign-in'
})

/**
 * Component for signing in via external auth providers
 */
export class ExternalSignInComponent implements IForm {
    submitInProgress:boolean;
    submitCompleted:boolean;
    submitFailed:boolean;
    submitSucceeded:boolean;
    validationErrors:Array<string>;
    formData:any;

    @Input() provider:string;
    @Input() buttonIcon:string;
    @Input() buttonClass:string;
    @Input() inputDisabled:boolean;

    @Output() onSignInSucceeded:EventEmitter<any> = new EventEmitter<any>();
    @Output() onSignInFailed:EventEmitter<any> = new EventEmitter<any>();
    @Output() onSignInInProgress:EventEmitter<any> = new EventEmitter<any>();

    constructor(private authService:AuthService,
                private notices:NoticesService,
                public externalAuthService:ExternalAuthService) {
    }

    submit(isValid:boolean):Promise<any> {
        this.onSignInInProgress.emit();
        return this.externalAuthService.login(this.provider)
            .then((data:any) => this.onExternalAuthSucceeded(data))
            .catch((error:any) => this.onExternalAuthFailed(error));
    };

    onSubmitSucceeded(response:any):Promise<any> {
        this.submitInProgress = false;
        this.submitFailed = false;
        this.onSignInSucceeded.emit(response);
        return response;
    };

    onSubmitFailed(errorResponse:any):Promise<any> {
        var error = errorResponse || {};
        if (!error.message) {
            error.message = 'Failed to sign in the external user';
        }
        this.submitInProgress = false;
        this.submitFailed = true;
        this.onSignInFailed.emit(error);
        return error;
    };

    reset():void {
    };

    private onExternalAuthSucceeded(data:any) {
        this.submitInProgress = true;
        this.authService.externalSignIn(data['token'], this.provider)
            .then((response:any) => this.onSubmitSucceeded(response))
            .catch((error:any) => this.onSubmitFailed(error));
    };

    private onExternalAuthFailed(error:any) {
        this.onSubmitFailed(error);
    };
}
