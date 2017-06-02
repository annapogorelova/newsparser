import {NgModule} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {BrowserModule} from '@angular/platform-browser';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {AppSettings} from '../app/app.settings';
import {SharedModule, ExternalAuthModule} from '../shared';
import {AccountRoutingProviders, AccountRouting} from './account.routing';
import {
    SignInComponent,
    SignUpComponent,
    ExternalSignInComponent,
    EmailConfirmationComponent,
    PasswordRemindComponent,
    PasswordResetComponent,
    AccountSettingsComponent,
    EditProfileComponent,
    ChangePasswordComponent,
    SubscriptionsSettingsComponent
} from './components';
import {NewsSourcesModule} from '../news-sources';

let authProviders = {
    google: {
        clientId: AppSettings.GOOGLE_CLIENT_ID
    },
    facebook: {
        clientId: AppSettings.FACEBOOK_CLIENT_ID,
        apiVersion: AppSettings.FACEBOOK_API_VERSION
    }
};

@NgModule({
    imports: [
        FormsModule,
        BrowserModule,
        ExternalAuthModule.initWithProviders(authProviders),
        SharedModule,
        AccountRouting,
        NgbModule,
        NewsSourcesModule
    ],
    declarations: [
        SignInComponent,
        SignUpComponent,
        ExternalSignInComponent,
        EmailConfirmationComponent,
        PasswordRemindComponent,
        PasswordResetComponent,
        AccountSettingsComponent,
        EditProfileComponent,
        ChangePasswordComponent,
        SubscriptionsSettingsComponent
    ],
    providers: [AccountRoutingProviders]
})

export class AccountModule {
}