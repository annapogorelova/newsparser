import {Component, OnInit, Inject, ViewChild} from '@angular/core';
import {ApiService} from '../../../../shared/services/api/api.service';
import {BaseForm} from '../../../../shared/abstract/base-form/base-form';
import {AuthService} from "../../../../shared/services/auth/auth.service";

@Component({
    templateUrl: 'edit-account.component.html',
    selector: 'edit-account'
})

/**
 * Component for editing the acount's email
 */
export class EditAccountComponent extends BaseForm implements OnInit {
    protected apiRoute: string = 'account';
    protected method: string = 'put';

    formData: any = {
        email: ''
    };

    formDataChanged: boolean;
    user: any;

    @ViewChild('f') form: any;

    constructor(@Inject(ApiService) apiService: ApiService,
                private authService: AuthService){
        super(apiService);
    }

    ngOnInit(){
        this.loadAccount();
    }

    ngAfterViewInit() {
        this.form.control.valueChanges.subscribe((values: any) => this.onFormChanged(values));
    }
    
    onFormChanged = (values: any) =>{
        if(!this.user){
            return;
        }

        if(this.user.email !== values.email){
            this.formDataChanged = true;
            return;
        }

        this.formDataChanged = false;
    };

    loadAccount = (refresh: boolean = false) => {
        this.authService.loadUser(refresh).then((data: any) => this.initializeFormData(data));
    };
    
    initializeFormData = (data: any) => {
        this.user = data;
        this.formData.email = data.email;
    };

    submit(isValid: boolean) {
        super.submit(isValid).then(() => this.loadAccount(true));
    };
}