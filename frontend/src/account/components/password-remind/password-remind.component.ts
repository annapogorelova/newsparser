import {Component, Inject} from '@angular/core';
import {ApiService} from '../../../shared/services/api/api.service';
import {BaseForm} from '../../../shared/abstract/base-form/base-form';

/**
 * Component contains functionality for the password reset
 */
@Component({
    templateUrl: 'password-remind.component.html',
    styleUrls: ['password-remind.component.css'],
    selector: 'password-remind'
})
export class PasswordRemindComponent extends BaseForm {
    protected method: string = 'post';
    protected apiRoute: string = 'account/passwordRecovery';
    protected formData: any = {
        email: ''
    };

    public constructor(@Inject(ApiService) apiService: ApiService){
        super(apiService);
    }
}
