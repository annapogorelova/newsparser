import {Component, ViewChild, Output, Inject, EventEmitter} from '@angular/core';
import {ApiService, NoticesService, BaseForm} from '../../../shared';
import {NgForm} from '@angular/forms';

/**
 * A component for adding a new news source
 */
@Component({
    selector: 'add-news-source',
    templateUrl: 'add-news-source.component.html',
    styleUrls: ['./add-news-source.component.css']
})

export class AddNewsSourceComponent extends BaseForm {
    protected apiRoute: string = 'newsSources';
    protected method: string = 'post';

    formData: any = {
        rssUrl: '',
        isPrivate: false
    };

    @ViewChild('f') form: NgForm;

    @Output() onSourceAdded: EventEmitter<any> = new EventEmitter<any>();

    constructor(@Inject(ApiService) apiService: ApiService,
                @Inject(NoticesService) notices: NoticesService){
        super(apiService, notices);
    }

    reset(){
        this.form.resetForm();
        this.formData.isPrivate = false;
    }

    submit(isValid: boolean): Promise<any>{
        return super.submit(isValid)
            .then((response: any) => this.handleSubmit(response));
    }
    
    handleSubmit(response: any){
        this.reset();
        this.onSourceAdded.emit(response.data);
    };
}