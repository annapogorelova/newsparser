import {Component, Inject, OnInit} from '@angular/core';
import {
    ApiService,
    BaseForm,
    NoticesService,
    NavigatorService,
    PageTitleService
} from '../../../shared';

/**
 * Component contains functionality for the password reset
 */
@Component({
    templateUrl: 'password-remind.component.html',
    styleUrls: ['password-remind.component.css'],
    selector: 'password-remind'
})
export class PasswordRemindComponent extends BaseForm implements OnInit {
    protected method:string = 'post';
    protected apiRoute:string = 'account/passwordRecovery';

    formData:any = {
        email: ''
    };

    constructor(@Inject(ApiService) apiService:ApiService,
                @Inject(NoticesService) notices:NoticesService,
                private navigator:NavigatorService,
                private pageTitleService:PageTitleService) {
        super(apiService, notices);
    }

    ngOnInit() {
        this.pageTitleService.appendTitle('Password Remind');
    };

    submit(isValid:boolean):Promise<any> {
        return super.submit(isValid).then(() => this.navigator.navigate(['']));
    };
}
