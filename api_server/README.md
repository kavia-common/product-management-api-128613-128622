# API Server

A .NET 8 Web API providing CRUD for Product (name, price) with EF Core persistence.

- OpenAPI UI at /docs (NSwag) and /swagger (Swashbuckle)
- Health check at /

Configuration via environment variables (see .env.example):
- DB_PROVIDER: sqlite (default) or inmemory
- DB_CONNECTION_STRING: for sqlite provider

By default, if DB_CONNECTION_STRING isn't provided and DB_PROVIDER=sqlite, a SQLite file is created under App_Data.
