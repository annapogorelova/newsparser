import {NgModule} from '@angular/core';
import {HttpModule} from '@angular/http';
import {BrowserModule} from '@angular/platform-browser';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {SharedModule} from '../shared/shared.module';
import {AppComponent}  from './components';
import {NewsModule} from '../news';
import {AppRoutingProviders, AppRouting} from './app.routing';
import {AccountModule} from '../account';
import {NewsSourcesModule} from '../news-sources';


@NgModule({
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        SharedModule,
        HttpModule,
        NewsModule,
        NewsSourcesModule,
        AccountModule,
        AppRouting,
        NgbModule.forRoot()
    ],
    declarations: [
        AppComponent
    ],
    bootstrap: [AppComponent],
    providers: [AppRoutingProviders]
})
export class AppModule {
}