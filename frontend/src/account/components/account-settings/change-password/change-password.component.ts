import {Component, Inject, ViewChild, OnInit} from '@angular/core';
import {ApiService} from '../../../../shared/services/api/api.service';
import {BaseForm} from '../../../../shared/abstract/base-form/base-form';
import {NgForm} from '@angular/forms';
import {NavigatorService} from "../../../../shared/services/navigator/navigator.service";

@Component({
    templateUrl: 'change-password.component.html',
    selector: 'change-password'
})

export class ChangePasswordComponent extends BaseForm implements OnInit {
    protected apiRoute: string = 'account/passwordChange';
    protected method: string = 'put';

    formData: any = {
        currentPassword: '',
        newPassword: '',
        confirmNewPassword: ''
    };

    @ViewChild('f') form: NgForm;

    constructor(@Inject(ApiService) apiService: ApiService,
                private navigator: NavigatorService){
        super(apiService);
    }

    ngOnInit(){
        this.navigator.navigate([], {fragment: 'password'});
    }

    reset(){
        this.form.resetForm();
    };

    submit(isValid: boolean){
        super.submit(isValid).then(() => this.reset());
    };
}