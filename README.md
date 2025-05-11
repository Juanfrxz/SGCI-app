# Sistema de Gestión de Compras e Inventario

**Proyecto para gestionar compras, inventario, promociones y movimientos de caja.**

## 📌 Descripción

Este sistema permite administrar operaciones clave en una empresa, incluyendo:

- **Gestión de terceros** : Clasificación de usuarios como empleados, clientes, proveedores o dealers.

- **Promociones** : Creación de planes promocionales con fechas de inicio/fin y productos específicos en oferta.

- **Movimientos de caja** : Apertura y cierre de caja para balances diarios, registro de dinero recogido/pagado (recibos, compras, etc.) y consulta de balances en tiempo real.

- Compras y ventas 

  :

  - **Compras** : Registro de proveedores y empleados responsables, actualización automática del stock tras la compra.
  - **Ventas** : Separación clara de registros para optimizar el control de inventario.

  

- **Tipos de movimientos** : Registros de pagos, recibos y gastos (ejemplo: salarios), con deducción automática de fondos según el tipo de transacción.

## 🔧 Características Principales

1. **Módulo de Inventarios**

   - Actualización automática de stock tras compras o ventas.
   - Control de productos en promoción.

   

2. **Gestión de Promociones**

   - Definición de fechas y productos bajo descuentos o ofertas.

   

3. **Caja Diaria**

   - Balance automatizado al cerrar caja.
   - Seguimiento de entradas/salidas de dinero (ventas, compras, recibos).

   

4. **Registro de Transacciones**

   - Detalles de compras (proveedor, empleado, fecha).
   - Tipos de movimientos personalizados (pago de empleados, servicios, etc.).

## 📦 Requisitos del Sistema

- **Backend** : Base de datos relacional (ej: MySQL o PostgreSQL).
- **Frontend** : Interfaz por consola.

# DDL

```sql
DROP DATABASE IF EXISTS sgci;

CREATE DATABASE sgci;

\c sgci;

CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- 1. Geografía
CREATE TABLE pais (
  id SERIAL PRIMARY KEY,
  nombre VARCHAR(100) NOT NULL
);

CREATE TABLE region (
  id SERIAL PRIMARY KEY,
  nombre VARCHAR(100) NOT NULL,
  pais_id INTEGER NOT NULL,
  CONSTRAINT fk_pais FOREIGN KEY (pais_id)
    REFERENCES pais(id)
);

CREATE TABLE ciudad (
  id SERIAL PRIMARY KEY,
  nombre VARCHAR(100) NOT NULL,
  region_id INTEGER NOT NULL,
  CONSTRAINT fk_region FOREIGN KEY (region_id) REFERENCES region(id)
);

-- 2. Tipos de terceros y documentos
CREATE TABLE tipo_terceros (
  id SERIAL PRIMARY KEY,
  descripcion VARCHAR(50) NOT NULL
);

CREATE TABLE tipo_documentos (
  id SERIAL PRIMARY KEY,
  descripcion VARCHAR(50) NOT NULL
);

CREATE TABLE tipo_telefonos (
  id SERIAL PRIMARY KEY,
  description VARCHAR(50) NOT NULL
);

CREATE TABLE direccion (
  id SERIAL PRIMARY KEY,
  calle VARCHAR(255) NOT NULL,
  numero_edificio VARCHAR(20)   NOT NULL,
  codigo_postal VARCHAR(20),
  ciudad_id INTEGER,
  informacion_adicional TEXT,
  CONSTRAINT fk_ciudad_direccion FOREIGN KEY (ciudad_id) REFERENCES ciudad(id)
);

-- 3. Terceros y teléfonos
CREATE TABLE terceros (
  id TEXT PRIMARY KEY,
  nombre VARCHAR(50) NOT NULL,
  apellidos VARCHAR(50),
  email VARCHAR(80) UNIQUE,
  tipo_terceros_id INTEGER NOT NULL,
  tipo_documento_id INTEGER NOT NULL,
  direccion_id INTEGER,
  CONSTRAINT fk_tipo_terceros FOREIGN KEY (tipo_terceros_id) REFERENCES tipo_terceros(id),
  CONSTRAINT fk_tipo_documentos FOREIGN KEY (tipo_documento_id) REFERENCES tipo_documentos(id),
  CONSTRAINT fk_ciudad FOREIGN KEY (direccion_id) REFERENCES direccion(id)
);

CREATE TABLE tercero_telefonos (
  id SERIAL PRIMARY KEY,
  tercero_id TEXT NOT NULL,
  numero VARCHAR(30) NOT NULL,
  tipo_telefono_id INTEGER NOT NULL,
  CONSTRAINT fk_tipo_telefono FOREIGN KEY (tipo_telefono_id) REFERENCES tipo_telefonos(id),
  CONSTRAINT fk_terceros FOREIGN KEY (tercero_id) REFERENCES terceros(id)
);

-- 4. Empresa
CREATE TABLE empresa (
  id VARCHAR(20) PRIMARY KEY,
  nombre VARCHAR(50) NOT NULL,
  direccion_id INTEGER,
  fecha_reg DATE DEFAULT CURRENT_DATE,
  CONSTRAINT fk_direccion_empresa FOREIGN KEY (direccion_id) REFERENCES direccion(id)
);

-- 5. Salud y riesgos
CREATE TABLE eps (
  id SERIAL PRIMARY KEY,
  nombre VARCHAR(100) NOT NULL
);

CREATE TABLE arl (
  id SERIAL PRIMARY KEY,
  nombre VARCHAR(100) NOT NULL
);

-- 6. Proveedor, Empleado, Cliente
CREATE TABLE proveedor (
  id SERIAL PRIMARY KEY,
  tercero_id TEXT NOT NULL UNIQUE,
  dto DOUBLE PRECISION,
  dia_pago INTEGER,
  CONSTRAINT fk_terceros_proveedor FOREIGN KEY (tercero_id) REFERENCES terceros(id)
);

CREATE TABLE empleado (
  id SERIAL PRIMARY KEY,
  tercero_id TEXT NOT NULL,
  fecha_ingreso DATE NOT NULL,
  salario_base DOUBLE PRECISION NOT NULL,
  eps_id INTEGER,
  arl_id INTEGER,
  CONSTRAINT fk_terceros_empleado FOREIGN KEY (tercero_id) REFERENCES terceros(id),
  CONSTRAINT fk_eps FOREIGN KEY (eps_id) REFERENCES eps(id),
  CONSTRAINT fk_arl FOREIGN KEY (arl_id) REFERENCES arl(id)
);

CREATE TABLE cliente (
  id SERIAL PRIMARY KEY,
  tercero_id TEXT NOT NULL,
  fecha_nac DATE,
  fecha_ult_compra DATE,
  CONSTRAINT fk_terceros_cliente FOREIGN KEY (tercero_id) REFERENCES terceros(id)
);

-- 7. Productos y su vínculo con proveedores
CREATE TABLE productos (
  id VARCHAR(20) PRIMARY KEY,
  nombre VARCHAR(50) NOT NULL,
  stock INTEGER NOT NULL DEFAULT 0,
  stock_min INTEGER NOT NULL DEFAULT 0,
  stock_max INTEGER NOT NULL DEFAULT 0,
  created_at DATE NOT NULL DEFAULT CURRENT_DATE,
  updated_at DATE NOT NULL DEFAULT CURRENT_DATE,
  barcode VARCHAR(50) UNIQUE
);

CREATE TABLE productos_proveedor (
  tercero_id TEXT NOT NULL,
  producto_id VARCHAR(20) NOT NULL,
  CONSTRAINT pk_productos_proveedor PRIMARY KEY (tercero_id, producto_id),
  CONSTRAINT fk_proveedor FOREIGN KEY (tercero_id) REFERENCES proveedor(tercero_id),
  CONSTRAINT fk_productos FOREIGN KEY (producto_id) REFERENCES productos(id)
);

-- 8. Planes y Plan–Productos
CREATE TABLE planes (
  id SERIAL PRIMARY KEY,
  nombre VARCHAR(30) NOT NULL,
  tecnico_id INTEGER,
  descuento DOUBLE PRECISION,
  fecha_inicio DATE,
  fecha_fin DATE,
  datos_extra TEXT,
  CONSTRAINT fk_empleado_planes FOREIGN KEY (tecnico_id) REFERENCES empleado(id)
);

CREATE TABLE plan_producto (
  plan_id INTEGER NOT NULL,
  producto_id VARCHAR(20) NOT NULL,
  CONSTRAINT pk_plan_producto PRIMARY KEY (plan_id, producto_id),
  CONSTRAINT fk_planes FOREIGN KEY (plan_id) REFERENCES planes(id),
  CONSTRAINT fk_productos_pp FOREIGN KEY (producto_id) REFERENCES productos(id)
);

-- 9. Compras
CREATE TABLE compras (
  id SERIAL PRIMARY KEY,
  proveedor_id INTEGER NOT NULL,
  fecha DATE NOT NULL,
  empleado_id INTEGER,
  doc_compra VARCHAR(50),
  CONSTRAINT fk_proveedor_compras FOREIGN KEY (proveedor_id) REFERENCES proveedor(id),
  CONSTRAINT fk_empleado_compras FOREIGN KEY (empleado_id) REFERENCES empleado(id)
);

CREATE TABLE detalle_compra (
  id SERIAL PRIMARY KEY,
  compra_id INTEGER NOT NULL,
  producto_id VARCHAR(20) NOT NULL,
  cantidad INTEGER NOT NULL,
  valor DOUBLE PRECISION NOT NULL,
  fecha DATE NOT NULL DEFAULT CURRENT_DATE,
  CONSTRAINT fk_compras_detalle FOREIGN KEY (compra_id) REFERENCES compras(id),
  CONSTRAINT fk_productos_dc FOREIGN KEY (producto_id) REFERENCES productos(id)
);

-- 10. Facturación
CREATE TABLE facturacion (
  id SERIAL PRIMARY KEY,
  fecha_resolucion DATE NOT NULL,
  num_inicio INTEGER NOT NULL,
  num_final INTEGER NOT NULL,
  factura_actual INTEGER NOT NULL
);

-- 11. Ventas
CREATE TABLE venta (
  fact_id SERIAL PRIMARY KEY,
  fecha DATE NOT NULL,
  empleado_id INTEGER,
  cliente_id INTEGER,
  CONSTRAINT fk_empleado_venta FOREIGN KEY (empleado_id) REFERENCES empleado(id),
  CONSTRAINT fk_cliente_venta FOREIGN KEY (cliente_id) REFERENCES cliente(id)
);

CREATE TABLE detalle_venta (
  id SERIAL PRIMARY KEY,
  fact_id INTEGER NOT NULL,
  producto_id VARCHAR(20) NOT NULL,
  cantidad INTEGER NOT NULL,
  valor DOUBLE PRECISION NOT NULL,
  CONSTRAINT fk_venta_detalle FOREIGN KEY (fact_id) REFERENCES venta(fact_id),
  CONSTRAINT fk_productos_dv FOREIGN KEY (producto_id) REFERENCES productos(id)
);

-- 12. Movimientos de caja
CREATE TABLE tipo_mov_caja (
  id SERIAL PRIMARY KEY,
  nombre VARCHAR(50) NOT NULL,
  tipo VARCHAR(20)
);

CREATE TABLE sesion_caja (
  id SERIAL PRIMARY KEY,
  abierto TIMESTAMP NOT NULL DEFAULT NOW(),
  cerrado TIMESTAMP,
  balance_apertura NUMERIC NOT NULL,
  balance_cierre NUMERIC
);

CREATE TABLE mov_caja (
  id SERIAL PRIMARY KEY,
  fecha DATE NOT NULL,
  tipo_mov_id INTEGER NOT NULL,
  valor DOUBLE PRECISION NOT NULL,
  concepto TEXT,
  tercero_id TEXT,
  sesion_id INT,
  CONSTRAINT fk_session_mov_caja FOREIGN KEY (sesion_id) REFERENCES sesion_caja(id),
  CONSTRAINT fk_tipo_mov_caja FOREIGN KEY (tipo_mov_id) REFERENCES tipo_mov_caja(id),
  CONSTRAINT fk_terceros_mov_caja FOREIGN KEY (tercero_id) REFERENCES terceros(id)
);
```

# Procedures

```sql
\c sgci;
-- 1. Asegúrate de tener la extensión para UUIDs
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- 2. Procedimiento almacenado
CREATE OR REPLACE PROCEDURE sp_create_client (
    p_calle                 text,
    p_numero_edificio       text,
    p_codigo_postal         text,
    p_ciudad_id             INTEGER,
    p_info_adicional        TEXT,
    p_nombre                text,
    p_apellidos             text,
    p_email                 text,
    p_tipo_tercero_id       INTEGER,
    p_tipo_documento_id     INTEGER,
    p_fecha_nac             TIMESTAMP,
    p_fecha_ult_compra      TIMESTAMP
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_direccion_id   INTEGER;
    v_tercero_id     text;
BEGIN
    -- 1) Insertar en dirección
    INSERT INTO direccion (
        calle,
        numero_edificio,
        codigo_postal,
        ciudad_id,
        informacion_adicional
    ) VALUES (
        p_calle,
        p_numero_edificio,
        p_codigo_postal,
        p_ciudad_id,
        p_info_adicional
    )
    RETURNING id INTO v_direccion_id;

    -- 2) Generar y truncar UUID a 20 chars
    v_tercero_id := LEFT(uuid_generate_v4()::text, 20);

    -- 2) Insertar en terceros
    INSERT INTO terceros (
        id,
        nombre,
        apellidos,
        email,
        tipo_terceros_id,
        tipo_documento_id,
        direccion_id
    ) VALUES (
        v_tercero_id,
        p_nombre,
        p_apellidos,
        p_email,
        p_tipo_tercero_id,
        p_tipo_documento_id,
        v_direccion_id
    )
    RETURNING id INTO v_tercero_id;

    -- 3) Insertar en cliente
    INSERT INTO cliente (
        tercero_id,
        fecha_nac,
        fecha_ult_compra
    ) VALUES (
        v_tercero_id,
        p_fecha_nac,
        p_fecha_ult_compra
    );
END;
$$;

-- 2. Procedimiento almacenado
CREATE OR REPLACE PROCEDURE sp_create_employee (
    p_calle text,
    p_numero_edificio text,
    p_codigo_postal text,
    p_ciudad_id INTEGER,
    p_info_adicional TEXT,
    p_nombre text,
    p_apellidos text,
    p_email text,
    p_tipo_tercero_id INTEGER,
    p_tipo_documento_id INTEGER,
    p_fecha_ingreso TIMESTAMP,
    p_salario_base DOUBLE PRECISION,
    p_Eps_id INTEGER,
    p_Arl_id INTEGER
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_direccion_id   INTEGER;
    v_tercero_id     text;
BEGIN
    -- 1) Insertar en dirección
    INSERT INTO direccion (
        calle,
        numero_edificio,
        codigo_postal,
        ciudad_id,
        informacion_adicional
    ) VALUES (
        p_calle,
        p_numero_edificio,
        p_codigo_postal,
        p_ciudad_id,
        p_info_adicional
    )
    RETURNING id INTO v_direccion_id;

    -- 2) Generar y truncar UUID a 20 chars
    v_tercero_id := LEFT(uuid_generate_v4()::text, 20);

    -- 2) Insertar en terceros
    INSERT INTO terceros (
        id,
        nombre,
        apellidos,
        email,
        tipo_terceros_id,
        tipo_documento_id,
        direccion_id
    ) VALUES (
        v_tercero_id,
        p_nombre,
        p_apellidos,
        p_email,
        p_tipo_tercero_id,
        p_tipo_documento_id,
        v_direccion_id
    )
    RETURNING id INTO v_tercero_id;

    -- 3) Insertar en cliente
    INSERT INTO empleado (
        tercero_id,
        fecha_ingreso,
        salario_base,
        eps_id,
        arl_id
    ) VALUES (
        v_tercero_id,
        p_fecha_ingreso,
        p_salario_base,
        p_Eps_id,
        p_Arl_id	
    );
END;
$$;


/*Update empleado*/

CREATE OR REPLACE PROCEDURE sp_update_employee (
    p_empleado_id          INTEGER,
    p_nombre               TEXT            DEFAULT NULL,
    p_apellidos            TEXT            DEFAULT NULL,
    p_email                TEXT            DEFAULT NULL,
    p_tipo_tercero_id      INTEGER         DEFAULT NULL,
    p_tipo_documento_id    INTEGER         DEFAULT NULL,
    p_salario_base         DOUBLE PRECISION DEFAULT NULL,
    p_eps_id               INTEGER         DEFAULT NULL,
    p_arl_id               INTEGER         DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
BEGIN
    -- 1) Actualizar datos en terceros
    UPDATE terceros t
    SET
        nombre             = COALESCE(NULLIF(p_nombre, ''),            t.nombre),
        apellidos          = COALESCE(NULLIF(p_apellidos, ''),         t.apellidos),
        email              = COALESCE(NULLIF(p_email, ''),             t.email),
        tipo_terceros_id   = COALESCE(p_tipo_tercero_id,              t.tipo_terceros_id),
        tipo_documento_id  = COALESCE(p_tipo_documento_id,            t.tipo_documento_id)
    FROM empleado e
    WHERE e.id = p_empleado_id
      AND t.id = e.tercero_id;

    -- 2) Actualizar datos específicos de empleado
    UPDATE empleado e2
    SET
        salario_base = COALESCE(p_salario_base, e2.salario_base),
        eps_id       = COALESCE(p_eps_id,       e2.eps_id),
        arl_id       = COALESCE(p_arl_id,       e2.arl_id)
    WHERE e2.id = p_empleado_id;
END;
$$;

CREATE OR REPLACE PROCEDURE sp_create_provider (
    -- Datos de la dirección (DtoAddress)
    p_calle             TEXT,
    p_numero_edificio   TEXT,
    p_codigo_postal     TEXT,
    p_ciudad_id         INTEGER,
    p_info_adicional    TEXT,
    -- Datos del tercero (DtoProvider)
    p_nombre            TEXT,
    p_apellidos         TEXT,
    p_email             TEXT,
    p_tipo_tercero_id   INTEGER,  -- corresponde a TipoTercero_id
    p_tipo_documento_id INTEGER,  -- corresponde a TipoDoc_id
    -- Datos específicos de proveedor (DtoProv)
    p_descuento         DOUBLE PRECISION,  -- corresponde a Descuento
    p_dia_pago          INTEGER   -- corresponde a DiaPago
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_direccion_id  INTEGER;
    v_tercero_id    TEXT;
BEGIN
    -- 1) Insertar en dirección
    INSERT INTO direccion (
        calle,
        numero_edificio,
        codigo_postal,
        ciudad_id,
        informacion_adicional
    ) VALUES (
        p_calle,
        p_numero_edificio,
        p_codigo_postal,
        p_ciudad_id,
        p_info_adicional
    )
    RETURNING id INTO v_direccion_id;

    -- 2) Generar y truncar UUID a 20 caracteres para el ID de tercero
    v_tercero_id := LEFT(uuid_generate_v4()::text, 20);

    -- 3) Insertar en terceros
    INSERT INTO terceros (
        id,
        nombre,
        apellidos,
        email,
        tipo_terceros_id,
        tipo_documento_id,
        direccion_id
    ) VALUES (
        v_tercero_id,
        p_nombre,
        p_apellidos,
        p_email,
        p_tipo_tercero_id,
        p_tipo_documento_id,
        v_direccion_id
    );

    -- 4) Insertar en proveedor
    INSERT INTO proveedor (
        tercero_id,
        dto,
        dia_pago
    ) VALUES (
        v_tercero_id,
        p_descuento,
        p_dia_pago
    );
END;
$$;

/*Update proveedor*/

CREATE OR REPLACE PROCEDURE sp_update_provider (
    p_provider_id         INTEGER,
    p_nombre              TEXT            DEFAULT NULL,
    p_apellidos           TEXT            DEFAULT NULL,
    p_email               TEXT            DEFAULT NULL,
    p_tipo_tercero_id     INTEGER         DEFAULT NULL,
    p_tipo_documento_id   INTEGER         DEFAULT NULL,
    p_descuento           DOUBLE PRECISION DEFAULT NULL,
    p_dia_pago            INTEGER         DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
BEGIN
    -- 1) Actualizar datos en terceros asociados al proveedor
    UPDATE terceros t
    SET
        nombre             = COALESCE(NULLIF(p_nombre, ''),           t.nombre),
        apellidos          = COALESCE(NULLIF(p_apellidos, ''),        t.apellidos),
        email              = COALESCE(NULLIF(p_email, ''),            t.email),
        tipo_terceros_id   = COALESCE(p_tipo_tercero_id,              t.tipo_terceros_id),
        tipo_documento_id  = COALESCE(p_tipo_documento_id,            t.tipo_documento_id)
    FROM proveedor p
    WHERE p.id = p_provider_id
      AND t.id = p.tercero_id;

    -- 2) Actualizar datos específicos de proveedor
    UPDATE proveedor p2
    SET
        dto = COALESCE(p_descuento, p2.dto),
        dia_pago  = COALESCE(p_dia_pago,   p2.dia_pago)
    WHERE p2.id = p_provider_id;
END;
$$;

-- 1) Procedimiento para CREAR una empresa
CREATE OR REPLACE PROCEDURE sp_create_company(
    p_id text,
    p_nombre text,
    p_calle text,
    p_numero_edificio text,
    p_codigo_postal text,
    p_ciudad_id integer,
    p_info_adicional text,
    p_fecha_registro timestamp without time zone
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_direccion_id integer;
BEGIN
    -- Insertar la dirección primero
    INSERT INTO direccion (
        calle,
        numero_edificio,
        codigo_postal,
        ciudad_id,
        informacion_adicional
    ) VALUES (
        p_calle,
        p_numero_edificio,
        p_codigo_postal,
        p_ciudad_id,
        p_info_adicional
    ) RETURNING id INTO v_direccion_id;

    -- Insertar la empresa con la dirección creada
    INSERT INTO empresa (
        id,
        nombre,
        direccion_id,
        fecha_reg
    ) VALUES (
        p_id,
        p_nombre,
        v_direccion_id,
        p_fecha_registro
    );
END;
$$;


-- 2) Procedimiento para ACTUALIZAR una empresa
CREATE OR REPLACE PROCEDURE sp_update_company(
    p_company_id text,
    p_nombre text,
    p_calle text,
    p_numero_edificio text,
    p_codigo_postal text,
    p_ciudad_id integer,
    p_info_adicional text,
    p_fecha_registro timestamp without time zone
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_direccion_id integer;
BEGIN
    -- Obtener el ID de la dirección actual de la empresa
    SELECT direccion_id INTO v_direccion_id
    FROM empresa
    WHERE id = p_company_id;

    IF v_direccion_id IS NULL THEN
        RAISE EXCEPTION 'No se encontró la empresa con ID %', p_company_id;
    END IF;

    -- Actualizar la dirección
    UPDATE direccion
    SET 
        calle = COALESCE(p_calle, calle),
        numero_edificio = COALESCE(p_numero_edificio, numero_edificio),
        codigo_postal = COALESCE(p_codigo_postal, codigo_postal),
        ciudad_id = COALESCE(p_ciudad_id, ciudad_id),
        informacion_adicional = COALESCE(p_info_adicional, informacion_adicional)
    WHERE id = v_direccion_id;

    -- Actualizar la empresa
    UPDATE empresa
    SET 
        nombre = COALESCE(p_nombre, nombre),
        fecha_reg = COALESCE(p_fecha_registro, fecha_reg)
    WHERE id = p_company_id;
END;
$$;

-- Procedimiento almacenado para crear una región
CREATE OR REPLACE PROCEDURE public.sp_create_region(
    p_nombre VARCHAR,
    p_pais_id INTEGER
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO region (nombre, pais_id)
    VALUES (p_nombre, p_pais_id);
END;
$$;

-- Procedimiento almacenado para actualizar una región
CREATE OR REPLACE PROCEDURE public.sp_update_region(
    p_id INTEGER,
    p_nombre VARCHAR,
    p_pais_id INTEGER
)
LANGUAGE plpgsql
AS $$
BEGIN
    UPDATE region
    SET nombre = p_nombre,
        pais_id = p_pais_id
    WHERE id = p_id;
END;
$$;

-- Purchase
-- Procedimiento para crear una compra
CREATE OR REPLACE PROCEDURE sp_create_purchase(
    p_proveedor_id INTEGER,
    p_fecha DATE,
    p_empleado_id INTEGER,
    p_doc_compra VARCHAR(50)
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO compras (
        proveedor_id,
        fecha,
        empleado_id,
        doc_compra
    ) VALUES (
        p_proveedor_id,
        p_fecha,
        p_empleado_id,
        p_doc_compra
    );
END;
$$;

-- Procedimiento para crear un detalle de compra
CREATE OR REPLACE PROCEDURE sp_create_purchase_detail(
    p_compra_id INTEGER,
    p_producto_id VARCHAR(20),
    p_cantidad INTEGER,
    p_valor DOUBLE PRECISION
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO detalle_compra (
        compra_id,
        producto_id,
        cantidad,
        valor,
        fecha
    ) VALUES (
        p_compra_id,
        p_producto_id,
        p_cantidad,
        p_valor,
        CURRENT_DATE
    );
END;
$$;

-- triggers

-- Trigger para actualizar el stock de productos
CREATE OR REPLACE FUNCTION update_product_stock()
RETURNS TRIGGER AS $$
BEGIN
    -- Actualizar el stock del producto
    UPDATE productos
    SET stock = stock + NEW.cantidad
    WHERE id = NEW.producto_id;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_update_stock_after_purchase
    AFTER INSERT ON detalle_compra
    FOR EACH ROW
    EXECUTE FUNCTION update_product_stock();

-- Trigger para validar proveedor
CREATE OR REPLACE FUNCTION validate_provider()
RETURNS TRIGGER AS $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM proveedor WHERE id = NEW.proveedor_id) THEN
        RAISE EXCEPTION 'El proveedor con ID % no existe', NEW.proveedor_id;
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_validate_provider
    BEFORE INSERT ON compras
    FOR EACH ROW
    EXECUTE FUNCTION validate_provider();

-- Trigger para validar empleado
CREATE OR REPLACE FUNCTION validate_employee()
RETURNS TRIGGER AS $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM empleado WHERE id = NEW.empleado_id) THEN
        RAISE EXCEPTION 'El empleado con ID % no existe', NEW.empleado_id;
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_validate_employee
    BEFORE INSERT ON compras
    FOR EACH ROW
    EXECUTE FUNCTION validate_employee();

```
