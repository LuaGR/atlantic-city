# Makefile para Atlantic City - Reto Técnico Fullstack .NET + React

.PHONY: help up down dev-backend dev-frontend test clean db-reset db-migrate

help:
	@echo "========================================="
	@echo "  Atlantic City - Makefile de Servicios"
	@echo "========================================="
	@echo ""
	@echo "Comandos disponibles:"
	@echo "  make up           - Iniciar servicios Docker (PostgreSQL)"
	@echo "  make down        - Detener servicios Docker"
	@echo "  make dev-backend - Iniciar desarrollo backend (.NET)"
	@echo "  make dev-frontend- Iniciar desarrollo frontend (Next.js)"
	@echo "  make test        - Ejecutar todos los tests"
	@echo "  make db-reset    - Reiniciar base de datos"
	@echo "  make db-migrate - Ejecutar migraciones EF Core"
	@echo "  make clean       - Limpiar binarios y node_modules"
	@echo ""

up:
	@echo "🚀 Iniciando servicios Docker..."
	cd docker && docker compose up -d
	@echo "✅ PostgreSQL disponible en localhost:5432"
	@echo "✅ pgAdmin disponible en http://localhost:5050"

down:
	@echo "🛑 Deteniendo servicios Docker..."
	cd docker && docker compose down

dev-backend:
	@echo "🔧 Iniciando backend .NET..."
	cd backend && dotnet restore && dotnet run

dev-frontend:
	@echo "🔧 Iniciando frontend Next.js..."
	cd frontend && npm install && npm run dev

test:
	@echo "🧪 Ejecutando tests..."
	@echo "--- Backend Tests ---"
	cd backend && dotnet test
	@echo "--- Frontend Tests ---"
	cd frontend && npm test

db-reset:
	@echo "🔄 Reiniciando base de datos..."
	cd docker && docker compose down -v && docker compose up -d

db-migrate:
	@echo "🔄 Ejecutando migraciones..."
	cd backend && dotnet ef migrations apply

clean:
	@echo "🧹 Limpiando binarios..."
	cd backend && dotnet clean
	cd frontend && rm -rf node_modules .next