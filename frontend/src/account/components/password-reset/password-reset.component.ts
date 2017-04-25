import {Component, OnInit, Inject} from '@angular/core';
import {ApiService} from '../../../shared/services/api/api.service';
import {ActivatedRoute} from '@angular/router';
import {BaseForm} from '../../../shared/abstract/base-form/base-form';

/**
 * Component contains functionality for the password reset
 */
@Component({
    templateUrl: 'password-reset.component.html',
    styleUrls: ['password-reset.component.css'],
    selector: 'password-reset'
})
export class PasswordResetComponent extends BaseForm implements OnInit{
    protected apiRoute: string;
    protected method: string = 'post';
    protected formData: any = {
        newPassword: ''
    };

    public email: string;

    public constructor(@Inject(ApiService) apiService: ApiService,
                       private route: ActivatedRoute){
        super(apiService);
    }

    ngOnInit(){
        this.route.queryParams
            .map((queryParams) => queryParams['email'])
            .subscribe((email: string) => this.onEmailRetrieved(email));

        this.route.queryParams
            .map((queryParams) => queryParams['passwordResetToken'])
            .subscribe((passwordResetToken: string) => this.formData.passwordResetToken = passwordResetToken);
    }

    onEmailRetrieved = (email: string) => {
        this.email = email;
        this.apiRoute = `account/${this.email}/passwordRecovery`;
    };
}
