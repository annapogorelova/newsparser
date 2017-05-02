import {NgModule}      from '@angular/core';
import {SignInComponent} from './components/sign-in/sign-in.component';
import {SignUpComponent} from './components/sign-up/sign-up.component';
import {FormsModule} from '@angular/forms';
import {ExternalSignInComponent} from './components/external-sign-in/external-sign-in.component';
import {AppSettings} from '../app/app.settings';
import {ExternalAuthModule} from '../shared/modules/external-auth/external-auth.module';
import {EmailConfirmationComponent} from './components/email-confirmation/email-confirmation.component';
import {SharedModule} from '../shared/shared.module';
import {BrowserModule} from '@angular/platform-browser';
import {AccountRoutingProviders, AccountRouting} from './account.routing';
import {PasswordRemindComponent} from './components/password-remind/password-remind.component';
import {PasswordResetComponent} from './components/password-reset/password-reset.component';
import {AccountSettingsComponent} from './components/account-settings/settings/settings.component';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {EditProfileComponent} from './components/account-settings/edit-profile/edit-profile.component';
import {ChangePasswordComponent} from './components/account-settings/change-password/change-password.component';

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
        NgbModule
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
        ChangePasswordComponent
    ],
    providers: [AccountRoutingProviders]
})

export class AccountModule {}