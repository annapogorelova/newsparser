import {ApiService} from "../../services/api/api.service";
/**
 * Class contains methods to manipulate the form data,
 * to be extended by specific components that represent forms
 */
export abstract class BaseForm {
    protected submitInProgress: boolean;
    protected submitCompleted: boolean;
    protected submitFailed: boolean;
    protected submitSucceeded: boolean;
    protected responseMessage: string;

    protected abstract apiRoute: string;
    protected abstract method: string;
    protected abstract formData: any;

    constructor(protected apiService: ApiService){

    }

    /**
     * Resets the form settings
     */
    protected reset() {
        this.submitInProgress = false;
        this.submitCompleted = false;
        this.submitFailed = false;
        this.submitSucceeded = false;
        this.responseMessage = '';
    };

    /**
     * Submits the form data to the soecified route
     * @param isValid
     * @returns {Promise<T>|Promise<TResult|T>|Promise<R>|any}
     */
    protected submit(isValid: boolean) {
        if(!isValid){
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
    protected onSubmitSucceeded = (response: any) => {
        this.submitInProgress = false;
        this.submitCompleted = true;
        this.submitSucceeded = true;
        this.submitFailed = false;
        if(response){
            this.responseMessage = response.message;
        }

        return Promise.resolve(response);
    };

    /**
     * Callback to be executed on submit fail
     * @param error
     */
    protected onSubmitFailed = (error: any) => {
        this.submitInProgress = false;
        this.submitCompleted = true;
        this.submitSucceeded = false;
        this.submitFailed = true;
        if(error){
            this.responseMessage = error.message;
        }

        return Promise.reject(error);
    };
}