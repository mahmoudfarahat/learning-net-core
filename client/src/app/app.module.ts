import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { Store } from './services/store.service';
import ProductListView from './views/productListView.component';
import { HttpClientModule } from "@angular/common/http";
@NgModule({
  declarations: [
        AppComponent,
        ProductListView
  ],
  imports: [
      BrowserModule,
      HttpClientModule
  ],
    providers: [
        Store
    ],
  bootstrap: [AppComponent]
})
export class AppModule { }
