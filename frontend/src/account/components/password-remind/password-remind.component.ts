import {Component, Inject} from '@angular/core';
import {ApiService, BaseForm, NoticesService, NavigatorService} from '../../../shared';

/**
 * Component contains functionality for the password reset
 */
@Component({
    templateUrl: 'password-remind.component.html',
    styleUrls: ['password-remind.component.css'],
    selector: 'password-remind'
})
export class PasswordRemindComponent extends BaseForm {
    protected method:string = 'post';
    protected apiRoute:string = 'account/passwordRecovery';

    formData:any = {
        email: ''
    };

    constructor(@Inject(ApiService) apiService:ApiService,
                @Inject(NoticesService) notices:NoticesService,
                private navigator:NavigatorService) {
        super(apiService, notices);
    }

    submit(isValid:boolean):Promise<any> {
        return super.submit(isValid).then(() => this.navigator.navigate(['']));
    };
}
