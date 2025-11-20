PcMate Full Stack (Angular + .NET) scaffold

Contents:
- /server : ASP.NET Core API with Identity, EF Core (SQLite), Stripe/Razorpay/Paytm endpoints
- /client : Angular app (frontend)
- docker-compose.yml : builds both services

Quickstart (local):
1. Replace secrets in server/appsettings.json or set environment variables.
2. Build and run: docker-compose up --build
3. Angular will be served on http://localhost:4200, API on http://localhost:5000

Important:
- Paytm checksum implementation here is an example. Use official SDK for production.
- Replace JWT secret and admin credentials before production.
