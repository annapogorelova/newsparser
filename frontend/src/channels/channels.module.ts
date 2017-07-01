import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {FormsModule} from '@angular/forms';
import {SharedModule} from '../shared';
import {
    ChannelCreationFormComponent,
    SubscriptionItemComponent,
    ChannelItemComponent,
    ChannelsMultiSelectList,
    ChannelsSingleSelectList
} from './components';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';

@NgModule({
    imports: [
        BrowserModule,
        SharedModule,
        FormsModule,
        NgbModule
    ],
    declarations: [
        ChannelCreationFormComponent,
        SubscriptionItemComponent,
        ChannelItemComponent,
        ChannelsMultiSelectList,
        ChannelsSingleSelectList
    ],
    exports: [
        ChannelCreationFormComponent,
        ChannelsMultiSelectList,
        ChannelsSingleSelectList
    ]
})
export class ChannelsModule {
}