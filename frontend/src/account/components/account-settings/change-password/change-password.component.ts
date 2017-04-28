import {Component, Inject, ViewChild} from '@angular/core';
import {ApiService} from '../../../../shared/services/api/api.service';
import {BaseForm} from '../../../../shared/abstract/base-form/base-form';
import {NgForm} from '@angular/forms';

@Component({
    templateUrl: 'change-password.component.html',
    selector: 'change-password'
})

export class ChangePasswordComponent extends BaseForm {
    protected apiRoute: string = 'account/passwordChange';
    protected method: string = 'put';

    formData: any = {
        currentPassword: '',
        newPassword: '',
        confirmNewPassword: ''
    };

    @ViewChild('f') form: NgForm;

    constructor(@Inject(ApiService) apiService: ApiService){
        super(apiService);
    }

    reset(){
        this.form.resetForm();
    };

    submit(isValid: boolean){
        super.submit(isValid).then(() => this.reset());
    };
}