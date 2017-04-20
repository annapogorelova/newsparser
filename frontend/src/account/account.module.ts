import {NgModule}      from '@angular/core';
import {SignInComponent} from './components/sign-in/sign-in.component';
import {SignUpComponent} from './components/sign-up/sign-up.component';
import {FormsModule} from '@angular/forms';
import {ExternalSignInComponent} from './components/external-sign-in/external-sign-in.component';
import {AppSettings} from '../app/app.settings';
import {ExternalAuthModule} from '../shared/modules/external-auth/external-auth.module';
import {AccountConfirmationComponent} from './components/account-confirmation/account-confirmation.component';
import {SharedModule} from '../shared/shared.module';
import {BrowserModule} from '@angular/platform-browser';
import {AccountRoutingProviders, AccountRouting} from './account.routing';
import {PasswordRemindComponent} from './components/password-remind/password-remind.component';
import {PasswordResetComponent} from './components/password-reset/password-reset.component';

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
    imports: [FormsModule, BrowserModule, ExternalAuthModule.initWithProviders(authProviders),
        SharedModule, AccountRouting],
    declarations: [SignInComponent, SignUpComponent, ExternalSignInComponent,
        AccountConfirmationComponent, PasswordRemindComponent, PasswordResetComponent],
    providers: [AccountRoutingProviders]
})

export class AccountModule {}