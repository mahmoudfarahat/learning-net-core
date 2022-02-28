import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { Store } from "../services/store.service";
 

@Component({
  selector: "checkout",
  templateUrl: "checkout.component.html",
  styleUrls: ['checkout.component.css']
})
export class Checkout {

    constructor(public store: Store, private router: Router) {
  }

  onCheckout() {
      this.store.checkout()
          .subscribe(() => {
              this.router.navigate(["/"])
          }, err => {
              console.log(err); 
          })
  }
}