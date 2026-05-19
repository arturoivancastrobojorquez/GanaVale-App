-- Script SQL para agregar soporte de Valeras al Sistema de Vales
-- Ejecutar este script para preparar la BD para múltiples valeras

USE sistema_vales;

-- ==================== AGREGAR COLUMNAS DE VALERA ====================

-- Agregar columna a tabla clientes
ALTER TABLE clientes ADD COLUMN valera VARCHAR(50) DEFAULT 'Impulsa' AFTER telefono;
ALTER TABLE clientes ADD INDEX idx_valera (valera);

-- Agregar columna a tabla vales
ALTER TABLE vales ADD COLUMN valera VARCHAR(50) DEFAULT 'Impulsa' AFTER fecha_limite;
ALTER TABLE vales ADD INDEX idx_vales_valera (valera);

-- Agregar columna a tabla pagos
ALTER TABLE pagos ADD COLUMN valera VARCHAR(50) DEFAULT 'Impulsa' AFTER fecha_pago;
ALTER TABLE pagos ADD INDEX idx_pagos_valera (valera);

-- ==================== ACTUALIZAR DATOS EXISTENTES ====================

-- Marcar todos los registros existentes como pertenecientes a Impulsa
UPDATE clientes SET valera = 'Impulsa' WHERE valera IS NULL OR valera = 'Impulsa';
UPDATE vales SET valera = 'Impulsa' WHERE valera IS NULL OR valera = 'Impulsa';
UPDATE pagos SET valera = 'Impulsa' WHERE valera IS NULL OR valera = 'Impulsa';

-- ==================== CREAR VISTA FILTRADA POR VALERA ====================

-- Vista para obtener detalles de vales por valera
DROP VIEW IF EXISTS vista_detalles_vales_por_valera;
CREATE VIEW vista_detalles_vales_por_valera AS
SELECT 
    c.id AS cliente_id,
    c.nombre AS cliente,
    c.telefono,
    c.valera,
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
INNER JOIN vales v ON c.id = v.cliente_id AND c.valera = v.valera
LEFT JOIN pagos p ON v.id = p.vale_id AND p.valera = v.valera
GROUP BY v.id;

-- ==================== INSERTAR DATOS DE PRUEBA ADICIONALES ====================

-- Agregar clientes de prueba para las otras valeras
INSERT INTO clientes (nombre, telefono, valera) VALUES
('Pedro Rodríguez', '3104567891', 'Nexus'),
('Laura Sánchez', '3115678902', 'Nexus'),
('Diego Morales', '3126789013', 'Sale Vale'),
('Catalina Rivas', '3137890124', 'Sale Vale');

-- Agregar vales de prueba para las otras valeras
INSERT INTO vales (cliente_id, monto, fecha_prestamo, fecha_limite, valera) VALUES
(5, 40000.00, '2026-04-01', '2026-05-01', 'Nexus'),
(6, 60000.00, '2026-03-15', '2026-04-15', 'Nexus'),
(7, 80000.00, '2026-02-01', '2026-03-01', 'Sale Vale'),
(8, 25000.00, '2026-04-10', '2026-05-10', 'Sale Vale');

-- Agregar pagos de prueba para las otras valeras
INSERT INTO pagos (vale_id, monto_pagado, fecha_pago, valera) VALUES
(5, 20000.00, '2026-04-20', 'Nexus'),
(6, 30000.00, '2026-04-10', 'Nexus'),
(7, 80000.00, '2026-04-01', 'Sale Vale'),
(8, 0.00, '2026-04-19', 'Sale Vale');

-- ==================== CONSULTAS ÚTILES ====================

-- Ver todos los clientes con sus valeras
-- SELECT * FROM clientes;

-- Ver todos los vales con detalles por valera
-- SELECT * FROM vista_detalles_vales_por_valera WHERE valera = 'Impulsa';

-- Contar vales por valera
-- SELECT valera, COUNT(*) as total_vales FROM vales GROUP BY valera;

-- Sumar montos por valera
-- SELECT valera, SUM(monto) as total_prestado FROM vales GROUP BY valera;
