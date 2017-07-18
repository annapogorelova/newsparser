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
import {
    CanActivatePrivate,
    CanActivatePublic
} from '../shared';

const accountRoutes:Routes = [
    {path: 'sign-in', component: SignInComponent, canActivate: [CanActivatePublic]},
    {path: 'sign-up', component: SignUpComponent, canActivate: [CanActivatePublic]},
    {path: 'email-confirmation', component: EmailConfirmationComponent},
    {path: 'password-remind', component: PasswordRemindComponent, canActivate: [CanActivatePublic]},
    {path: 'password-reset', component: PasswordResetComponent, canActivate: [CanActivatePublic]},
    {path: 'account', component: AccountSettingsComponent, canActivate: [CanActivatePrivate]},
    {path: 'subscriptions', component: SubscriptionsSettingsComponent, canActivate: [CanActivatePrivate]}
];

export const AccountRoutingProviders:any[] = [];
export const AccountRouting:ModuleWithProviders = RouterModule.forRoot(accountRoutes);