import {NgModule} from '@angular/core';
import {ApiService} from './services/api/api.service';
import {AuthService} from './services/auth/auth.service';
import {PageNotFoundComponent} from './components/page-not-found/page-not-found.component';
import {CanActivateAuth} from './services/auth/can-activate';
import {BrowserModule} from '@angular/platform-browser';
import {NavigatorService} from './services/navigator/navigator.service';
import {Router, ActivatedRoute} from '@angular/router';
import {RefreshButtonComponent} from './components/refresh-button/refresh-button.component';
import {ApiErrorHandler} from './services/api/api-error-handler';
import {PagerServiceProvider} from './services/pager/pager.service.provider';
import {CacheService} from './services/cache/cache.service';
import {SearchComponent} from './components/search/search.component';
import {FormsModule} from '@angular/forms';
import {GoTopButtonModule} from 'ng2-go-top-button';
import {TagListComponent} from './components/tags-list/tags-list.component';
import {ExternalAuthModule} from './modules/external-auth/external-auth.module';
import {EqualityValidator} from './directives/equality-validator.directive';
import {Http, Response} from '@angular/http';
import {AppSettings} from '../app/app.settings';
import {AuthProviderService} from './services/auth/auth-provider.service';

@NgModule({
    imports: [BrowserModule, FormsModule, GoTopButtonModule],
    providers: [
        AuthService,
        CanActivateAuth,
        PagerServiceProvider,
        {
            provide: NavigatorService,
            useFactory: (router: Router, activatedRoute: ActivatedRoute) =>
                new NavigatorService(router, activatedRoute),
            deps: [Router, ActivatedRoute]
        },
        {
            provide: ApiService,
            useFactory: getApiService,
            deps: [Http, CacheService, AuthProviderService, ApiErrorHandler],
        },
        AuthProviderService,
        ApiErrorHandler,
        CacheService
    ],
    declarations: [PageNotFoundComponent, RefreshButtonComponent, SearchComponent,
        TagListComponent, EqualityValidator],
    exports: [PageNotFoundComponent, RefreshButtonComponent, SearchComponent, GoTopButtonModule,
        TagListComponent, ExternalAuthModule, EqualityValidator]
})

export class SharedModule {
}

export function getApiService(http:Http,
                              cacheService:CacheService,
                              authProvider:AuthProviderService,
                              errorHandler:ApiErrorHandler) {
    return new ApiService(
        http,
        AppSettings.API_ENDPOINT,
        function onResponseSuccess(response:Response, cache: boolean = false) {
            let maxAge = 300;
            let body = response.json();
            if (cache && body) {
                // TODO: maybe customize this
                var cacheKey = response.url.replace(/&autotimestamp=([^&]*)/, '');
                cacheService.set(cacheKey, body, maxAge);
            }
            return body || {};
        },
        function onResponseError(response:Response) {
            return errorHandler.handleResponse(response);
        },
        function provideDefaultHeaders() {
            const headers = {'Content-Type': 'application/json'};
            if (authProvider.hasAuth()) {
                headers['Authorization'] = `Bearer ${authProvider.getAuthToken()}`;
            }
            return headers;
        }
    );
}