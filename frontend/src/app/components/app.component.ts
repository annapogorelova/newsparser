import { Component } from '@angular/core';
import { AuthService } from '../../shared/services/auth/auth.service';

@Component({
  selector: 'app',
  templateUrl: 'app.component.html',
})
export class AppComponent  {
  public isNavbarCollapsed = true;
  public minContentHeight: number;

  constructor(private authService: AuthService){}

  ngOnInit(){
    this.minContentHeight = window.screen.height;
  }

  signOut = () => {
    this.authService.signOut()
  };
}
