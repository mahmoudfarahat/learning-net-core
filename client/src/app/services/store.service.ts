import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { title } from "process";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { LoginRequest, LoginResults } from "../shared/LoginResults";
import { Order, OrderITem } from "../shared/Order";
import { Product } from "../shared/Product";
@Injectable()

export class Store {

    constructor(private http: HttpClient) {
        
    }
    public products: Product[] = []
    public order: Order = new Order();

    public token = "";
    public expiration = new Date();

    loadProducts(): Observable<void> {
        return this.http.get<[]>("/api/product")
            .pipe(map(data => {
                this.products = data;
                return;
            }))
    }

    get loginRequired(): boolean {
        return this.token.length === 0 || this.expiration > new Date();
    }
    login(creds :LoginRequest) {
        return this.http.post<LoginResults>("/account/CreatToken", creds)
            .pipe(map(data => {
                this.token = data.token;
                this.expiration = data.expirarion;
            }))

    }
    checkout() {
        const headers = new HttpHeaders().set("Authorization", `Bearer ${ this.token }`)
        return this.http.post("/api/orders", this.order, {
            headers: headers
        })
            .pipe(map(() => {
                this.order = new Order();
            }))
    }
    addToOrder(product: Product) {

        let newItem: OrderITem;
        newItem = this.order.iTems.find(o => o.productId === product.id);
        if (newItem) {
            newItem.quantity++;
        } else {
              newItem = new OrderITem();
            newItem.productId = product.id;
            newItem.productTitle = product.title;
            newItem.quantity = 1;
            newItem.unitPrice = product.price;
            newItem.productCategory = product.category;
            newItem.productSize = product.size;
            newItem.productPrice = product.price;
            newItem.productArtId = product.artId;

        this.order.iTems.push(newItem);

        }
    }

}