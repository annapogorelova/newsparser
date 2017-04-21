import {ModuleWithProviders} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {SignInComponent} from './components/sign-in/sign-in.component';
import {SignUpComponent} from './components/sign-up/sign-up.component';
import {EmailConfirmationComponent} from './components/email-confirmation/email-confirmation.component';
import {PasswordRemindComponent} from './components/password-remind/password-remind.component';
import {PasswordResetComponent} from './components/password-reset/password-reset.component';
import {AccountSettingsComponent} from './components/account-settings/settings/settings.component';

const accountRoutes: Routes = [
    {path: 'sign-in', component: SignInComponent},
    {path: 'sign-up', component: SignUpComponent},
    {path: 'email-confirmation', component: EmailConfirmationComponent},
    {path: 'password-remind', component: PasswordRemindComponent},
    {path: 'password-reset', component: PasswordResetComponent},
    {path: 'account-settings', component: AccountSettingsComponent}
];

export const AccountRoutingProviders:any[] = [];
export const AccountRouting: ModuleWithProviders = RouterModule.forRoot(accountRoutes);