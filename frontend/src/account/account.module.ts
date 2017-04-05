import { NgModule }      from '@angular/core';
import {SignInComponent} from './sign-in/sign-in.component';
import {RegisterComponent} from "./register/register.component";
import {FormsModule} from '@angular/forms';
import {Angular2SocialLoginModule} from 'angular2-social-login';
import {ExternalSignInComponent} from './external-sign-in/external-sign-in.component';
import {AppSettings} from '../app/app.settings';


let providers = {
    google: {
        clientId: AppSettings.GOOGLE_CLIENT_ID
    },
    facebook: {
        clientId: AppSettings.FACEBOOK_CLIENT_ID,
        apiVersion: AppSettings.FACEBOOK_API_VERSION
    }
};

@NgModule({
    imports: [FormsModule, Angular2SocialLoginModule.initWithProviders(providers)],
    declarations: [SignInComponent, RegisterComponent, ExternalSignInComponent],
    exports: [SignInComponent, RegisterComponent]
})

export class AccountModule {}