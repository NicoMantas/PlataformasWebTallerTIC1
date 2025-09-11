-- SCRIPT PARA POSTGRES SIN SERIAL Y CON CLIENTE → VEHICULO

CREATE TABLE "Repuesto" (
  "id" int PRIMARY KEY,
  "nombre" varchar(255) NOT NULL,
  "numero_serie" bigint,
  "precio" real,
  "stock" int
);

CREATE TABLE "Proveedor" (
  "id" int PRIMARY KEY,
  "nombre" varchar(255) NOT NULL,
  "contacto" varchar(255)
);

CREATE TABLE "RepuestoProveedor" (
  "idRepuesto" int,
  "idProveedor" int,
  PRIMARY KEY ("idRepuesto", "idProveedor"),
  FOREIGN KEY ("idRepuesto") REFERENCES "Repuesto" ("id"),
  FOREIGN KEY ("idProveedor") REFERENCES "Proveedor" ("id")
);

CREATE TABLE "TipoEmpleado" (
  "id" int PRIMARY KEY,
  "descripcion" varchar(255)
);

CREATE TABLE "Empleado" (
  "id" int PRIMARY KEY,
  "nombre" varchar(255),
  "apellido" varchar(255),
  "cedula" bigint,
  "salario" double precision,
  "fechaContratacion" date,
  "idTipoEmpleado" int REFERENCES "TipoEmpleado" ("id")
);

CREATE TABLE "EstadoServicio" (
  "id" int PRIMARY KEY,
  "descripcion" varchar(255)
);

CREATE TABLE "Vehiculo" (
  "id" int PRIMARY KEY,
  "placa" varchar(50),
  "marca" varchar(50),
  "modelo" varchar(50),
  "anio" int
);

-- Subtipos de vehículos
CREATE TABLE "VGasolina" (
  "idVehiculo" int PRIMARY KEY REFERENCES "Vehiculo" ("id"),
  "cilindraje" int
);

CREATE TABLE "VElectrico" (
  "idVehiculo" int PRIMARY KEY REFERENCES "Vehiculo" ("id"),
  "capacidadBateria" int
);

CREATE TABLE "VHibrido" (
  "idVehiculo" int PRIMARY KEY REFERENCES "Vehiculo" ("id"),
  "capacidadBateria" int,
  "cilindraje" int
);

CREATE TABLE "Cliente" (
  "id" int PRIMARY KEY,
  "nombre" varchar(255),
  "email" varchar(255),
  "telefono" bigint,
  "idVehiculo" int REFERENCES "Vehiculo" ("id")  -- Cliente referencia a Vehículo
);

-- Subtipos de clientes
CREATE TABLE "CNatural" (
  "idCliente" int PRIMARY KEY REFERENCES "Cliente" ("id"),
  "cedula" bigint,
  "apellido" varchar(255)
);

CREATE TABLE "CEmpresa" (
  "idCliente" int PRIMARY KEY REFERENCES "Cliente" ("id"),
  "nit" bigint,
  "representante" varchar(255)
);

CREATE TABLE "Servicio" (
  "id" int PRIMARY KEY,
  "idCliente" int REFERENCES "Cliente" ("id"),
  "idEmpleado" int REFERENCES "Empleado" ("id"),
  "idEstado" int REFERENCES "EstadoServicio" ("id"),
  "costo" real
);

CREATE TABLE "DetalleRevision" (
  "idServicio" int PRIMARY KEY REFERENCES "Servicio" ("id"),
  "detalles" varchar(255)
);

CREATE TABLE "DetalleReparacionRepuesto" (
  "id" int PRIMARY KEY,
  "idRepuesto" int REFERENCES "Repuesto" ("id"),
  "cantidad" int
);

CREATE TABLE "DetalleReparacion" (
  "idServicio" int PRIMARY KEY REFERENCES "Servicio" ("id"),
  "idDetalleReparacionRepuesto" int REFERENCES "DetalleReparacionRepuesto" ("id")
);

CREATE TABLE "Taller" (
  "id" int PRIMARY KEY,
  "nombre" varchar(255),
  "direccion" varchar(255)
);

CREATE TABLE "UsuariosEmpleadoTaller" (
  "id" int PRIMARY KEY,
  "email" varchar(255),
  "contrasena" varchar(255),
  "idTaller" int REFERENCES "Taller" ("id"),
  "idEmpleado" int REFERENCES "Empleado" ("id")
);

CREATE TABLE "UsuariosClienteTaller" (
  "id" int PRIMARY KEY,
  "email" varchar(255),
  "contrasena" varchar(255),
  "idTaller" int REFERENCES "Taller" ("id"),
  "idCliente" int REFERENCES "Cliente" ("id")
);

CREATE TABLE "TipoEstadoOrden" (
  "id" int PRIMARY KEY,
  "descripcion" varchar(255)
);

CREATE TABLE "OrdenTrabajo" (
  "id" int PRIMARY KEY,
  "fechaCreacion" date,
  "idTipoEstadoOrden" int REFERENCES "TipoEstadoOrden" ("id")
);

CREATE TABLE "DetalleServicioOrden" (
  "idOrdenTrabajo" int REFERENCES "OrdenTrabajo" ("id"),
  "idServicio" int REFERENCES "Servicio" ("id"),
  PRIMARY KEY ("idOrdenTrabajo", "idServicio")
);

-- Comentarios opcionales
COMMENT ON COLUMN "TipoEmpleado"."descripcion" IS 'Ej: "Administrador", "Secretaria", "Mecánico"';
COMMENT ON COLUMN "EstadoServicio"."descripcion" IS 'Ej: "Pendiente", "En proceso", "Completado"';
COMMENT ON COLUMN "TipoEstadoOrden"."descripcion" IS 'Ej: "En Progreso", "Completada", "Cancelada"';
