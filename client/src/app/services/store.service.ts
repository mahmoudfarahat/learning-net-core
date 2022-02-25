import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { Product } from "../shared/Product";
@Injectable()

export class Store {

    constructor(private http: HttpClient) {
        
    }
    public products: Product[] = []

    loadProducts(): Observable<void> {
        return this.http.get<[]>("/api/product")
            .pipe(map(data => {
                this.products = data;
                return;
            }))
    }
}