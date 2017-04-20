import {Component} from '@angular/core';
import {ApiService} from "../../../shared/services/api/api.service";

/**
 * Component contains functionality for creating user account
 */
@Component({
    templateUrl: 'sign-up.component.html',
    styleUrls: ['sign-up.component.css'],
    selector: 'sign-up'
})
export class SignUpComponent  {
    public user: any = {
        email: '',
        password: '',
        confirmPassword: ''
    };

    public registrationSucceeded: boolean;
    public registrationFailed: boolean;
    public submitInProgress: boolean;
    public errorMessage: string;
    
    public constructor(private apiService: ApiService){}

    reset = () => {
        this.registrationFailed = false;
        this.registrationSucceeded = false;
        this.errorMessage = '';
    };

    submit = (isValid: boolean) => {
        if(!isValid){
            return;
        }

        this.reset();
        this.submitInProgress = true;
        this.apiService.post('account',
            {
                email: this.user.email,
                password: this.user.password
            }
        ).then(() => this.onSubmitSuccess()).catch(error => this.onSubmitError(error));
    };

    onSubmitSuccess = () => {
        this.registrationSucceeded = true;
        this.submitInProgress = false;
    };

    onSubmitError = (error: any) => {
        this.registrationFailed = true;
        this.submitInProgress = false;
        this.errorMessage = error.message;
    };
}
