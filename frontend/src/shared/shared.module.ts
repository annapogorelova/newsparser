import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {Router, ActivatedRoute} from '@angular/router';
import {FormsModule} from '@angular/forms';
import {GoTopButtonModule} from 'ng2-go-top-button';
import {ExternalAuthModule} from './modules';
import {EqualityValidator} from './directives';
import {Http, Response} from '@angular/http';
import {AppSettings} from '../app/app.settings';
import {
    ApiService,
    AuthService,
    NavigatorService,
    ApiErrorHandler,
    PagerServiceProvider,
    CacheService,
    AuthProviderService,
    AbstractDataProviderService,
    AuthRefreshLocker,
    CanActivateAuth
} from './services';
import {
    PageNotFoundComponent,
    RefreshButtonComponent,
    SearchComponent,
    TagListComponent
} from './components';

@NgModule({
    imports: [BrowserModule, FormsModule, GoTopButtonModule],
    providers: [
        AuthService,
        CanActivateAuth,
        PagerServiceProvider,
        AuthProviderService,
        AuthRefreshLocker,
        ApiErrorHandler,
        CacheService,
        AbstractDataProviderService,
        {
            provide: NavigatorService,
            useFactory: (router: Router, activatedRoute: ActivatedRoute) =>
                new NavigatorService(router, activatedRoute),
            deps: [Router, ActivatedRoute]
        },
        {
            provide: ApiService,
            useFactory: getApiService,
            deps: [Http, AuthProviderService, ApiErrorHandler],
        }
    ],
    declarations: [
        PageNotFoundComponent,
        RefreshButtonComponent,
        SearchComponent,
        TagListComponent,
        EqualityValidator
    ],
    exports: [
        PageNotFoundComponent,
        RefreshButtonComponent,
        SearchComponent,
        GoTopButtonModule,
        TagListComponent,
        ExternalAuthModule,
        EqualityValidator
    ]
})

export class SharedModule {
}

export function getApiService(http:Http,
                              authProvider:AuthProviderService,
                              errorHandler:ApiErrorHandler) {
    return new ApiService(
        http,
        AppSettings.API_ENDPOINT,
        function onResponseSuccess(response: Response) {
            return response.json() || {};
        },
        function onResponseError(response: Response) {
            return errorHandler.onRequestFailed(response);
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