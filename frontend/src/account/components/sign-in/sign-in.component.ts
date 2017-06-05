import {Component} from '@angular/core';
import {Router} from '@angular/router';
import {AuthService, IForm, NoticesService} from '../../../shared';

@Component({
    templateUrl: 'sign-in.component.html',
    styleUrls: ['./sign-in.component.css'],
    selector: 'sign-in'
})
export class SignInComponent implements IForm {
    submitCompleted:boolean;
    validationErrors:Array<string>;
    submitInProgress:boolean;
    submitFailed:boolean;
    submitSucceeded:boolean;

    formData:any = {
        email: '',
        password: ''
    };

    constructor(private router:Router,
                private authService:AuthService,
                private notices:NoticesService) {

    }

    reset():void {
    };

    onSubmitSucceeded(response:any):Promise<any> {
        this.submitInProgress = false;
        this.submitSucceeded = true;
        this.router.navigate(['/feed']);
        return Promise.resolve(response);
    };

    onSubmitFailed(error:any):Promise<any> {
        this.submitInProgress = false;
        this.submitFailed = true;
        this.notices.error(error.message || 'Failed to sign in the user.');
        return Promise.reject(error);
    };

    submit(isValid:boolean):Promise<any> {
        if (!isValid) {
            return;
        }
        this.submitInProgress = true;
        return this.authService.signIn(this.formData.email, this.formData.password)
            .then(auth => this.onSubmitSucceeded(auth))
            .catch(error => this.onSubmitFailed(error));
    };

    disableInputs() {
        this.submitInProgress = true;
    };
}
