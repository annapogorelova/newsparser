import {Component, Inject} from '@angular/core';
import {Router} from '@angular/router';
import {AuthService} from '../../../shared/services/auth/auth.service';
import {IForm} from '../../../shared/abstract/base-form/base-form';

@Component({
    templateUrl: 'sign-in.component.html',
    styleUrls: ['./sign-in.component.css'],
    selector: 'sign-in'
})
export class SignInComponent implements IForm {
    public submitCompleted: boolean;
    public validationErrors: Array<string>;
    public submitInProgress: boolean;
    public submitFailed: boolean;
    public submitSucceeded: boolean;
    public responseMessage: string;

    public formData: any = {
        email: '',
        password: ''
    };

    constructor(@Inject(Router) private router: Router,
                private authService: AuthService){

    }

    reset() {}

    public onSubmitSucceeded = (response: any) => {
        this.submitInProgress = false;
        this.submitSucceeded = true;
        this.router.navigate(['/news']);
        return Promise.resolve(response);
    };

    public onSubmitFailed = (error: Error) => {
        this.submitInProgress = false;
        this.submitFailed = true;
        this.responseMessage = error.message;
        return Promise.reject(error);
    };

    submit = (isValid: boolean) => {
        if(!isValid){
            return;
        }
        this.submitInProgress = true;
        return this.authService.signIn(this.formData.email, this.formData.password)
            .then(auth => this.onSubmitSucceeded(auth))
            .catch(error => this.onSubmitFailed(error));
    };
}
