import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { HeaderComponent } from './shared/header.component';
import { ProductsComponent } from './products/products.component';
import { CartComponent } from './cart/cart.component';
import { CheckoutComponent } from './checkout/checkout.component';
import { AdminLoginComponent } from './admin/admin-login.component';
import { AdminDashboardComponent } from './admin/admin-dashboard.component';
import { AdminUploadComponent } from './admin/admin-upload.component';
import { AuthInterceptor } from './services/auth.interceptor';
@NgModule({
  declarations: [AppComponent, HeaderComponent, ProductsComponent, CartComponent, CheckoutComponent, AdminLoginComponent, AdminDashboardComponent, AdminUploadComponent],
  imports: [BrowserModule, HttpClientModule, FormsModule, ReactiveFormsModule, RouterModule.forRoot([
    { path: '', component: ProductsComponent },
    { path: 'cart', component: CartComponent },
    { path: 'checkout', component: CheckoutComponent },
    { path: 'admin/login', component: AdminLoginComponent },
    { path: 'admin', component: AdminDashboardComponent },
    { path: 'admin/upload', component: AdminUploadComponent }
  ])],
  providers: [{ provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }],
  bootstrap: [AppComponent]
})
export class AppModule { }