import {Component, Inject, ViewChild} from '@angular/core';
import {ApiService} from '../../../shared/services/api/api.service';
import {BaseForm} from '../../../shared/abstract/base-form/base-form';
import {NgForm} from '@angular/forms';

/**
 * A component for adding a new news source
 */
@Component({
    selector: 'add-news-source',
    templateUrl: 'add-news-source.component.html',
    styleUrls: ['./add-news-source.component.css']
})

export class AddNewsSourceComponent extends BaseForm{
    protected apiRoute: string = 'newsSources';
    protected method: string = 'post';
    
    showResponseMessage: boolean = false;

    formData: any = {
        rssUrl: ''
    };

    @ViewChild('f') form: NgForm;

    constructor(@Inject(ApiService) apiService: ApiService){
        super(apiService);
    }

    submit(isValid: boolean){
        super.submit(isValid)
            .then(() => this.handleSubmit())
            .catch(() => this.showResponseMessage = true);
    }
    
    handleSubmit(){
        this.showResponseMessage = true;
        this.form.resetForm();
    };
    
    hideResponseMessage = () => {
        this.showResponseMessage = false;
    };
    
    isResponseMessageShown = () => {
        return (this.showResponseMessage && this.submitCompleted && !this.submitInProgress);
    };
}