import { error } from "@angular/compiler/src/util";
import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { Store } from "../services/store.service";
import {LoginRequest} from "../shared/LoginResults";
@Component({
    selector: "login-page",
    templateUrl: "loginPage.component.html"
})
export class LoginPage {

    constructor(private store: Store, private router: Router) { }

    public creds: LoginRequest = {

        username: "",
        password:""
    }


    onLogin() {

        this.store.login(this.creds)
            .subscribe(() => {
                if (this.store.order.iTems.length > 0) {
                    this.router.navigate(["checkout"]);
                } else {
                    this.router.navigate([""]);

                }
            }, error => {
                console.log(error);
                console.log("Dsds")
            }
            );
    }




}