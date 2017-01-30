import { Component } from '@angular/core';
import { AuthService } from '../../shared/services/auth/auth.service';

@Component({
  selector: 'app',
  templateUrl: 'app.component.html',
})
export class AppComponent  {
  public isNavbarCollapsed = true;

  constructor(private authService: AuthService){}

  signOut = () => {
    this.authService.signOut()
  };
}
