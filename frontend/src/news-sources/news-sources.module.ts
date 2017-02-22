import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {SharedModule} from '../shared/shared.module';
import {NewsSourcesListComponent} from './components/news-sources-list/news-sources-list.component';
import {NewsSourcesRouting, NewsSourcesRoutingProviders} from './news-sources.routing';
import {AddNewsSourceComponent} from './components/add-news-source/add-news-source.component';
import {FormsModule} from '@angular/forms';

@NgModule({
    imports: [NewsSourcesRouting, BrowserModule, SharedModule, FormsModule],
    declarations: [NewsSourcesListComponent, AddNewsSourceComponent],
    providers: [NewsSourcesRoutingProviders]
})
export class NewsSourcesModule {}