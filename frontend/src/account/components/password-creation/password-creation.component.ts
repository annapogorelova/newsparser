import {Component, Inject} from '@angular/core';
import {
    ApiService,
    BaseForm,
    NoticesService,
    NavigatorService
} from '../../../shared';

/**
 * Component contains functionality for the password creation
 */
@Component({
    templateUrl: 'password-creation.component.html',
    selector: 'password-creation'
})
export class PasswordCreationComponent extends BaseForm {
    protected method:string = 'post';
    protected apiRoute:string = 'account/passwordCreation';

    formData:any = {
        password: '',
        passwordRepeat: ''
    };

    constructor(@Inject(ApiService) apiService:ApiService,
                @Inject(NoticesService) notices:NoticesService,
                private navigator:NavigatorService) {
        super(apiService, notices);
    }

    submit(isValid:boolean):Promise<any> {
        return super.submit(isValid)
            .then(() => this.navigator.navigate(['/account']));
    };
}
