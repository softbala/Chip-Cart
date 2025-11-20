import { Component } from '@angular/core';
@Component({selector:'app-root', template: `<div class="container"><app-header></app-header><router-outlet></router-outlet><footer class="mt-5 text-center text-muted">© {{year}} PcMate</footer></div>`})
export class AppComponent { year = new Date().getFullYear(); }