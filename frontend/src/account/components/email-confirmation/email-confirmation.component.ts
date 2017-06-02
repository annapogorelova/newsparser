import {Component, OnInit, Inject} from '@angular/core';
import {ApiService, AuthProviderService, BaseForm, NoticesService} from '../../../shared';
import {ActivatedRoute, Router} from '@angular/router';

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
                       @Inject(NoticesService) notices: NoticesService,
                       private authProvider: AuthProviderService,
                       private route: ActivatedRoute,
                       private router: Router){
        super(apiService, notices);
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

        this.submit(true)
	        .then(() => this.router.navigate(['']))
	        .catch(() => this.router.navigate(['']));
    };

    onEmailRetrieved(email: string) {
        this.email = email;
        this.apiRoute = `account/${this.email}/confirmation`;
    };
}
