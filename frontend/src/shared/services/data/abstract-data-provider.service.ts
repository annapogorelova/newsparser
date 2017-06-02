import {ApiService} from '../api/api.service';
import {Injectable} from '@angular/core';

/**
 * Abstract service class to be put between component and apiService
 * to implement the 'middleware' logic in its methods (caching, etc.);
 * Contains basic methods for the data operations.
 */
@Injectable()
export class AbstractDataProviderService {
    constructor(private apiService:ApiService) {
    }

    get(apiRoute:string, params:any, headers:any, refresh:boolean = false) {
        return this.apiService.get(apiRoute, params, headers, refresh)
            .then((response:any) => this.onRequestSucceeded(response));
    };

    post(apiRoute:string, params:any, headers:any) {
        return this.apiService.post(apiRoute, params, headers);
    };

    put(apiRoute:string, params:any, headers:any) {
        return this.apiService.put(apiRoute, params, headers);
    };

    delete(apiRoute:string, id:number) {
        return this.apiService.delete(apiRoute, id);
    };

    private onRequestSucceeded(response:any) {
        return Promise.resolve(response);
    };
}