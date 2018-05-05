﻿import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import 'rxjs/add/operator/map';
import { Product } from "../shared/product";
import { Order } from "./order";
import { OrderItem } from "./order";



@Injectable()
export class DataService {
    constructor(private http: HttpClient) {

    }

    private token: string = "";
    private tokenExpiration: Date;

    public order: Order = new Order();

    public products: Product[] = [];

    loadProducts() : Observable<boolean>{
        return this.http.get("/api/products").map((data: any[]) => {
            this.products = data;
            return true;
        });
    }

    public get LoginRequired(): boolean {
        return this.token.length === 0 || this.tokenExpiration > new Date();
    }


    login(creds): Observable<boolean> {
        return this.http.post("/account/createtoken", creds).map((data: any) => {
            this.token = data.token;
            this.tokenExpiration = data.expiration;
            return true;
        });
    }

    public checkout() {
        if (!this.order.orderNumber) {

            this.order.orderNumber = this.order.orderDate.getFullYear().toString()+this.order.orderDate.getTime().toString()
        }
        return this.http.post("/api/orders", this.order,
            {
                headers: new HttpHeaders().set("Authorization", "Bearer " + this.token)
            }).map(response => {
            this.order = new Order();
            return true;
        });

    }

    public addOrder(newProduct: Product) {
        let item: OrderItem = this.order.items.find(i => i.productId == newProduct.id);

        if (item) {
            item.quantity++;
        } else {
            item = new OrderItem();
            item.productId = newProduct.id;
            item.productArtist = newProduct.artist;
            item.productArtId = newProduct.artId;
            item.productCategory = newProduct.category;
            item.productSize = newProduct.size;
            item.productTitle = newProduct.title;
            item.unitPrice = newProduct.price;
            item.quantity = 1;
            this.order.items.push(item);
        }
    }
}