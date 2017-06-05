import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {ChannelsModule} from '../channels';
import {SharedModule} from '../shared';
import {SidebarComponent} from './components';

@NgModule({
    imports: [
        BrowserModule,
        ChannelsModule,
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