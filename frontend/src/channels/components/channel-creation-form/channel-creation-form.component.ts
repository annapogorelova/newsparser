import {Component, ViewChild, Output, Inject, EventEmitter} from '@angular/core';
import {NgForm} from '@angular/forms';
import {ApiService, NoticesService, BaseForm} from '../../../shared';

/**
 * A component for adding a new news channel
 */
@Component({
    selector: 'channel-creation-form',
    templateUrl: './channel-creation-form.component.html',
    styleUrls: ['./channel-creation-form.component.css']
})

export class ChannelCreationFormComponent extends BaseForm {
    protected apiRoute:string = 'channels';
    protected method:string = 'post';

    formData:any = {
        feedUrl: '',
        isPrivate: false
    };

    @ViewChild('f') form:NgForm;

    @Output() onChannelAdded:EventEmitter<any> = new EventEmitter<any>();

    constructor(@Inject(ApiService) apiService:ApiService,
                @Inject(NoticesService) notices:NoticesService) {
        super(apiService, notices);
    }

    reset() {
        this.form.resetForm();
        this.formData.isPrivate = false;
    };

    submit(isValid:boolean):Promise<any> {
        return super.submit(isValid)
            .then((response:any) => this.handleSubmit(response));
    }

    handleSubmit(response:any) {
        this.reset();
        this.onChannelAdded.emit(response.data);
    };
}