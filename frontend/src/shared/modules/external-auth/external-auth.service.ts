declare let gapi: any;
declare let FB: any;

export interface IProviders {
    [provider: string]: IProvider;
}

export interface IProvider {
    clientId: string;
    apiVersion?: string;
}

/**
 * Service contains methods for authentication with  external providers
 */
export class ExternalAuthService {
    gauth: any;

    login = (provider: string): Promise<any> => {
        switch(provider){
            case 'google':
                return this.loginViaGoogle();
            case 'facebook':
                return this.loginViaFacebook();
        }
    };

    private loginViaFacebook = (): Promise<any> => {
        return new Promise((resolve, reject) => {
            FB.getLoginStatus((response: any) => {
                if(response.status === 'connected'){
                    return resolve(this.getFacebookUserDetails(response));
                }
                else{
                    FB.login((response: any) => {
                        if(response.status === 'connected'){
                            return resolve(this.getFacebookUserDetails(response));
                        }

                        return reject(response);
                    }, {scope: 'email'});
                }
            });
        });
    };

    private getFacebookUserDetails = (response: any): Promise<any> => {
        return new Promise((resolve, reject) => {
            FB.api('/me?fields=email', (res: any) => {
                if (!res || res.error) {
                    return reject(res.error);
                } else {
                    let userDetails = {
                        email: res.email,
                        provider: 'facebook',
                        token: response.authResponse.accessToken
                    };
                    return resolve(userDetails);
                }
            });
        });
    };

    private loginViaGoogle = (): Promise<any> => {
        return new Promise((resolve, reject) => {
            if (typeof(this.gauth) == 'undefined') {
                this.gauth = gapi.auth2.getAuthInstance();
            }

            if (!this.gauth.isSignedIn.get()) {
                this.gauth.signIn().then(() => {
                    return resolve(this.getGoogleUserDetails());
                });
            } else {
                return resolve(this.getGoogleUserDetails());
            }
        });
    };

    private getGoogleUserDetails = () => {
        let currentUser = this.gauth.currentUser.get();
        let profile = currentUser.getBasicProfile();
        let idToken = currentUser.getAuthResponse().id_token;
        return {
            token: idToken,
            email: profile.getEmail(),
            provider: 'google'
        };
    };
}