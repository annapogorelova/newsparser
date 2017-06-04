import {Component, OnInit, Inject, ViewChild, Input} from '@angular/core';
import {
    ApiService,
    BaseForm,
    NavigatorService,
    NoticesService
} from '../../../../shared';

@Component({
    templateUrl: 'edit-profile.component.html',
    selector: 'edit-profile'
})

/**
 * Component for editing the profile's email
 */
export class EditProfileComponent extends BaseForm implements OnInit {
    protected apiRoute:string = 'account';
    protected method:string = 'put';

    formData:any = {
        email: ''
    };

    formDataChanged:boolean;
    @Input() user:any;

    @ViewChild('f') form:any;

    constructor(@Inject(ApiService) apiService:ApiService,
                @Inject(NoticesService) notices:NoticesService,
                private navigator:NavigatorService) {
        super(apiService, notices);
    }

    ngOnInit() {
        this.navigator.navigate([], {fragment: 'profile'});
        this.initializeFormData(this.user);
    };

    ngAfterViewInit() {
        this.form.control.valueChanges.subscribe((values:any) => this.onFormChanged(values));
    };

    onFormChanged(values:any) {
        if (!this.user) {
            return;
        }

        if (this.user.email !== values.email) {
            this.formDataChanged = true;
            return;
        }

        this.formDataChanged = false;
    };

    initializeFormData(data:any) {
        this.user = data;
        this.formData.email = data.email;
    };

    submit(isValid:boolean):Promise<any> {
        return super.submit(isValid);
    };
}