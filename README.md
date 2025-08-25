# product-management-api-128613-128622

This workspace contains a .NET 8 backend API server that provides CRUD operations for Products (name, price).

Folder: api_server

How to run:
- Optionally copy api_server/.env.example to api_server/.env and set DB_PROVIDER and DB_CONNECTION_STRING.
- Default database is SQLite file under api_server/bin/.../App_Data/products.db (auto-created). You can switch to in-memory by setting DB_PROVIDER=inmemory.

Endpoints:
- GET    /            -> Health check
- GET    /api/products
- GET    /api/products/{id}
- POST   /api/products
- PUT    /api/products/{id}
- DELETE /api/products/{id}

OpenAPI Docs:
- NSwag UI:    /docs
- Swagger UI:  /swagger

Environment variables:
- DB_PROVIDER: "sqlite" (default) or "inmemory"
- DB_CONNECTION_STRING: e.g., Data Source=./App_Data/products.db
- ALLOWED_ORIGINS: optional CORS list