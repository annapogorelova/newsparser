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
import {BaseListComponent} from './components/base-list/base-list.component';
import {CacheService} from './services/cache/cache.service';
import {SearchComponent} from './components/search/search.component';
import {FormsModule} from '@angular/forms';
import {GoTopButtonModule} from 'ng2-go-top-button';

@NgModule({
    imports: [BrowserModule, FormsModule, GoTopButtonModule],
    providers: [
        ApiService, AuthService, CanActivateAuth,
        PagerServiceProvider,
        {
            provide: NavigatorService,
            useFactory: (router: Router, activatedRoute: ActivatedRoute) =>
                new NavigatorService(router, activatedRoute),
            deps: [Router, ActivatedRoute]
        },
        ApiErrorHandler,
        CacheService
    ],
    declarations: [PageNotFoundComponent, RefreshButtonComponent, BaseListComponent, SearchComponent],
    exports: [PageNotFoundComponent, RefreshButtonComponent, BaseListComponent, SearchComponent, GoTopButtonModule]
})

export class SharedModule {
}