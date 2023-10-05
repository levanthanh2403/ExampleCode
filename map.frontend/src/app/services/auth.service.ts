import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private ACCESS_TOKEN: string = "ACCESS_TOKEN";
    constructor(
        private router: Router,
        private http: HttpClient
    ) {
    }

    logout() {
        console.log("reomve token");
        localStorage.removeItem(this.ACCESS_TOKEN);
        this.router.navigate(['/auth/signin'], {
            queryParams: {},
        });
    }

    public getAuthFromLocalStorage(): any | undefined {
        try {
            const lsValue = localStorage.getItem(this.ACCESS_TOKEN);
            if (!lsValue) {
                return undefined;
            }
            const authData = JSON.parse(lsValue);
            return authData;
        } catch (error) {
            console.error(error);
            return undefined;
        }
    }
    checkLogin(): boolean {
        var token = localStorage.getItem(this.ACCESS_TOKEN)
        if (token == null || token == '')
            return false;
        return true;
    }
}