import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {FormsModule} from '@angular/forms';
import {SharedModule} from '../shared';
import {
    AddNewsSourceComponent,
	SubscriptionItemComponent,
	NewsSourceListItemComponent,
	NewsSourcesMultiSelectList,
	NewsSourcesSingleSelectList
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
	    AddNewsSourceComponent,
	    SubscriptionItemComponent,
	    NewsSourceListItemComponent,
	    NewsSourcesMultiSelectList,
	    NewsSourcesSingleSelectList
    ],
    exports: [
	    AddNewsSourceComponent,
	    NewsSourcesMultiSelectList,
	    NewsSourcesSingleSelectList
    ]
})
export class NewsSourcesModule {}