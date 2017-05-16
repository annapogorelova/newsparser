import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {NewsSourcesModule} from '../news-sources';
import {SharedModule} from '../shared';
import {SidebarComponent} from './components';

@NgModule({
    imports: [
        BrowserModule,
        NewsSourcesModule,
        SharedModule
    ],
    declarations: [
        SidebarComponent
    ],
    exports: [
        SidebarComponent
    ]
})
export class LayoutModule {
}