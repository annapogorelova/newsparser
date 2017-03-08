import {NgModule} from '@angular/core';
import {NewsListComponent} from './components/news-list/news-list.component';
import {NewsRouting, NewsRoutingProviders} from './news.routing';
import {BrowserModule} from '@angular/platform-browser';
import {InfiniteScrollModule} from 'angular2-infinite-scroll';
import {SharedModule} from '../shared/shared.module';
import {FormsModule} from '@angular/forms';
import {NewsSourcesModule} from '../news-sources/news-sources.module';

@NgModule({
    imports: [NewsRouting, BrowserModule, InfiniteScrollModule, SharedModule, FormsModule, NewsSourcesModule],
    declarations: [NewsListComponent],
    providers: [NewsRoutingProviders]
})
export class NewsModule {
}