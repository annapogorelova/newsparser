import {Component, OnInit, Inject} from '@angular/core';
import {ApiService} from '../../../shared/services/api/api.service';
import {ActivatedRoute, Router} from '@angular/router';
import {AuthService} from '../../../shared/services/auth/auth.service';
import {BaseForm} from '../../../shared/abstract/base-form/base-form';

/**
 * Component contains functionality for the confirmation of email
 */
@Component({
    templateUrl: 'email-confirmation.component.html',
    selector: 'email-confirmation'
})
export class EmailConfirmationComponent extends BaseForm implements OnInit {
    protected apiRoute: string;
    protected method: string = 'post';

    formData: any = {
        confirmationToken: ''
    };

    email: string;

    public constructor(@Inject(ApiService) apiService: ApiService,
                       private authService: AuthService,
                       private route: ActivatedRoute,
                       private router: Router){
        super(apiService);
    }

    ngOnInit() {
        this.route.queryParams
            .map((queryParams) => queryParams['email'])
            .subscribe((email: string) => this.onEmailRetrieved(email));

        this.route.queryParams
            .map((queryParams) => queryParams['confirmationToken'])
            .subscribe((confirmationToken: string) => this.formData.confirmationToken = confirmationToken);
    }

    ngAfterViewInit() {
        if(!this.email || !this.formData.confirmationToken){
            this.router.navigate(['/sign-in']);
        }

        this.submit(true);
    }

    onEmailRetrieved = (email: string) => {
        this.email = email;
        this.apiRoute = `account/${this.email}/confirmation`;
    };
}
