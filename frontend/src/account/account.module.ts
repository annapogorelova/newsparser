import {NgModule}      from '@angular/core';
import {SignInComponent} from './components/sign-in/sign-in.component';
import {RegisterComponent} from "./components/register/register.component";
import {FormsModule} from '@angular/forms';
import {ExternalSignInComponent} from './components/external-sign-in/external-sign-in.component';
import {AppSettings} from '../app/app.settings';
import {ExternalAuthModule} from '../shared/modules/external-auth/external-auth.module';
import {AccountConfirmationComponent} from "./components/confirmation/account-confirmation.component";
import {SharedModule} from "../shared/shared.module";
import {BrowserModule} from "@angular/platform-browser";

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
    imports: [FormsModule, BrowserModule, ExternalAuthModule.initWithProviders(authProviders), SharedModule],
    declarations: [SignInComponent, RegisterComponent, ExternalSignInComponent,
        AccountConfirmationComponent],
    exports: [SignInComponent, RegisterComponent, AccountConfirmationComponent]
})

export class AccountModule {}