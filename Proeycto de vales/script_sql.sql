-- Script SQL para Sistema de Vales
-- Crear base de datos

DROP DATABASE IF EXISTS sistema_vales;
CREATE DATABASE sistema_vales CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

USE sistema_vales;

-- Tabla de Usuarios
CREATE TABLE usuarios (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre_usuario VARCHAR(50) NOT NULL UNIQUE,
    contraseña VARCHAR(100) NOT NULL,
    nombre_completo VARCHAR(100) NOT NULL,
    rol VARCHAR(50) NOT NULL DEFAULT 'Operador',
    activo BOOLEAN DEFAULT TRUE,
    fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    INDEX idx_usuario (nombre_usuario)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla de Clientes
CREATE TABLE clientes (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    telefono VARCHAR(20) NOT NULL,
    fecha_registro TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE KEY unique_nombre (nombre)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla de Vales
CREATE TABLE vales (
    id INT AUTO_INCREMENT PRIMARY KEY,
    cliente_id INT NOT NULL,
    monto DECIMAL(10, 2) NOT NULL,
    fecha_prestamo DATE NOT NULL,
    fecha_limite DATE NOT NULL,
    fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (cliente_id) REFERENCES clientes(id) ON DELETE CASCADE,
    INDEX idx_cliente (cliente_id),
    INDEX idx_fecha (fecha_limite)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tabla de Pagos
CREATE TABLE pagos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    vale_id INT NOT NULL,
    monto_pagado DECIMAL(10, 2) NOT NULL,
    fecha_pago DATE NOT NULL,
    fecha_registro TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (vale_id) REFERENCES vales(id) ON DELETE CASCADE,
    INDEX idx_vale (vale_id),
    INDEX idx_fecha_pago (fecha_pago)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Insertar datos de ejemplo
INSERT INTO usuarios (nombre_usuario, password, nombre_completo, rol) VALUES
('admin', 'admin123', 'Administrador', 'Administrador'),
('usuario1', 'pass123', 'Juan Operador', 'Operador'),
('usuario2', 'pass456', 'María Operador', 'Operador');

INSERT INTO clientes (nombre, telefono) VALUES
('Juan Pérez', '3104567890'),
('María García', '3115678901'),
('Carlos López', '3126789012'),
('Ana Martínez', '3137890123');

INSERT INTO vales (cliente_id, monto, fecha_prestamo, fecha_limite) VALUES
(1, 50000.00, '2026-04-01', '2026-05-01'),
(2, 75000.00, '2026-03-15', '2026-04-15'),
(3, 100000.00, '2026-02-01', '2026-03-01'),
(4, 30000.00, '2026-04-10', '2026-05-10');

INSERT INTO pagos (vale_id, monto_pagado, fecha_pago) VALUES
(1, 25000.00, '2026-04-20'),
(2, 50000.00, '2026-04-10'),
(3, 100000.00, '2026-04-01'),
(4, 0.00, '2026-04-19');

-- Vista para reportes (opcional pero útil)
CREATE VIEW vista_detalles_vales AS
SELECT 
    c.id AS cliente_id,
    c.nombre AS cliente,
    c.telefono,
    v.id AS vale_id,
    v.monto,
    v.fecha_prestamo,
    v.fecha_limite,
    COALESCE(SUM(p.monto_pagado), 0) AS total_pagado,
    v.monto - COALESCE(SUM(p.monto_pagado), 0) AS deuda_actual,
    CASE 
        WHEN v.monto - COALESCE(SUM(p.monto_pagado), 0) <= 0 THEN 'Pagado'
        WHEN CURDATE() > v.fecha_limite THEN 'Atrasado'
        ELSE 'En tiempo'
    END AS estado
FROM clientes c
INNER JOIN vales v ON c.id = v.cliente_id
LEFT JOIN pagos p ON v.id = p.vale_id
GROUP BY v.id;

-- Crear usuario para la aplicación (recomendado por seguridad)
-- Descomenta estas líneas si quieres crear un usuario específico
-- CREATE USER 'usuario_vales'@'localhost' IDENTIFIED BY 'contraseña123';
-- GRANT ALL PRIVILEGES ON sistema_vales.* TO 'usuario_vales'@'localhost';
-- FLUSH PRIVILEGES;
