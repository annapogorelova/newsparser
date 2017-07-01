import {Component, OnInit, Inject} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {ApiService, BaseForm, NoticesService} from '../../../shared';

/**
 * Component contains functionality for the password reset
 */
@Component({
    templateUrl: 'password-reset.component.html',
    styleUrls: ['password-reset.component.css'],
    selector: 'password-reset'
})
export class PasswordResetComponent extends BaseForm implements OnInit {
    protected apiRoute:string;
    protected method:string = 'post';

    formData:any = {
        newPassword: ''
    };

    email:string;

    constructor(@Inject(ApiService) apiService:ApiService,
                @Inject(NoticesService) notices:NoticesService,
                private route:ActivatedRoute) {
        super(apiService, notices);
    }

    ngOnInit() {
        this.route.queryParams
            .map((queryParams) => queryParams['email'])
            .subscribe((email:string) => this.onEmailRetrieved(email));

        this.route.queryParams
            .map((queryParams) => queryParams['passwordResetToken'])
            .subscribe((passwordResetToken:string) => this.formData.passwordResetToken = passwordResetToken);
    };

    onEmailRetrieved(email:string) {
        this.email = email;
        this.apiRoute = `account/${this.email}/passwordRecovery`;
    };
}
