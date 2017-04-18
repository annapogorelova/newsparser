import {Component} from '@angular/core';
import {ApiService} from "../../../shared/services/api/api.service";

/**
 * Component contains functionality for the password reset
 */
@Component({
    templateUrl: 'password-remind.component.html',
    styleUrls: ['password-remind.component.css'],
    selector: 'password-remind'
})
export class PasswordRemindComponent {
    public userEmail: string;
    public submitInProgress: boolean;
    public submitComplete: boolean;
    public errorMessage: string;

    public constructor(private apiService: ApiService){}

    submit = (isValid: boolean) => {
        if(!isValid){
            return;
        }

        this.submitInProgress = true;
        this.apiService.post(`account/passwordRecovery`, {email: this.userEmail})
            .then(() => this.onSubmitSucceeded())
            .catch(error => this.onSubmitFailed(error));
    };

    onSubmitSucceeded = () => {
        this.submitInProgress = false;
        this.submitComplete = true;
    };

    onSubmitFailed = (error: any) => {
        this.submitInProgress = false;
        this.errorMessage = error.message;
    };
}
