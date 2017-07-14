import {NgModule} from '@angular/core';
import {HttpModule} from '@angular/http';
import {BrowserModule} from '@angular/platform-browser';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {SharedModule} from '../shared/shared.module';
import {AppComponent}  from './components';
import {FeedModule} from '../feed';
import {AppRoutingProviders, AppRouting} from './app.routing';
import {AccountModule} from '../account';
import {ChannelsModule} from '../channels';
import 'hammerjs';
import './meta.ts';

@NgModule({
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        SharedModule,
        HttpModule,
        FeedModule,
        ChannelsModule,
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