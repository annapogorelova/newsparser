import {ApiService} from '../api/api.service';
import {Injectable} from '@angular/core';
import {CacheService} from '../cache/cache.service';
import {AppSettings} from '../../../app/app.settings';

/**
 * Abstract service class to be put between component and apiService
 * to implement the 'middleware' logic in its methods (caching, etc.);
 * Contains basic methods for the data operations.
 */
@Injectable()
export class AbstractDataProviderService {
    constructor(private apiService: ApiService,
                private cacheService: CacheService){}

    get(apiRoute: string, params: any, headers: any, refresh: boolean = false){
        var cacheKey = this.cacheService.getCacheKey(AppSettings.API_ENDPOINT + apiRoute, params);
        var cachedData = this.cacheService.get(cacheKey);
        if (!refresh && cachedData) {
            return Promise.resolve(cachedData);
        }

        if(cachedData){
            this.cacheService.remove(cacheKey);
        }

        return this.apiService.get(apiRoute, params, headers, refresh)
            .then((response: any) => this.onRequestSucceeded(response, cacheKey));
    };

    post(apiRoute: string, params: any, headers: any){
        return this.apiService.post(apiRoute, params, headers);
    };

    put(apiRoute: string, params: any, headers: any){
        return this.apiService.put(apiRoute, params, headers);
    };

    delete(apiRoute: string, id: number){
        return this.apiService.delete(apiRoute, id);
    };

    private onRequestSucceeded(response: any, cacheKey: string = null){
        if (response) {
            var expiresIn = response.expires_in || AppSettings.DEFAULT_CACHE_DURATION_SECONDS;
            this.cacheService.set(cacheKey, response, expiresIn);
        }

        return Promise.resolve(response);
    };
}