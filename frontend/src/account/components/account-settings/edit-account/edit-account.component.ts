import {Component, OnInit, Inject, OnChanges, SimpleChanges, ViewChild} from '@angular/core';
import {ApiService} from '../../../../shared/services/api/api.service';
import {BaseForm} from '../../../../shared/abstract/base-form/base-form';

@Component({
    templateUrl: 'edit-account.component.html',
    selector: 'edit-account'
})

export class EditAccountComponent extends BaseForm implements OnInit {
    protected apiRoute: string = 'account';
    protected method: string = 'put';
    protected formData: any = {
        email: ''
    };

    public formDataChanged: boolean;
    public user: any;

    @ViewChild('f') form: any;

    constructor(@Inject(ApiService) apiService: ApiService){
        super(apiService);
    }

    ngOnInit(){
        this.getAccount();
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
    
    getAccount = () => {
        this.apiService.get('account', null, null, true)
            .then(response => this.initializeFormData(response.data));
    };
    
    initializeFormData = (data: any) => {
        this.user = data;
        this.formData.email = data.email;
    };

    protected submit(isValid: boolean) {
        super.submit(isValid).then(() => this.getAccount());
    };
}