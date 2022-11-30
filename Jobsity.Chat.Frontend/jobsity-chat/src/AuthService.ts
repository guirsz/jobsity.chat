import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import { environment } from 'src/environments/environment';  

@Injectable()
export class AuthService implements CanActivate {

    constructor(private http: HttpClient,
        private router: Router) {
    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        if (localStorage.getItem('session')) {
            let token = JSON.parse(localStorage.getItem('session') ?? "");

            if (Date.parse(token.expiration) > Date.now()) {
                return true;
            } else {
                this.router.navigate(['/login']);
                return false;
            }
        } else {
            // not logged in so redirect to login page with the return url
            this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
            return false;
        }
    }

    getSession() {
        let itemSession = localStorage.getItem('session');
        if (itemSession) {
            const session = JSON.parse(itemSession);
            if (!session) {
                return undefined;
            }
            return session;
        }
		return undefined;
	}

    getToken(): string {
		const session = this.getSession();

		if (session && session.accessToken) {
			return session.accessToken;
		} else {
			return "";
		}
	}

    getUserEmail(): string {
		const session = this.getSession();

		if (session && session.userEmail) {
			return session.userEmail;
		} else {
			return "";
		}
	}

    getUserName(): string {
		const session = this.getSession();

		if (session && session.userName) {
			return session.userName;
		} else {
			return "";
		}
	}

    login(email: string, password: string) {
        /*
            {
                "authenticated": true,
                "created": "2022-11-29T20:39:50",
                "expiration": "2022-11-29T21:39:50",
                "accessToken": "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6WyJndWlyc3pAZ21haWwuY29tIiwiR3VpbGhlcm1lIFNvdXphIl0sImp0aSI6ImU2Nzc0MGM3LWY1MWYtNDE5NS05OTEzLTYxY2NkYTcxMTBiYyIsImVtYWlsIjoiZ3VpcnN6QGdtYWlsLmNvbSIsIm5hbWVpZCI6IjI4YWZjYmU2LTc5YWMtNGJiOC1hZDU5LTY1NTY1N2Q4M2Y3NSIsIm5iZiI6MTY2OTc2NTE5MCwiZXhwIjoxNjY5NzY4NzkwLCJpYXQiOjE2Njk3NjUxOTAsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcxNzUiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MTc1In0.FoIOZhXglMWZDjvU4Gx1RCLG4cSq7bBF8MGbzvvgv-mNhXkI0fl3mBBXTFX9f0xsXtAxVNlV5o0KfZP30wRSVNURwdWsPCAw844QodMp3o98A_ay1FZb74tOF30K3D7IWeF6KEUxVlIouRbZ8hHIapqrWIIfx7upd-JnrAn856DqXSEnv3GbTOhgWUa6vVrgyif-64XqnbGCAQHMdOWpGgQ1RK1StxVV8g4SG5D_k6xB6g_79MNicFEzmN2C5R3BUW1wg6ACmCzlhaxaKZ5mhnfcb8gl1RF7YvZjg6PLBQR2gyxPmefqsSKXeRc2nMjJXSXUQB3Akb8FaMcphokQew",
                "userEmail": "guirsz@gmail.com",
                "userName": "Guilherme Souza",
                "message": "User logged in successfully."
            }
            {
                "authenticated": false,
                "message": "Authentication Failure."
            }
        */
        let payload = {
            Email: email,
            password: password
        };
        let httpOptions = {
            headers: new HttpHeaders({ 'Content-Type': 'application/json' })
        };

        this.http.post<any>(environment.baseUrl + 'api/Login', payload, httpOptions)
            .subscribe(token => {
                if (token.authenticated) {
                    localStorage.setItem('session', JSON.stringify(token));
                    this.router.navigateByUrl('/chat');
                }
            });
    }

    logout() {
        localStorage.removeItem('session');
        if (!localStorage.getItem('session')) {
            this.router.navigate(['/login']);
        }
    }
}