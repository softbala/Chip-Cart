import { Component } from '@angular/core';
import { CartService } from '../services/cart.service';
import { Router } from '@angular/router';
@Component({selector:'app-cart', template: `<h2>Cart</h2><div *ngIf="cart.items.length===0">Cart is empty</div><ul class="list-group" *ngIf="cart.items.length>0"><li class="list-group-item" *ngFor="let i of cart.items">{{i.title}} x {{i.qty}} — {{(i.price*i.qty)/100 | currency:'INR':'symbol'}}</li></ul><div class="mt-3" *ngIf="cart.items.length>0"><button class="btn btn-success" (click)="checkout()">Checkout</button></div>`})
export class CartComponent { constructor(public cart: CartService, private router: Router) {} checkout(){ this.router.navigate(['/checkout']); } }