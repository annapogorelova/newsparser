import {NgModule} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {BrowserModule} from '@angular/platform-browser';
import {InfiniteScrollModule} from 'ngx-infinite-scroll';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {FeedListComponent, FeedPageComponent} from './components';
import {FeedRouting, FeedRoutingProviders} from './feed.routing';
import {SharedModule} from '../shared';
import {ChannelsModule} from '../channels';
import {LayoutModule} from '../layout';

@NgModule({
    imports: [
        FeedRouting,
        BrowserModule,
        InfiniteScrollModule,
        SharedModule,
        FormsModule,
        ChannelsModule,
        NgbModule,
        LayoutModule
    ],
    declarations: [
        FeedListComponent,
        FeedPageComponent
    ],
    providers: [FeedRoutingProviders]
})
export class FeedModule {
}