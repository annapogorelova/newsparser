import {Component, Inject, ViewChild, OnInit} from '@angular/core';
import {NgForm} from '@angular/forms';
import {ApiService, BaseForm, NavigatorService, NoticesService} from '../../../../shared';

@Component({
    templateUrl: 'change-password.component.html',
    selector: 'change-password'
})

export class ChangePasswordComponent extends BaseForm implements OnInit {
    protected apiRoute:string = 'account/passwordChange';
    protected method:string = 'put';

    formData:any = {
        currentPassword: '',
        newPassword: '',
        confirmNewPassword: ''
    };

    @ViewChild('f') form:NgForm;

    constructor(@Inject(ApiService) apiService:ApiService,
                @Inject(NoticesService) notices:NoticesService,
                private navigator:NavigatorService) {
        super(apiService, notices);
    }

    ngOnInit() {
        this.navigator.navigate([], {fragment: 'password'});
    };

    reset() {
        this.form.resetForm();
    };

    submit(isValid:boolean):Promise<any> {
        return super.submit(isValid).then(() => this.reset());
    };
}