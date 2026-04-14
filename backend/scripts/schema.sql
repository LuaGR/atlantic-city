-- ============================================
-- Atlantic City - Schema de Base de Datos
-- ============================================

-- Tabla de Usuarios
CREATE TABLE IF NOT EXISTS Usuarios (
    Id SERIAL PRIMARY KEY,
    Email VARCHAR(150) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    Rol VARCHAR(20) NOT NULL DEFAULT 'User',
    Activo BOOLEAN NOT NULL DEFAULT TRUE,
    CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Tabla de Pedidos
CREATE TABLE IF NOT EXISTS Pedidos (
    Id SERIAL PRIMARY KEY,
    NumeroPedido VARCHAR(50) NOT NULL UNIQUE,
    Cliente VARCHAR(150) NOT NULL,
    Fecha TIMESTAMP NOT NULL,
    Total DECIMAL(10,2) NOT NULL,
    Estado VARCHAR(20) NOT NULL DEFAULT 'Registrado',
    Eliminado BOOLEAN NOT NULL DEFAULT FALSE,
    CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP
);

-- Índice para búsqueda rápida de pedidos
CREATE INDEX IF NOT EXISTS idx_pedidos_estado ON Pedidos(Estado);
CREATE INDEX IF NOT EXISTS idx_pedidos_fecha ON Pedidos(Fecha);