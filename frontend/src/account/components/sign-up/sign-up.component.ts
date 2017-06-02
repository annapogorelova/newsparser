import {Component, Inject} from '@angular/core';
import {ApiService, BaseForm, NoticesService} from '../../../shared';

/**
 * Component contains functionality for creating user account
 */
@Component({
    templateUrl: 'sign-up.component.html',
    styleUrls: ['sign-up.component.css'],
    selector: 'sign-up'
})
export class SignUpComponent extends BaseForm {
    protected apiRoute:string = 'account';
    protected method:string = 'post';

    formData:any = {
        email: '',
        password: '',
        confirmPassword: ''
    };

    constructor(@Inject(ApiService) apiService:ApiService,
                @Inject(NoticesService) notices:NoticesService) {
        super(apiService, notices);
    }
}
