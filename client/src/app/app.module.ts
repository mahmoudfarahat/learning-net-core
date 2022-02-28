import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { Store } from './services/store.service';
import ProductListView from './views/productListView.component';
import { HttpClientModule } from "@angular/common/http";
import { CartView } from './views/cartView.component';
import { Checkout } from './pages/checkout.component';
import { shopPage } from './pages/shopPage.component';
import router from './router';
import { LoginPage } from './pages/loginPage.component';
import { authActivator } from './services/authActivator.service';
import { FormsModule } from '@angular/forms';
 
@NgModule({
  declarations: [
        AppComponent,
        ProductListView,
        CartView,
        Checkout,
        shopPage,
        LoginPage
      
  ],
  imports: [
      BrowserModule,
      HttpClientModule,
      router,
      FormsModule
      
  ],
    providers: [
        Store,
        authActivator
    ],
  bootstrap: [AppComponent]
})
export class AppModule { }
