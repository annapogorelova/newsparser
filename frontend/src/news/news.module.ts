import { NgModule } from '@angular/core';
import { NewsListComponent } from './components/news-list/news-list.component';
import { NewsRouting, NewsRoutingProviders } from './news.routing';
import {BrowserModule} from '@angular/platform-browser';

@NgModule({
    imports: [ NewsRouting, BrowserModule ],
    declarations: [ NewsListComponent ],
    providers: [ NewsRoutingProviders ]
})
export class NewsModule {}