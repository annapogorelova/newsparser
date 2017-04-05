import { Component } from '@angular/core';

@Component({
    templateUrl: 'register.component.html',
})
export class RegisterComponent  {
    public form: any = {
        email: '',
        password: '',
        passwordRepeat: ''
    };

    public submit = () => {
        // Nothing!
    };
}
