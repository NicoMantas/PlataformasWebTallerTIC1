CREATE TABLE "Proveedor" (
  "id" bigint PRIMARY KEY,
  "nombre" varchar,
  "contacto" varchar
);

CREATE TABLE "Repuesto" (
  "id" bigint PRIMARY KEY,
  "nombre" varchar,
  "numeroParte" bigint,
  "descripcion" varchar,
  "costoCompra" double,
  "precioVenta" double,
  "cantidadStock" int,
  "idProveedor" bigint
);

CREATE TABLE "TipoServicio" (
  "id" bigint PRIMARY KEY,
  "descripcion" varchar
);

CREATE TABLE "Estado" (
  "id" bigint PRIMARY KEY,
  "descripcion" varchar
);

CREATE TABLE "Servicio" (
  "id" bigint PRIMARY KEY,
  "descripcion" varchar,
  "costoBase" double,
  "tiempoEstimado" int,
  "idEstado" bigint,
  "idTipoServicio" bigint
);

CREATE TABLE "Reparacion" (
  "idServicio" bigint PRIMARY KEY,
  "mano_de_obra" double
);

CREATE TABLE "Revision" (
  "idServicio" bigint PRIMARY KEY,
  "diagnostico" text
);

CREATE TABLE "ServicioMecanicos" (
  "idServicio" bigint,
  "idMecanico" bigint,
  "fechaAsignacion" datetime DEFAULT (CURRENT_TIMESTAMP),
  "Primary" "Key(servicio_id,mecanico_id)"
);

CREATE TABLE "ReparacionRepuestos" (
  "idReparacion" bigint,
  "idRepuesto" bigint,
  "cantidad" int,
  "Primary" "Key(reparacion_id,repuesto_id)"
);

CREATE TABLE "Persona" (
  "id" bigint PRIMARY KEY,
  "nombre" varchar,
  "email" varchar UNIQUE,
  "telefono" bigint,
  "idTDetalleTipoPersona" varchar,
  "fechaCreacion" datetime DEFAULT (CURRENT_TIMESTAMP)
);

CREATE TABLE "TipoPersona" (
  "id" bigint PRIMARY KEY,
  "descripcion" varchar
);

CREATE TABLE "DetalleTipoPersona" (
  "id" bigint PRIMARY KEY,
  "descripcion" varchar,
  "idTipoPersona" int
);

CREATE TABLE "Usuarios" (
  "id" bigint PRIMARY KEY,
  "idPersona" bigint,
  "username" varchar UNIQUE,
  "passwordHash" varchar,
  "activo" boolean,
  "fechaCreacion" datetime DEFAULT (CURRENT_TIMESTAMP),
  "ultimoLogin" datetime
);

CREATE TABLE "clientesNaturales" (
  "idPersona" bigint PRIMARY KEY,
  "cedula" bigint UNIQUE,
  "apellido" varchar
);

CREATE TABLE "clientesJuridicos" (
  "idPersona" bigint PRIMARY KEY,
  "nit" bigint UNIQUE,
  "representanteLegal" varchar
);

CREATE TABLE "Empleados" (
  "id" bigint PRIMARY KEY,
  "nombre" varchar,
  "apellido" varchar,
  "cedula" bigint UNIQUE,
  "salario" double,
  "fechaContratacion" datetime,
  "idDetalleTipoPersona" int
);

CREATE TABLE "Mecanico" (
  "idEmpleado" bigint PRIMARY KEY,
  "especialidad" varchar,
  "tareasTrabajadas" varchar
);

CREATE TABLE "Vehiculo" (
  "placa" varchar PRIMARY KEY,
  "marca" varchar,
  "modelo" varchar,
  "año" int,
  "idPersona" bigint,
  "idTipoVehiculo" varchar,
  "fechaCreacion" datetime DEFAULT (CURRENT_TIMESTAMP)
);

CREATE TABLE "TipoVehiculo" (
  "id" bigint PRIMARY KEY,
  "descripcion" varchar
);

CREATE TABLE "VehiculoGasolina" (
  "placa" varchar PRIMARY KEY,
  "cilindraje" int
);

CREATE TABLE "VehiculoElectrico" (
  "placa" varchar PRIMARY KEY,
  "capacidadBateria" int
);

CREATE TABLE "VehiculoHibrido" (
  "placa" varchar PRIMARY KEY,
  "cilindraje" int,
  "capacidadBateria" int
);

CREATE TABLE "Taller" (
  "id" bigint PRIMARY KEY DEFAULT 1,
  "nombre" varchar,
  "direccion" varchar,
  "telefono" varchar,
  "email" varchar,
  "horarioAtencion" varchar,
  "fechaCreacion" datetime DEFAULT (CURRENT_TIMESTAMP)
);

CREATE TABLE "OrdenDeTrabajo" (
  "id" bigint PRIMARY KEY,
  "fechaCreacion" datetime DEFAULT (CURRENT_TIMESTAMP),
  "idTipoOrdenEstado" varchar,
  "idPersona" bigint,
  "placaVehiculo" varchar,
  "total" double,
  "fechaInicio" datetime,
  "fechaFinalizacion" datetime,
  "observaciones" text
);

CREATE TABLE "TipoOrdenEstado" (
  "id" bigint PRIMARY KEY,
  "descripcion" varchar
);

CREATE TABLE "OrdenServicio" (
  "idOrden" bigint,
  "idServicio" bigint,
  "Primary" "Key(idOrden,idServicio)"
);

CREATE TABLE "TallerEmpleado" (
  "idTaller" bigint,
  "idEmpleado" bigint,
  "fechaContratacion" datetime DEFAULT (CURRENT_TIMESTAMP),
  "Primary" "Key(idTaller,idEmpleado)"
);

CREATE TABLE "TallerCliente" (
  "idTaller" bigint,
  "idCliente" bigint,
  "fechaRegistro" datetime DEFAULT (CURRENT_TIMESTAMP),
  "Primary" "Key(idTaller,idCliente)"
);

CREATE TABLE "TallerVehiculo" (
  "idTaller" bigint,
  "placaVehiculo" varchar,
  "fechaRegistro" datetime DEFAULT (CURRENT_TIMESTAMP),
  "Primary" "Key(idTaller,placaVehiculo)"
);

CREATE TABLE "InventarioTaller" (
  "idTaller" bigint,
  "idRepuesto" bigint,
  "cantidad" int,
  "stockMinimo" int,
  "Primary" "Key(idTaller,idRepuesto)"
);

COMMENT ON COLUMN "TipoServicio"."descripcion" IS 'REPARACION, REVISION';

COMMENT ON COLUMN "Estado"."descripcion" IS 'En Proceso, Pendiente, Terminado';

COMMENT ON COLUMN "TipoPersona"."descripcion" IS 'NATURAL, JURIDICO, ADMIN, MECANICO, SECRETARIA';

COMMENT ON COLUMN "DetalleTipoPersona"."idTipoPersona" IS 'CLIENTE, EMPLEADO';

COMMENT ON COLUMN "TipoVehiculo"."descripcion" IS 'GASOLINA, ELECTRICO, HIBRIDO';

COMMENT ON COLUMN "TipoOrdenEstado"."descripcion" IS 'En_Progreso, Completada, Cancelada';

ALTER TABLE "Taller" ADD FOREIGN KEY ("id") REFERENCES "InventarioTaller" ("idTaller");

ALTER TABLE "Repuesto" ADD FOREIGN KEY ("id") REFERENCES "InventarioTaller" ("idRepuesto");

ALTER TABLE "Taller" ADD FOREIGN KEY ("id") REFERENCES "TallerVehiculo" ("idTaller");

ALTER TABLE "Vehiculo" ADD FOREIGN KEY ("placa") REFERENCES "TallerVehiculo" ("placaVehiculo");

ALTER TABLE "Taller" ADD FOREIGN KEY ("id") REFERENCES "TallerCliente" ("idTaller");

ALTER TABLE "Persona" ADD FOREIGN KEY ("id") REFERENCES "TallerCliente" ("idCliente");

ALTER TABLE "Taller" ADD FOREIGN KEY ("id") REFERENCES "TallerEmpleado" ("idTaller");

ALTER TABLE "Empleados" ADD FOREIGN KEY ("id") REFERENCES "TallerEmpleado" ("idEmpleado");

ALTER TABLE "Servicio" ADD FOREIGN KEY ("id") REFERENCES "OrdenServicio" ("idServicio");

ALTER TABLE "OrdenDeTrabajo" ADD FOREIGN KEY ("id") REFERENCES "OrdenServicio" ("idOrden");

ALTER TABLE "Vehiculo" ADD FOREIGN KEY ("placa") REFERENCES "OrdenDeTrabajo" ("placaVehiculo");

ALTER TABLE "Persona" ADD FOREIGN KEY ("id") REFERENCES "OrdenDeTrabajo" ("idPersona");

ALTER TABLE "TipoOrdenEstado" ADD FOREIGN KEY ("id") REFERENCES "OrdenDeTrabajo" ("idTipoOrdenEstado");

ALTER TABLE "Vehiculo" ADD FOREIGN KEY ("placa") REFERENCES "VehiculoGasolina" ("placa");

ALTER TABLE "Vehiculo" ADD FOREIGN KEY ("placa") REFERENCES "VehiculoHibrido" ("placa");

ALTER TABLE "Vehiculo" ADD FOREIGN KEY ("placa") REFERENCES "VehiculoElectrico" ("placa");

ALTER TABLE "Persona" ADD FOREIGN KEY ("id") REFERENCES "Vehiculo" ("idPersona");

ALTER TABLE "TipoVehiculo" ADD FOREIGN KEY ("id") REFERENCES "Vehiculo" ("idTipoVehiculo");

ALTER TABLE "Usuarios" ADD FOREIGN KEY ("idPersona") REFERENCES "Persona" ("id");

ALTER TABLE "Proveedor" ADD FOREIGN KEY ("id") REFERENCES "Repuesto" ("idProveedor");

ALTER TABLE "Servicio" ADD FOREIGN KEY ("id") REFERENCES "Revision" ("idServicio");

ALTER TABLE "Servicio" ADD FOREIGN KEY ("id") REFERENCES "Reparacion" ("idServicio");

ALTER TABLE "ServicioMecanicos" ADD FOREIGN KEY ("idServicio") REFERENCES "Servicio" ("id");

ALTER TABLE "Reparacion" ADD FOREIGN KEY ("idServicio") REFERENCES "ReparacionRepuestos" ("idReparacion");

ALTER TABLE "Repuesto" ADD FOREIGN KEY ("id") REFERENCES "ReparacionRepuestos" ("idRepuesto");

ALTER TABLE "TipoServicio" ADD FOREIGN KEY ("id") REFERENCES "Servicio" ("idTipoServicio");

ALTER TABLE "DetalleTipoPersona" ADD FOREIGN KEY ("id") REFERENCES "Persona" ("idTDetalleTipoPersona");

ALTER TABLE "Persona" ADD FOREIGN KEY ("id") REFERENCES "clientesNaturales" ("idPersona");

ALTER TABLE "Persona" ADD FOREIGN KEY ("id") REFERENCES "clientesJuridicos" ("idPersona");

ALTER TABLE "DetalleTipoPersona" ADD FOREIGN KEY ("id") REFERENCES "Empleados" ("idDetalleTipoPersona");

ALTER TABLE "Empleados" ADD FOREIGN KEY ("id") REFERENCES "Mecanico" ("idEmpleado");

ALTER TABLE "TipoPersona" ADD FOREIGN KEY ("id") REFERENCES "DetalleTipoPersona" ("idTipoPersona");

ALTER TABLE "Estado" ADD FOREIGN KEY ("id") REFERENCES "Servicio" ("idEstado");

ALTER TABLE "Mecanico" ADD FOREIGN KEY ("idEmpleado") REFERENCES "ServicioMecanicos" ("idMecanico");
