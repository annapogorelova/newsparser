import {ModuleWithProviders} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {SignInComponent} from './components/sign-in/sign-in.component';
import {RegisterComponent} from './components/register/register.component';
import {AccountConfirmationComponent} from './components/account-confirmation/account-confirmation.component';
import {PasswordRemindComponent} from './components/password-remind/password-remind.component';
import {PasswordResetComponent} from './components/password-reset/password-reset.component';

const accountRoutes: Routes = [
    {path: 'sign-in', component: SignInComponent},
    {path: 'register', component: RegisterComponent},
    {path: 'account-confirmation', component: AccountConfirmationComponent},
    {path: 'password-remind', component: PasswordRemindComponent},
    {path: 'password-reset', component: PasswordResetComponent}
];

export const AccountRoutingProviders:any[] = [];
export const AccountRouting: ModuleWithProviders = RouterModule.forRoot(accountRoutes);