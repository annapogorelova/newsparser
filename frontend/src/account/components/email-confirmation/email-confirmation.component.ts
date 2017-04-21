import {Component, OnInit} from '@angular/core';
import {ApiService} from "../../../shared/services/api/api.service";
import {ActivatedRoute, Router} from "@angular/router";
import {AuthService} from "../../../shared/services/auth/auth.service";

/**
 * Component contains functionality for the confirmation of email
 */
@Component({
    templateUrl: 'email-confirmation.component.html',
    selector: 'email-confirmation'
})
export class EmailConfirmationComponent implements OnInit {
    private email: string;
    private confirmationToken: string;
    public confirmationSucceeded: boolean;
    public confirmationFailed: boolean;
    public errorMessage: string;
    public message: string;

    public constructor(private apiService: ApiService,
                       private authService: AuthService,
                       private route: ActivatedRoute,
                       private router: Router){}

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
            .then(data => this.onConfirmationSucceeded(data))
            .catch(error => this.onConfirmationFailed(error));
    }

    onConfirmationSucceeded = (data: any) => {
        this.confirmationSucceeded = true;
        this.message = data.message;
    };

    onConfirmationFailed = (error: any) => {
        this.confirmationFailed = true;
        this.errorMessage = error.message;
    };
}
