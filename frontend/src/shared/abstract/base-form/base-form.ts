import {ApiService} from '../../services';
import {NoticesService} from '../../modules';

/**
 * Interface contains a declaration of methods and properties for a basic form
 */
export interface IForm {
    submitInProgress:boolean;
    submitCompleted:boolean;
    submitFailed:boolean;
    submitSucceeded:boolean;
    validationErrors:Array<string>;
    formData:any;

    submit(isValid:boolean):Promise<any>;
    onSubmitSucceeded(response:any):Promise<any>;
    onSubmitFailed (error:any):Promise<any>;
    reset():void;
}

/**
 * Class contains methods to manipulate the form data,
 * to be extended by specific components that represent forms
 */
export abstract class BaseForm implements IForm {
    submitInProgress:boolean;
    submitCompleted:boolean;
    submitFailed:boolean;
    submitSucceeded:boolean;
    validationErrors:Array<string>;
    abstract formData:any;

    protected abstract apiRoute:string;
    protected abstract method:string;

    constructor(protected apiService:ApiService,
                protected notices:NoticesService) {
    }

    /**
     * Resets the form settings
     */
    reset() {
        this.submitInProgress = false;
        this.submitCompleted = false;
        this.submitFailed = false;
        this.submitSucceeded = false;
    };

    /**
     * Submits the form data to the soecified route
     * @param isValid
     * @returns {Promise<T>|Promise<TResult|T>|Promise<R>|any}
     */
    submit(isValid:boolean):Promise<any> {
        if (!isValid) {
            return;
        }

        this.submitInProgress = true;
        return this.apiService[this.method](this.apiRoute, this.formData)
            .then((response:any) => this.onSubmitSucceeded(response))
            .catch((error:any) => this.onSubmitFailed(error));
    };

    /**
     * Callback to be executed on submit success
     * @param response
     */
    onSubmitSucceeded(response:any) {
        this.submitInProgress = false;
        this.submitCompleted = true;
        this.submitSucceeded = true;
        this.submitFailed = false;
        if (response) {
            this.notices.success(response.message);
        }

        return response;
    };

    /**
     * Callback to be executed on submit fail
     * @param error
     */
    onSubmitFailed(error:any) {
        this.submitInProgress = false;
        this.submitCompleted = true;
        this.submitSucceeded = false;
        this.submitFailed = true;

        if (error) {
            if (error.message) {
                this.notices.error(error.message);
            }

            if (error.validationErrors) {
                this.validationErrors = error.validationErrors.map(function (e:any) {
                    return e['message'];
                });
            }

            for (var i = 0; i < this.validationErrors.length; i++) {
                this.notices.error(this.validationErrors[i]);
            }
        }

        return error;
    };
}