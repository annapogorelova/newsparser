import {NgModule} from '@angular/core';
import {HttpModule} from '@angular/http';
import {BrowserModule} from '@angular/platform-browser';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {SharedModule} from '../shared/shared.module';
import {AppComponent}  from './components/app.component';
import {NewsModule} from '../news';
import {AppRoutingProviders, AppRouting} from './app.routing';
import {AccountModule} from '../account';
import {NewsSourcesModule} from '../news-sources';


@NgModule({
    imports: [BrowserModule, SharedModule, HttpModule, NewsModule, NewsSourcesModule,
        AccountModule, AppRouting, NgbModule.forRoot()],
    declarations: [AppComponent],
    bootstrap: [AppComponent],
    providers: [AppRoutingProviders]
})
export class AppModule {
}