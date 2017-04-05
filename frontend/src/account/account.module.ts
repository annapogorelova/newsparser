import { NgModule }      from '@angular/core';
import {SignInComponent} from './components/sign-in/sign-in.component';
import {RegisterComponent} from "./components/register/register.component";
import {FormsModule} from '@angular/forms';
import {ExternalSignInComponent} from './components/external-sign-in/external-sign-in.component';
import {AppSettings} from '../app/app.settings';
import {ExternalAuthModule} from '../shared/modules/external-auth/external-auth.module';

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
    imports: [FormsModule, ExternalAuthModule.initWithProviders(authProviders)],
    declarations: [SignInComponent, RegisterComponent, ExternalSignInComponent],
    exports: [SignInComponent, RegisterComponent]
})

export class AccountModule {}