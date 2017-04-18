import {Component, OnInit} from '@angular/core';
import {ApiService} from "../../../shared/services/api/api.service";
import {ActivatedRoute} from "@angular/router";

/**
 * Component contains functionality for the password reset
 */
@Component({
    templateUrl: 'password-reset.component.html',
    styleUrls: ['password-reset.component.css'],
    selector: 'password-reset'
})
export class PasswordResetComponent implements OnInit{
    public email: string;
    public passwordResetToken: string;
    public submitInProgress: boolean;
    public submitComplete: boolean;
    public passwordResetSucceeded: boolean;
    public passwordResetFailed: boolean;
    public errorMessage: string;
    public resetPasswordForm: any = {
        password: '',
        confirmPassword: ''
    };

    public constructor(private apiService: ApiService, private route: ActivatedRoute){}

    ngOnInit(){
        this.route.queryParams
            .map((queryParams) => queryParams['email'])
            .subscribe((email: string) => this.email = email);

        this.route.queryParams
            .map((queryParams) => queryParams['passwordResetToken'])
            .subscribe((passwordResetToken: string) => this.passwordResetToken = passwordResetToken);
    }
    
    submit = (isValid: boolean) => {
        if(!isValid){
            return;
        }

        this.submitInProgress = true;
        this.apiService.post(`account/${this.email}/passwordRecovery`,
            {
                newPassword: this.resetPasswordForm.password,
                passwordResetToken: this.passwordResetToken
            })
            .then(() => this.onSubmitSucceeded())
            .catch(error => this.onSubmitFailed(error));
    };

    onSubmitSucceeded = () => {
        this.submitInProgress = false;
        this.submitComplete = true;
        this.passwordResetSucceeded = true;
    };

    onSubmitFailed = (error: any) => {
        this.submitInProgress = false;
        this.passwordResetFailed = true;
        this.errorMessage = error.message;
    };
}
