import {Component, OnInit} from '@angular/core';
import {ApiService} from '../../../../shared/services/api/api.service';

@Component({
    templateUrl: 'settings.component.html',
    styleUrls: ['settings.component.css'],
    selector: 'account-settings'
})

export class AccountSettingsComponent implements OnInit {
    public user: any;

    constructor(private apiService: ApiService){

    }

    ngOnInit(){
        this.apiService.get('account', null, null, true).then(response => this.user = response.data);
    }
}