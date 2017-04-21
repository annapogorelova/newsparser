import {Component, OnInit} from '@angular/core';
import {ApiService} from '../../../../shared/services/api/api.service';

@Component({
    templateUrl: 'change-password.component.html',
    selector: 'change-password'
})

export class ChangePasswordComponent {
    public submitInProgress: boolean;
    public changePasswordForm: any = {
        oldPassword: '',
        newPassword: '',
        confirmPassword: ''
    };

    constructor(private apiService: ApiService){

    }

    submit = (isValid: boolean) => {

    };
}