import {NgModule} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {BrowserModule} from '@angular/platform-browser';
import {InfiniteScrollModule} from 'ngx-infinite-scroll';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {NewsListComponent, NewsPageComponent} from './components';
import {NewsRouting, NewsRoutingProviders} from './news.routing';
import {SharedModule} from '../shared';
import {NewsSourcesModule} from '../news-sources';
import {LayoutModule} from '../layout';

@NgModule({
    imports: [
        NewsRouting,
        BrowserModule,
        InfiniteScrollModule,
        SharedModule,
        FormsModule,
        NewsSourcesModule,
        NgbModule,
        LayoutModule
    ],
    declarations: [
        NewsListComponent,
        NewsPageComponent
    ],
    providers: [NewsRoutingProviders]
})
export class NewsModule {
}