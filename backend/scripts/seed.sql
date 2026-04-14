INSERT INTO "Usuarios" ("Email", "PasswordHash", "Rol", "Activo", "CreatedAt")
VALUES (
    'admin@atlanticcity.com',
    '$2a$11$oXPq.KskC62etBuij9vQf.1M.SsV7REhPCRsNzjtsPc3OXOWhhDEq',
    'Admin',
    TRUE,
    CURRENT_TIMESTAMP
)
ON CONFLICT ("Email") DO NOTHING;

INSERT INTO "Usuarios" ("Email", "PasswordHash", "Rol", "Activo", "CreatedAt")
VALUES (
    'user@atlanticcity.com',
    '$2a$11$sqSaWIwkOES26OAW6R410uVv1RrNz7VAS03IVGn0X8eJaAq0OtDmq',
    'User',
    TRUE,
    CURRENT_TIMESTAMP
)
ON CONFLICT ("Email") DO NOTHING;

INSERT INTO "Pedidos" ("NumeroPedido", "Cliente", "Fecha", "Total", "Estado", "Eliminado", "CreatedAt")
VALUES 
    ('PED-001', 'Juan Pérez', CURRENT_TIMESTAMP, 150.50, 'Registrado', FALSE, CURRENT_TIMESTAMP),
    ('PED-002', 'María García', CURRENT_TIMESTAMP, 280.00, 'Procesado', FALSE, CURRENT_TIMESTAMP),
    ('PED-003', 'Carlos López', CURRENT_TIMESTAMP, 450.75, 'Enviado', FALSE, CURRENT_TIMESTAMP),
    ('PED-004', 'Ana Martínez', CURRENT_TIMESTAMP, 95.00, 'Entregado', FALSE, CURRENT_TIMESTAMP)
ON CONFLICT ("NumeroPedido") DO NOTHING;
