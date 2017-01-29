import { NgModule }      from '@angular/core';
import {SignInComponent} from './sign-in/sign-in.component';
import {RegisterComponent} from "./register/register.component";
import {FormsModule} from '@angular/forms';


@NgModule({
    imports: [FormsModule],
    declarations: [SignInComponent, RegisterComponent],
    exports: [SignInComponent, RegisterComponent]
})

export class AccountModule {}