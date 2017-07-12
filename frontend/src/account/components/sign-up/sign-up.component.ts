import {Component, Inject, OnInit} from '@angular/core';
import {
    ApiService,
    BaseForm,
    NoticesService,
    PageTitleService
} from '../../../shared';

/**
 * Component contains functionality for creating user account
 */
@Component({
    templateUrl: 'sign-up.component.html',
    styleUrls: ['sign-up.component.css'],
    selector: 'sign-up'
})
export class SignUpComponent extends BaseForm implements OnInit{
    protected apiRoute:string = 'account';
    protected method:string = 'post';

    formData:any = {
        email: '',
        password: '',
        confirmPassword: ''
    };

    constructor(@Inject(ApiService) apiService:ApiService,
                @Inject(NoticesService) notices:NoticesService,
                private pageTitleService: PageTitleService) {
        super(apiService, notices);
    }

    ngOnInit(){
        this.pageTitleService.appendTitle('Sign Up');
    };
}
