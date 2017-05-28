import {ModuleWithProviders} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {
	AccountSettingsComponent,
	PasswordResetComponent,
	PasswordRemindComponent,
	EmailConfirmationComponent,
	SignUpComponent,
	SignInComponent,
	SubscriptionsSettingsComponent
} from './components';

const accountRoutes: Routes = [
    {path: 'sign-in', component: SignInComponent},
    {path: 'sign-up', component: SignUpComponent},
    {path: 'email-confirmation', component: EmailConfirmationComponent},
    {path: 'password-remind', component: PasswordRemindComponent},
    {path: 'password-reset', component: PasswordResetComponent},
    {path: 'account', component: AccountSettingsComponent},
    {path: 'subscriptions', component: SubscriptionsSettingsComponent}
];

export const AccountRoutingProviders:any[] = [];
export const AccountRouting: ModuleWithProviders = RouterModule.forRoot(accountRoutes);