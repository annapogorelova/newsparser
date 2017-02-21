import {NgModule, ErrorHandler} from '@angular/core';
import { ApiService } from './services/api/api.service';
import { AuthService } from './services/auth/auth.service';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { CanActivateAuth } from './services/auth/can-activate';
import { GoTopComponent } from './components/go-top-button/go-top-button.component';
import { BrowserModule } from '@angular/platform-browser';
import {PagerService} from './services/pager/pager.service';
import {NavigatorService} from './services/navigator/navigator.service';
import {Router, ActivatedRoute} from '@angular/router';
import {RefreshButtonComponent} from './components/refresh-button/refresh-button.component';
import {ApiErrorHandler} from './services/api/api-error-handler';

@NgModule({
    imports: [ BrowserModule ],
    providers: [
        ApiService, AuthService, CanActivateAuth,
        {
            provide: PagerService,
            useFactory: () => new PagerService()
        },
        {
            provide: NavigatorService,
            useFactory: (router: Router, activatedRoute: ActivatedRoute) =>
                new NavigatorService(router, activatedRoute),
            deps: [Router, ActivatedRoute]
        },
        ApiErrorHandler
    ],
    declarations: [ PageNotFoundComponent, GoTopComponent, RefreshButtonComponent ],
    exports: [ PageNotFoundComponent, GoTopComponent, RefreshButtonComponent ]
})

export class SharedModule {}