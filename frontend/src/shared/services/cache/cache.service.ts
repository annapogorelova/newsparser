import {Injectable} from '@angular/core';

/**
 * Wrapper for localStorage caching
 */
@Injectable()
export class CacheService {
    /**
     * Get cached data by key
     * @param key
     * @returns {any}
     */
    get = (key: string) => {
        var data = JSON.parse(localStorage.getItem(key));
        if(data){
            if(!data.expires || Date.now() <= data.expires){
                return data.data;
            }

            this.remove(key);
        }

        return null;
    };

    /**
     * Set cached data
     * @param key
     * @param data
     * @param maxAge
     */
    set = (key: string, data: any, maxAge: number = null) => {
        var expires = maxAge !== null ? Date.now() + (maxAge * 1000) : null;
        localStorage.setItem(key, JSON.stringify({data: data, expires: expires}));
    };

    /**
     * Remove cached data
     * @param key
     */
    remove = (key: string) => {
        localStorage.removeItem(key);
    };

    /**
     * Clear all data from localStorage
     */
    clear = ()=> {
        localStorage.clear();
    };

    /**
     * Get cache key from url and params
     * @param url
     * @param params
     * @returns {string}
     */
    getCacheKey(url: string, params: any = {}){
        return params ? `${url}?${this.getJoinedUrlParams(params)}` : url;
    };

    /**
     * Joins params object into url query string representation
     * @param params
     * @returns {string}
     */
    private getJoinedUrlParams(params: any = {}) {
        var propNames = Object.getOwnPropertyNames(params);
        var result = propNames.map(function(name){
            if(params[name] !== null){
                return [name, params[name]].join('=');
            }

            return null;
        }).filter(function(n){ return n != null }).join('&');
        return result;
    };
}