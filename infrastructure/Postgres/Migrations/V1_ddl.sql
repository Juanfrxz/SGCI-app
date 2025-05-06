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
  id VARCHAR(20) PRIMARY KEY,
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
  tercero_id VARCHAR(20) NOT NULL,
  numero VARCHAR(30) NOT NULL,
  tipo VARCHAR(20),
  CONSTRAINT fk_terceros FOREIGN KEY (tercero_id) REFERENCES terceros(id)
);

-- 4. Empresa
CREATE TABLE empresa (
  id VARCHAR(20) PRIMARY KEY,
  nombre VARCHAR(50) NOT NULL,
  ciudad_id INTEGER,
  fecha_reg DATE DEFAULT CURRENT_DATE,
  CONSTRAINT fk_ciudad_empresa FOREIGN KEY (ciudad_id) REFERENCES ciudad(id)
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
  tercero_id VARCHAR(20) NOT NULL UNIQUE,
  dto DOUBLE PRECISION,
  dia_pago INTEGER,
  CONSTRAINT fk_terceros_proveedor FOREIGN KEY (tercero_id) REFERENCES terceros(id)
);

CREATE TABLE empleado (
  id SERIAL PRIMARY KEY,
  tercero_id VARCHAR(20) NOT NULL,
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
  tercero_id VARCHAR(20) NOT NULL,
  fecha_nac DATE,
  fecha_ult_compra DATE,
  CONSTRAINT fk_terceros_cliente FOREIGN KEY (tercero_id) REFERENCES terceros(id)
);

-- 7. Productos y su vínculo con proveedores
CREATE TABLE productos (
  id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
  nombre VARCHAR(50) NOT NULL,
  stock INTEGER NOT NULL DEFAULT 0,
  stock_min INTEGER NOT NULL DEFAULT 0,
  stock_max INTEGER NOT NULL DEFAULT 0,
  created_at DATE NOT NULL DEFAULT CURRENT_DATE,
  updated_at DATE NOT NULL DEFAULT CURRENT_DATE,
  barcode VARCHAR(50) UNIQUE
);

CREATE TABLE productos_proveedor (
  tercero_id VARCHAR(20) NOT NULL,
  producto_id UUID NOT NULL,
  CONSTRAINT pk_productos_proveedor PRIMARY KEY (tercero_id, producto_id),
  CONSTRAINT fk_proveedor FOREIGN KEY (tercero_id) REFERENCES proveedor(tercero_id),
  CONSTRAINT fk_productos FOREIGN KEY (producto_id) REFERENCES productos(id)
);

-- 8. Planes y Plan–Productos
CREATE TABLE planes (
  id SERIAL PRIMARY KEY,
  nombre VARCHAR(30) NOT NULL,
  tecnico_id INTEGER,
  fecha_inicio DATE,
  fecha_fin DATE,
  datos_extra TEXT,
  CONSTRAINT fk_empleado_planes FOREIGN KEY (tecnico_id) REFERENCES empleado(id)
);

CREATE TABLE plan_producto (
  plan_id INTEGER NOT NULL,
  producto_id UUID NOT NULL,
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
  producto_id UUID NOT NULL,
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
  producto_id UUID NOT NULL,
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
  tercero_id VARCHAR(20),
  sesion_id INT,
  CONSTRAINT fk_session_mov_caja FOREIGN KEY (sesion_id) REFERENCES sesion_caja(id),
  CONSTRAINT fk_tipo_mov_caja FOREIGN KEY (tipo_mov_id) REFERENCES tipo_mov_caja(id),
  CONSTRAINT fk_terceros_mov_caja FOREIGN KEY (tercero_id) REFERENCES terceros(id)
);