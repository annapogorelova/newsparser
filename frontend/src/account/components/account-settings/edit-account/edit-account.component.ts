import {Component, OnInit} from '@angular/core';
import {ApiService} from '../../../../shared/services/api/api.service';

@Component({
    templateUrl: 'edit-account.component.html',
    selector: 'edit-account'
})

export class EditAccountComponent implements OnInit {
    public submitInProgress: boolean;
    public submitSucceeded: boolean;
    public submitFailed: boolean;
    public errorMessage: string;
    public message: string;
    public user: any = {
        email: ''
    };

    constructor(private apiService: ApiService){

    }

    ngOnInit(){
        this.apiService.get('account').then(response => this.user = response.data);
    }

    submit = (isValid: boolean) => {
        if(!isValid){
            return;
        }
        
        this.apiService.put('account', this.user)
            .then(data => this.onSubmitSucceeded(data))
            .catch(error => this.onSubmitFailed(error));
    };
    
    onSubmitSucceeded = (data: any) => {
        this.submitInProgress = false;
        this.submitSucceeded = true;
        this.message = data.message;
    };
    
    onSubmitFailed = (error: any) => {
        this.submitInProgress = false;
        this.submitFailed = true;
        this.errorMessage = error.message;
    };
}