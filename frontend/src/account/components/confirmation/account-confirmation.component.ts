import {Component, OnInit, Inject} from '@angular/core';
import {ApiService} from "../../../shared/services/api/api.service";
import {ActivatedRoute, Router} from "@angular/router";

/**
 * Component contains functionality for the confirmation of account by email
 */
@Component({
    templateUrl: 'account-confirmation.component.html',
    selector: 'account-confirmation'
})
export class AccountConfirmationComponent implements OnInit {
    private email: string;
    private confirmationToken: string;
    public confirmationSucceeded: boolean;
    public confirmationFailed: boolean;
    public errorMessage: string;

    public constructor(private apiService: ApiService,
                       private route:ActivatedRoute,
                       private router:Router){}

    ngOnInit(){
        this.route.queryParams
            .map((queryParams) => queryParams['email'])
            .subscribe((email: string) => this.email = email);

        this.route.queryParams
            .map((queryParams) => queryParams['confirmationToken'])
            .subscribe((confirmationToken: string) => this.confirmationToken = confirmationToken);
    }

    ngAfterViewInit() {
        if(!this.email || !this.confirmationToken){
            this.router.navigate(['/sign-in']);
        }

        this.apiService.post(`account/${this.email}/confirmation`, { confirmationToken: this.confirmationToken})
            .then(() => this.onConfirmationSucceeded())
            .catch(error => this.onConfirmationFailed(error));
    }

    onConfirmationSucceeded = () => {
        this.confirmationSucceeded = true;
    };

    onConfirmationFailed = (error: any) => {
        this.confirmationFailed = true;
        this.errorMessage = error.message;
    };
}
